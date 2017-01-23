using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearContainer : MonoBehaviour 
{
	void OnDisable () {
		Inventory.ContainerItems.Clear ();
	}
}
