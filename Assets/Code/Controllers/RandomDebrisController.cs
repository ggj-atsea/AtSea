using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDebrisController : MonoBehaviour {

	public FloatingContainers[] floatingContainers;
	private Transform _sea;

	// Use this for initialization
	void Start () {
		_sea = this.gameObject.transform.parent;

		DayNightController.Instance.OnDawn += OnDawn;

        Day0();
	}
	
	public void OnDawn(int day)
	{
		foreach(var container in floatingContainers)
		{	
			var random = Random.Range (0, 6);
			if (random >= 3) {
				container.transform.parent = _sea;
				container.gameObject.SetActive (true);
			}
		}
	}

    public void Day0()
    {
        int i = 0;
		foreach(var container in floatingContainers)
		{	
            if (i <= 1) {
				container.transform.parent = _sea;
				container.gameObject.SetActive (true);
                container.Day0(i);
            }
            ++i;
		}
    }
}
