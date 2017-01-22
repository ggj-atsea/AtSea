using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
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

        DayNightController.Instance.OnMidnight += OnMidnight;
	}

    void OnDestroy() {
        if (DayNightController.HasInstance)
            DayNightController.Instance.OnMidnight -= OnMidnight;
    }

    void OnMidnight(int day) {
        Hunger += 1.0f;
        Thirst += 1.0f;

        HUD.Instance.UpdateStats(this);
    }

    public void EatFood()
    {
        Hunger -= 5.0f;
        if(Hunger <= 0)
            Hunger = 0;
    }

    public void DrinkWater()
    {
        Thirst -= 5.0f;
        if(Thirst <= 0)
            Thirst = 0;
    }
}
