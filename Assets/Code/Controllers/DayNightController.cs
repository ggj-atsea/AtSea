using UnityEngine;
using System;
using System.Collections;

public class DayNightController : Singleton<DayNightController> 
{
    [SerializeField] private float _sunriseAngle;
    [SerializeField] private float _sunriseStart;
    [SerializeField] private float _sunriseEnd;
    [SerializeField] private float _sunsetStart;
    [SerializeField] private float _sunsetEnd;

    [SerializeField] private GameObject _water;
    [SerializeField] private GameObject _backgroundWater;
    [SerializeField] private Light _light;

    [SerializeField] private Color _nightColor;
    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _stormColor;
    [SerializeField] private Color _rainColor;
    [SerializeField] private float _brightnessNight;
    [SerializeField] private float _brightnessDay;
    [SerializeField] private float _brightnessStorm;
    [SerializeField] private float _brightnessLightning;
    [SerializeField] private float _brightnessRain;
    [SerializeField] private float _stormLightVariance;
    [SerializeField] private float _rainLightVariance;

    [SerializeField] private float _lightningStart;
    [SerializeField] private float _lightningEnd;

    public event Action<int> OnDawn;
    public event Action<int> OnSunrise;
    public event Action<int> OnSunset;
    public event Action<int> OnDusk;
    public event Action<int> OnMidnight;

    public bool IsRaining { get; set; }
    public bool IsStorm { get; set; }

    private float _lastTime = -1.0f;
    private float _noiseSeed = 0.0f;

    private float lightning;

    void Start() {
        // Initial day is always a storm
        IsStorm = true;
    }

    // Update weather, etc at end of day
    void AdvanceDay() {
        float roll = UnityEngine.Random.Range(0.0f,1.0f);

        if (IsStorm) {
            // Chance of rain after a storm
            if (roll < 0.4f) {
                IsRaining = true;
            }
            IsStorm = false;
        }

        else if (IsRaining) {
            // Go back to sunshine after rain
            IsRaining = false;
        }

        else {
            // Small chance of storm
            if (roll < 0.15f) {
                IsStorm = true;
            }
            // And small chance of rain
            else if (roll < 0.3f) {
                IsRaining = true;
            }
        }

        // Randomize the noise factor each day
        _noiseSeed = UnityEngine.Random.Range(0.0f,10.0f);
    }

	// Update is called once per frame
	void Update() {
        var time = Clock.Instance.Hour;
        var day = Clock.Instance.Day;

        Color dayColor = (IsStorm ? _stormColor :
                         (IsRaining ? _rainColor : _dayColor));

        float maxBrightness = (IsStorm ? _brightnessStorm :
                               IsRaining ? _brightnessRain : _brightnessDay);

        if (IsStorm)
        {
            lightning += Time.deltaTime;
            if (lightning > _lightningStart) {
                maxBrightness = maxBrightness + _brightnessLightning * (1.0f - (lightning - _lightningStart) / (_lightningEnd - _lightningStart));

                if (lightning > _lightningEnd) {
                    float roll = UnityEngine.Random.Range(0.0f,1.0f);
                    if (roll < 0.55f) {
                        lightning = UnityEngine.Random.Range(0,1.5f);
                    }
                    else {
                        lightning = UnityEngine.Random.Range(0.97f, 1.1f) * _lightningStart;
                    }
                }
            }
        }

        // Set color and intensity based on sunrise/sunset
        if (time < _sunriseStart) {
            SetEnvironment(_nightColor, _nightColor, 0.0f);
            SetLightIntensity(0.0f, 0.0f, 0.0f);

            if (_lastTime > time && OnMidnight != null) {
                AdvanceDay();
                OnMidnight(day);
            }
        }
        else if (time < _sunriseEnd) {
            float percent = (time - _sunriseStart) / (_sunriseEnd - _sunriseStart);
            SetEnvironment(_nightColor, dayColor, percent);
            SetLightIntensity(0.0f, maxBrightness, percent);

            if (_lastTime < _sunriseStart && OnDawn != null) {
                OnDawn(day);
            }
        }
        else if (time < _sunsetStart) {
            SetEnvironment(dayColor, dayColor, 0.0f);
            SetLightIntensity(maxBrightness, maxBrightness, 0.0f);

            if (_lastTime < _sunriseEnd && OnSunrise != null) {
                OnSunrise(day);
            }
        }
        else if (time < _sunsetEnd) {
            float percent = (time - _sunsetStart) / (_sunsetEnd - _sunsetStart);
            SetEnvironment(dayColor, _nightColor, percent);
            SetLightIntensity(maxBrightness, 0.0f, percent);

            if (_lastTime < _sunsetStart && OnSunset != null) {
                OnSunset(day);
            }
        }
        else { // time >= _sunsetEnd)
            SetEnvironment(_nightColor, _nightColor, 0.0f);
            SetLightIntensity(0.0f, 0.0f, 0.0f);

            if (_lastTime < _sunsetEnd && OnDusk != null) {
                OnDusk(day);
            }
        }

        _lastTime = time;

        // Set position from sunriseStart to sunsetEnd
        float sunPos = (time - _sunriseStart) / (_sunsetEnd - _sunriseStart);
        sunPos = Wrap(sunPos, 0.0f, 1.0f);
        SetLightAngle(90.0f - _sunriseAngle, 90.0f + _sunriseAngle, sunPos);
	}

    // Lighting modifiers
    private void SetEnvironment(Color start, Color end, float percent) {
        var color = Interp(start, end, percent);
        _water.GetComponent<Renderer>().material.color = color;
        _backgroundWater.GetComponent<Renderer>().material.color = color;
    }

    private void SetLightAngle(float start, float end, float percent) {
        var angle = Interp(start, end, percent);
        _light.transform.eulerAngles = new Vector3(angle, 90.0f, 0.0f);
    }

    private void SetLightIntensity(float start, float end, float percent) {
        _light.intensity = (start + (end - start) * percent) * LightVariance;
    }

    private float LightVariance {
        get {
            if (IsStorm) {
                float perlin = Mathf.PerlinNoise(4 * Clock.Instance.Hour, _noiseSeed);
                return 1.0f + (1.0f - perlin * 2.0f) * _stormLightVariance;
            }
            if (IsRaining) {
                float perlin = Mathf.PerlinNoise(2 * Clock.Instance.Hour, _noiseSeed);
                return 1.0f + (1.0f - perlin * 2.0f) * _rainLightVariance;
            }
            else {
                return 1.0f;
            }
        }
    }

    // Math helpers
    private Color Interp(Color a, Color b, float percent) {
        return new Color(a.r + (b.r - a.r) * percent,
                         a.g + (b.g - a.g) * percent,
                         a.b + (b.b - a.b) * percent,
                         a.a + (b.a - a.a) * percent);

    }

    private float Interp(float a, float b, float percent) {
        return a + (b - a) * percent;
    }
    private float Wrap(float num, float min, float max) {
        float range = max - min;
        while (num > max)   num -= range;
        while (num < min)   num += range;
        return num;
    }
}
