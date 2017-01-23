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

	// Use this for initialization
	void Start() {
        Hunger = 0.0f;
        Thirst = 0.0f;

        DayNightController.Instance.OnDusk += OnDusk;
        DayNightController.Instance.OnMidnight += OnMidnight;
        DayNightController.Instance.OnDawn += OnDawn;
        DayNightController.Instance.OnSunrise += OnSunrise;
	}

    void OnDestroy() {
        if (DayNightController.HasInstance) {
            DayNightController.Instance.OnDusk -= OnDusk;
            DayNightController.Instance.OnMidnight -= OnMidnight;
            DayNightController.Instance.OnDawn -= OnDawn;
            DayNightController.Instance.OnSunrise -= OnSunrise;
        }
    }

    void OnDusk(int day) {
        In(2.0f, () => {
            SetState("sleeping");
            BoatController.Instance.ShowCanopy();
           });
    }

    void OnMidnight(int day) {
        if (Hunger >= HungerMax) {
            GameController.Instance.LoseGame("died of starvation");
        }
        else if (Thirst >= ThirstMax) {
            GameController.Instance.LoseGame("died from dehydration");
        }

        Hunger += 1.0f;
        Thirst += 1.0f;

        // Go to sleep
        UI.Instance.FadeOut();
    }

    void OnDawn(int day) {
        if (GameController.Instance.GameOver) {
            GameController.Instance.ShowEnding();
        }
        else {
            // Wake up
            UI.Instance.FadeIn();
            UI.Instance.SetDay(day);

            HUD.Instance.UpdateStats(this);

            In(1.0f, () => {
               SetState("standing");
               BoatController.Instance.HideCanopy();
            });
        }
    }

    void OnSunrise(int day) {
        string sub = null;

        if (LandmarkController.Instance.NearShip())
        {
            sub = "A ship!  I hope they're friendly...";
        }
        else if (LandmarkController.Instance.NearIsland())
        {
            sub = "An island!  I'm saved!";
        }
        else if (Hunger >= HungerMax / 2) {
            sub = "I'm starving...";
        }
        else if (Thirst >= ThirstMax / 2) {
            sub = "I'm thirsty...";
        }

        if (sub != null)
            UI.Instance.SetSubtitle(sub);
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
        Thirst -= 2.0f;
        if(Thirst <= 0)
            Thirst = 0;
 
        HUD.Instance.UpdateStats(this);
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
