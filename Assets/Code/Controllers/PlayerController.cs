// 'variable never assigned' for serialized fields
#pragma warning disable 0649

using UnityEngine;
//using System;
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

    public bool HasMap { get; private set; }
    public bool HasOar { get; private set; }
    public bool HasSail { get; private set; }

	// Use this for initialization
	void Start() {
        Hunger = 0.0f;
        Thirst = 0.0f;

        DayNightController.Instance.OnDusk += OnDusk;
        DayNightController.Instance.OnDusk += OnDusk;
        DayNightController.Instance.OnMidnight += OnMidnight;
        DayNightController.Instance.OnDawn += OnDawn;
        DayNightController.Instance.OnSunrise += OnSunrise;
        Inventory.OnItemAdded += OnItemAdded;
	}

    void OnDestroy() {
        if (DayNightController.HasInstance) {
            DayNightController.Instance.OnDusk -= OnDusk;
            DayNightController.Instance.OnDusk -= OnDusk;
            DayNightController.Instance.OnMidnight -= OnMidnight;
            DayNightController.Instance.OnDawn -= OnDawn;
            DayNightController.Instance.OnSunrise -= OnSunrise;
        }
    }

    private bool Nightfall = false;
    void OnSunset(int day) {
        Nightfall = true;
        BoatController.Instance.Stop();
    }

    void OnDusk(int day) {
        In(0.5f, () => {
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

		if (transform.parent.name != "Boat") {
			GameController.Instance.LoseGame ("drowned");
		}

        Hunger += 1.0f;
        Thirst += 1.0f;

        // Go to sleep
        UI.Instance.FadeOut();
    }

    void OnDawn(int day) {
        Nightfall = false;

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

        int dice = Random.Range(0,5);
        if (dice > HasABucket) {
            ++HasABucket;
        }
        
        dice = Random.Range(0,5);
        if (dice > HasANet) {
            ++HasANet;
        }

        if (day == 0) {
            StartCoroutine(Tutorial());
        }
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(1.0f);
        UI.Instance.SetSubtitle("There's an island nearby, and a shipping lane...\n");
        yield return new WaitForSeconds(1.0f);
        UI.Instance.SetSubtitle("There's an island nearby, and a shipping lane...\nI'll mark them on my map.");
        yield return new WaitForSeconds(1.0f);
        UI.Instance.Compass.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        UI.Instance.SetSubtitle("");
        yield return new WaitForSeconds(2.0f);
        UI.Instance.SetSubtitle("But I need supplies first...\nMy oars were in that crate.");
        yield return new WaitForSeconds(2.0f);
    }

    private bool HasUsedOars = false;
    public bool NeedsOars {
        get {
            if (DayNightController.Instance.IsIntro || DayNightController.Instance.IsOutro)
                return false;

            return HasUsedOars == false;
        }
    }

    public string EquippedItem { get; private set; }

    public void UseItem(string item)
    {
        switch (EquippedItem) {
        case "Oars":
            SetState("rowing");
            if (!HasUsedOars) {
                UI.Instance.SetSubtitle("Tap to row");
            }
            break;

        case "Pole":
            SetState("fishing");
            break;
        }
        EquippedItem = item;
    }
    
    public void Interact(Vector2 point)
    {
        if (Nightfall)
            return;

        if (NeedsOars && HasOar && EquippedItem != "Oars") {
            UI.Instance.SetSubtitle("Tap the oars to equip");
        }

        switch (EquippedItem) {
            case "Sail":
            SetState("rowing");
            BoatController.Instance.MoveTowards(point, 1.0f, 0.2f);
            break;

            case "Oars":
            SetState("rowing");
            BoatController.Instance.MoveTowards(point, 0.5f, 0.3f);
            if (HasUsedOars == false) {
                HasUsedOars = true;
                UI.Instance.SetSubtitle("");
            }
            break;

            case "Pole":
            StartCoroutine(Fish());
            break;

            case "Bucket":
            StartCoroutine(CheckBucket());
            break;

            case "Net":
            StartCoroutine(CheckNet());
            break;

            //case "Knife":
            //StartCoroutine(UseKnife());
            //break;
        }
    }

    bool fishing = false;

    IEnumerator Fish() {
        if (fishing)
            yield break;

        fishing = true;
        UI.Instance.SetSubtitle("Fishing...");

        yield return new WaitForSeconds(2.0f);

        if (Random.Range(0,100) < 85) {
            UI.Instance.SetSubtitle("I didn't catch anything.");
        }
        else {
            UI.Instance.SetSubtitle("Got one!  (+1 food)");
            Inventory.AddItem(new InventoryItem("Food"));
        }

        fishing = false;
    }

    int HasABucket = 0;
    IEnumerator CheckBucket() {
        if (fishing)
            yield break;

        fishing = true;

        UI.Instance.SetSubtitle("Checking bucket...");
        yield return new WaitForSeconds(2.0f);

        if (HasABucket > 0) {
            UI.Instance.SetSubtitle("+" + HasABucket + " water");
            for (int i = 0; i < HasABucket; ++i)
                Inventory.AddItem(new InventoryItem("Water"));
            HasABucket = 0;
        }
        else {
            UI.Instance.SetSubtitle("It's empty...");
        }

        fishing = false;

        yield return new WaitForSeconds(1.0f);
        UI.Instance.SetSubtitle("");
    }

    int HasANet = 0;
    IEnumerator CheckNet() {
        UI.Instance.SetSubtitle("Checking net...");
        yield return new WaitForSeconds(2.0f);

        if (Random.Range(0,5) == 0) {
            UI.Instance.SetSubtitle("found a water bottle (+1 water)");
            Inventory.AddItem(new InventoryItem("Water"));
            HasANet = 0;
        }
        else if (HasANet > 0) {
            UI.Instance.SetSubtitle("found some fish (+" + HasANet + " food)");
            for (int i = 0; i < HasANet; ++i)
                Inventory.AddItem(new InventoryItem("Water"));
            HasANet = 0;
        }
        else {
            UI.Instance.SetSubtitle("It's empty...");
        }

        fishing = false;

        yield return new WaitForSeconds(1.0f);
        UI.Instance.SetSubtitle("");
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

    void OnItemAdded(string item)
    {
        /*
        if (item == "Compass") {
            UI.Instance.SetSubtitle("A compass...");
            HasCompass = true;
        }*/
        if (item == "Map") {
            UI.Instance.SetSubtitle("A map!  This will help...");
            HasMap = true;
        }
        if (item == "Oars") {
            //UI.Instance.SetSubtitle("Oars...  I can move a bit quicker with these");
            HasOar = true;
        }
        if (item == "Sail") {
            //HasSail = true;
        }
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
