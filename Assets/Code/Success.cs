using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class Success : Singleton<Success> 
{
    [SerializeField] private Text _text;
    [SerializeField] private string _method;

    void Start() {

        _text.text = "I'm saved!\n\n" + _method + " after " + Clock.DayCounter + " days.";

        StartCoroutine(Reload());
    }

	void Update() {
    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadSceneAsync("Scenes/Intro");
    }
}

