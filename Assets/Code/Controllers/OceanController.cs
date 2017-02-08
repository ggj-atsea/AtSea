using System.Collections;
using UnityEngine;

public class OceanController : Singleton<OceanController>, IInteractable
{
    private Renderer _renderer;
    private Vector2 _seaPos;

    public void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void OnTouchDown(Vector2 point)
    {
        PlayerController.Instance.MoveTowards(point);
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

    public void Update()
    {
        var offset = BoatController.Instance.Velocity * Time.deltaTime;

        _seaPos += new Vector2(-offset.x, -offset.z);
        _seaPos = new Vector2(MathHelper.Wrap(_seaPos.x, -1.0f, 1.0f),
                              MathHelper.Wrap(_seaPos.y, -1.0f, 1.0f));

        _renderer.material.SetTextureOffset("_MainTex", _seaPos);
    }
}
