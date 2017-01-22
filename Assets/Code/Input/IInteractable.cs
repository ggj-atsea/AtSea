using UnityEngine;

public interface IInteractable
{
	void OnTouchDown(Vector2 pos);
	void OnTouchUp(Vector2 pos);
	void OnTouchStay(Vector2 pos);
	void OnTouchExit(Vector2 pos);
}
