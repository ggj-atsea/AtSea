using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class InventoryCard : MonoBehaviour {

    [SerializeField] private Image _icon;

    private Action _onClick;

    public void Setup(string id, Action onClick)
    {
        _icon.sprite = Resources.Load<Sprite>("Resources/Sprites/" + id);
        _onClick = onClick;
    }

    #region UI Callbacks

    public void OnClick()
    {
        if (_onClick != null)
            _onClick();
    }

    #endregion
}

