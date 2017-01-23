
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class IslandController : Behavior, IInteractable
{
    [SerializeField] private bool island = false;

    public void OnTouchDown(Vector2 point)
    {
        if (island)
            GameController.Instance.WinGame("made it to shore");
        else
            GameController.Instance.WinGame("were rescued by a passing ship");

        GameController.Instance.ShowEnding();
    }

    public void OnTouchUp(Vector2 point)
    {
    }

    public void OnTouchStay(Vector2 point)
    {
    }

    public void OnTouchExit(Vector2 point)
    {
    }
}

