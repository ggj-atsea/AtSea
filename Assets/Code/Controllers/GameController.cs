﻿
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameController : Singleton<GameController>
{
    public bool GameOver { get; private set; }

    private string _message = null;

	// Use this for initialization
	void Start() {
        GameOver = false;
	}

    void OnDestroy() {
    }

    public void WinGame(string reason) {
        GameOver = true;
        _message = "You " + reason + " after " + Clock.Instance.Day + " days...";
    }

    public void LoseGame(string reason) {
        GameOver = true;
        _message = "You " + reason + " after " + Clock.Instance.Day + " days...";
    }

    public void Restart() {
        SceneManager.LoadScene("Main");
    }

    public void ShowEnding() {
        UI.Instance.GameOver(_message);
        In(15.0f, () => Restart());
    }
}
