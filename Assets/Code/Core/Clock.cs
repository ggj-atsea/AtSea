using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clock : Singleton<Clock>
{
    [SerializeField] private float _secondsInDay = 60.0f;

    [SerializeField] private float _hour;
    [SerializeField] private int _day;

    public float Hour { get { return _hour; } }
    public int Day { get { return _day; } }

    public static int DayCounter;

	void Start() {
        _hour = 4.0f;
        _day = 0;
	}
	
	void Update()
    {
        if (GameController.Instance.GameStarted)
        {
            if (_hour > 16 && PlayerController.Instance.NeedsOars)
            {
                return;
            }

            _hour += (UnityEngine.Time.deltaTime * 24) / _secondsInDay;
            if (_hour > 24.0f && GameController.Instance.GameOver == false) {
                _hour -= 24.0f;
                ++_day;

                DayCounter = _day;
            }
        }
	}

    public void FastForwardToEOD()
    {
        if (_hour < 18)
            _hour = 18;
    }
}

