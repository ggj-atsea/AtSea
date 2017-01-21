using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Clock))]
public class DayNight : MonoBehaviour
{
    [SerializeField] private float _sunriseAngle;
    [SerializeField] private float _sunriseStart;
    [SerializeField] private float _sunriseEnd;
    [SerializeField] private float _sunsetStart;
    [SerializeField] private float _sunsetEnd;

    [SerializeField] private GameObject _water;
    [SerializeField] private Light _light;

    [SerializeField] private Color _dayColor;
    [SerializeField] private Color _nightColor;

    private Clock _clock;

	// Use this for initialization
	void Start() {
        _clock = GetComponent<Clock>();
	}
	
	// Update is called once per frame
	void Update() {
        var time = _clock.Hour;

        // Set color and intensity based on sunrise/sunset
        if (time < _sunriseStart || time > _sunsetEnd) {
            SetEnvironment(_nightColor, _nightColor, 0.0f);
            SetLightIntensity(0.0f, 0.0f, 0.0f);
            UI.Instance.SetSubtitle("Night...");
        }
        else if (time < _sunriseEnd) {
            float percent = (time - _sunriseStart) / (_sunriseEnd - _sunriseStart);
            SetEnvironment(_nightColor, _dayColor, percent);
            SetLightIntensity(0.0f, 1.0f, percent);
            UI.Instance.SetSubtitle("Sunrise.");

            if (time < _sunriseStart + 0.1f)
                UI.Instance.SetDay(_clock.Day);
        }
        else if (time < _sunsetStart) {
            SetEnvironment(_dayColor, _dayColor, 0.0f);
            SetLightIntensity(1.0f, 1.0f, 0.0f);
            UI.Instance.SetSubtitle("Daytime.");
        }
        else /*if (time < _sunsetEnd)*/ {
            float percent = (time - _sunsetStart) / (_sunsetEnd - _sunsetStart);
            SetEnvironment(_dayColor, _nightColor, percent);
            SetLightIntensity(1.0f, 0.0f, percent);
            UI.Instance.SetSubtitle("Sunset.");
        }

        // Set position from sunriseStart to sunsetEnd
        float sunPos = (time - _sunriseStart) / (_sunsetEnd - _sunriseStart);
        sunPos = Wrap(sunPos, 0.0f, 1.0f);
        SetLightAngle(90.0f - _sunriseAngle, 90.0f + _sunriseAngle, sunPos);
	}

    // Lighting modifiers
    private void SetEnvironment(Color start, Color end, float percent) {
        var color = Interp(start, end, percent);
        _water.GetComponent<Renderer>().material.color = color;
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
