// 'variable never assigned' for serialized fields
#pragma warning disable 0649

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Singleton<PlayerController>
{
    public float Hunger { get; private set; }
    public float Thirst { get; private set; }
    public float Fatigue { get { return Hunger + Thirst; } }

    public float HungerMax { get { return 10.0f; } }
    public float ThirstMax { get { return 4.0f; } }
    public float FatigueMax { get { return HungerMax + ThirstMax; } }

    public float HungerPercent() { return Hunger / HungerMax; }
    public float ThirstPercent() { return Thirst / ThirstMax; }
    public float FatiguePercent() { return Fatigue / FatigueMax; }

    private bool _gameOver = false;

	// Use this for initialization
	void Start() {
        Hunger = 0.0f;
        Thirst = 0.0f;

        DayNightController.Instance.OnDusk += OnDusk;
        DayNightController.Instance.OnMidnight += OnMidnight;
        DayNightController.Instance.OnDawn += OnDawn;
	}

    void OnDestroy() {
        if (DayNightController.HasInstance) {
            DayNightController.Instance.OnDusk -= OnDusk;
            DayNightController.Instance.OnMidnight -= OnMidnight;
            DayNightController.Instance.OnDawn -= OnDawn;
        }
    }

    void OnDusk(int day) {
        In(3.0f, () => SetState("asleep"));
    }

    void OnMidnight(int day) {
        if (Hunger >= HungerMax || Thirst >= ThirstMax)
            _gameOver = true;

        Hunger += 1.0f;
        Thirst += 1.0f;

        // Go to sleep
        UI.Instance.FadeOut();
    }

    void OnDawn(int day) {
        if (_gameOver) {
            UI.Instance.GameOver();
        }
        else {
            // Wake up
            UI.Instance.FadeIn();
            UI.Instance.SetDay(day);

            HUD.Instance.UpdateStats(this);

            In(1.0f, () => SetState("standing"));
        }
    }

    public void EatFood()
    {
        Hunger -= 5.0f;
        if(Hunger <= 0)
            Hunger = 0;
 
        HUD.Instance.UpdateStats(this);
    }

    public void DrinkWater()
    {
        Thirst -= 5.0f;
        if(Thirst <= 0)
            Thirst = 0;
 
        HUD.Instance.UpdateStats(this);
    }

    private void In(float seconds, Action action) {
        StartCoroutine(InCo(seconds, action));
    }
    private IEnumerator InCo(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action();
    }

    #region State Management

    [System.Serializable]
    private class State {
        public string _name;
        public GameObject _state;
    }

    [SerializeField] private State[] _playerStates;

    public void SetState(string name) {
        foreach (var state in _playerStates) {
            if (state._state != null)
                state._state.SetActive(state._name == name);
        }
    }

    #endregion
}
