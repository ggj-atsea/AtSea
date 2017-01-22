using UnityEngine;

public class BoardBoat : MonoBehaviour {

	void OnTriggerEnter(Collider collider)
	{
		if(collider.name == "Boat")
		{
			GetComponent<BoxCollider>().enabled = false;
			StartCoroutine(collider.GetComponent<BoatController>().FlipBoat());
		}
	}
}
