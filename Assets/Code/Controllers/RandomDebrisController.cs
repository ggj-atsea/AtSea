using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDebrisController : MonoBehaviour {

	public Transform[] floatingContainers;
	private Transform _sea;

	// Use this for initialization
	void Start () {
		floatingContainers = GetComponentsInChildren<Transform>(true);
		_sea = this.gameObject.transform.parent;

		DayNightController.Instance.OnDawn += OnDawn;
	}
	
	public void OnDawn(int day)
	{
		foreach(var container in floatingContainers)
		{
			container.parent = _sea;
			container.gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
