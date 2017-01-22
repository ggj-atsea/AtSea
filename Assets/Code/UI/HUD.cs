using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HUD : Singleton<HUD> {

    [SerializeField] private Image _hunger;
    [SerializeField] private Image _thirst;
    [SerializeField] private Image _fatigue;

    public void UpdateStats(PlayerController player)
    {
        _hunger.fillAmount = player.HungerPercent();
        _thirst.fillAmount = player.ThirstPercent();
        _fatigue.fillAmount = player.FatiguePercent();
    }
}

