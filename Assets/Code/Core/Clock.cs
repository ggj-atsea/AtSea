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

	void Start() {
        _hour = 4.0f;
        _day = 0;
	}
	
	void Update()
    {
        if (GameController.Instance.GameStarted)
        {
            _hour += (UnityEngine.Time.deltaTime * 24) / _secondsInDay;
            if (_hour > 24.0f && GameController.Instance.GameOver == false) {
                _hour -= 24.0f;
                ++_day;
            }
        }
	}
}

