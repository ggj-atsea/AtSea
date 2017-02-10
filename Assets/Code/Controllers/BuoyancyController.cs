using System.Collections;
using UnityEngine;

public class BuoyancyController : MonoBehaviour {

	// The higher the force, the faster it moves
	[SerializeField] private float _wobbleForce = 40;

	void OnEnable()
	{
		StartCoroutine(LoopRotation());
	}

	void OnDisable()
	{
		StopCoroutine(LoopRotation());
	}

	IEnumerator LoopRotation()
	{
		float rot = 0f;
		float dir = 1f;
		while(true)
		{
			while(rot < 60)
			{
				float step = Time.deltaTime * _wobbleForce;
				transform.Rotate(new Vector3(1, 0, 1) * step * dir);
				rot += 1;
				yield return null;
			}
			rot= 0f;
			dir *= -1f;
		}
	}

}
