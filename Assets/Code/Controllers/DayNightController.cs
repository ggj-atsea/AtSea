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

    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;

    public event Action<int> OnDawn;
    public event Action<int> OnSunrise;
    public event Action<int> OnSunset;
    public event Action<int> OnDusk;
    public event Action<int> OnMidnight;

    private float _lastTime = -1.0f;

	// Update is called once per frame
	void Update() {
        var time = Clock.Instance.Hour;
        var day = Clock.Instance.Day;

        // Set color and intensity based on sunrise/sunset
        if (time < _sunriseStart) {
            SetEnvironment(_nightColor, _nightColor, 0.0f);
            SetLightIntensity(0.0f, 0.0f, 0.0f);

            if (_lastTime > time && OnMidnight != null)
                OnMidnight(day);
        }
        else if (time < _sunriseEnd) {
            float percent = (time - _sunriseStart) / (_sunriseEnd - _sunriseStart);
            SetEnvironment(_nightColor, _dayColor, percent);
            SetLightIntensity(0.0f, 1.0f, percent);

            if (_lastTime < _sunriseStart && OnDawn != null) {
                OnDawn(day);
            }
        }
        else if (time < _sunsetStart) {
            SetEnvironment(_dayColor, _dayColor, 0.0f);
            SetLightIntensity(1.0f, 1.0f, 0.0f);

            if (_lastTime < _sunriseEnd && OnSunrise != null) {
                OnSunrise(day);
            }
        }
        else if (time < _sunsetEnd) {
            float percent = (time - _sunsetStart) / (_sunsetEnd - _sunsetStart);
            SetEnvironment(_dayColor, _nightColor, percent);
            SetLightIntensity(1.0f, 0.0f, percent);

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
        _light.intensity = (start + (end - start) * percent);
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
