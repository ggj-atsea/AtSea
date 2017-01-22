using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private Camera _camera;
	[SerializeField] private Transform _player;
	[SerializeField] private Transform _boat;

	private const float MaxDistanceY = 17.4f;
	//Towards the bottom of the screen;
	private const float MaxDistanceZ = -1.5f;
	// Towards the top of the screen
	private const float MinDistanceZ = -18.02f;
	private const float MinDistanceX = -3f;
	private const float MaxDistanceX = 3f;
	private const float OriginalPositionY = 15f;
	private const float OriginalPositionZ = -9f;

	void Update()
	{
		if(_boat.position.y > -0.2f && _boat.position.y < 0)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(45f, transform.rotation.y, transform.rotation.z), 3);
		}
		else if(_boat.position.y >= 0)
		{
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(65f, transform.rotation.y, transform.rotation.z), 3);
			iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, MaxDistanceY, MaxDistanceZ), "time", 3, "easetype", iTween.EaseType.easeInOutSine));
		}

		// if(transform.rotation.x < 58 && transform.rotation.x > 50)
		// {
		// 	iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, 11f, -5f), "time", 3, "easetype", iTween.EaseType.easeInOutSine));
		// }
		else if(transform.rotation.x <= 50)
		{
			iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, 8.9f, -8.6f), "time", 3, "easetype", iTween.EaseType.easeInOutSine));
			iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, MaxDistanceY, MaxDistanceZ), "time", 3, "easetype", iTween.EaseType.easeInOutSine));
		}
		else if(transform.rotation.x < 45)
		{
			
		}
	}

	private void TrackPlayer()
	{
		if(_boat.position.y > 0.9f)
		{
			iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(transform.position.x, MaxDistanceY, MaxDistanceZ), "time", 3, "easetype", iTween.EaseType.easeInOutSine));
		}
	}	
	
}
