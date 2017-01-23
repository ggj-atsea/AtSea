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
			var random = Random.Range (0, 6);
			if (random == 3) {
				container.parent = _sea;
				container.gameObject.SetActive (true);
			}
		}
	}
}
