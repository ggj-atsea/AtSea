using System.Collections;
using UnityEngine;

public class OceanController : Singleton<OceanController>, IInteractable
{
    public void OnTouchDown(Vector2 point)
    {
        BoatController.Instance.MoveTowards(point);
    }

    public void OnTouchUp(Vector2 point)
    {
    }

    public void OnTouchStay(Vector2 point)
    {
    }

    public void OnTouchExit(Vector2 point)
    {
    }
}
