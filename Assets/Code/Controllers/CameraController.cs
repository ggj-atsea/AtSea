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

	void Start()
	{
		StartCoroutine("WiggleCamera");
	}

	void Update()
	{
		_camera.transform.LookAt(_player);
		_camera.transform.position = new Vector3(_player.position.x, _player.position.y + 15, _player.position.z - 9);
	}
}
