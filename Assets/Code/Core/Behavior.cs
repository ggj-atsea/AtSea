using UnityEngine;
using System;
using System.Collections;

public abstract class Behavior : MonoBehaviour
{
    protected void In(float seconds, Action action) {
        StartCoroutine(InCo(seconds, action));
    }
    private IEnumerator InCo(float seconds, Action action) {
        yield return new WaitForSeconds(seconds);
        action();
    }
}

