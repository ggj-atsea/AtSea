using UnityEngine;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {

    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;
    private RaycastHit hit;

	void Update () 
    {

#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();
            
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            Vector2 touchPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

#pragma warning disable 472
            if (Physics.Raycast(ray, out hit))
#pragma warning restore 472
            {
                GameObject recipient = hit.transform.gameObject;
                touchList.Add(recipient);
                Debug.Log("Hello from !" + recipient.name);

                if (Input.GetMouseButtonDown(0))
                {
                    recipient.SendMessage("OnTouchDown", touchPos, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", touchPos, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButton(0))
                {
                    recipient.SendMessage("OnTouchStay", touchPos, SendMessageOptions.DontRequireReceiver);
                }
            }
            // No longer being held down
            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    if (g.activeInHierarchy)
                    {
                        g.SendMessage("OnTouchExit", touchPos, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
#endif

        if (Input.touchCount > 0)
        {
            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            foreach (Touch touch in Input.touches)
            {
                Vector2 touchPos = new Vector2(touch.position.x, touch.position.y);

#pragma warning disable 472
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);
                
                if (Physics.Raycast(ray, out hit))
#pragma warning restore 472
                {
                    GameObject recipient = hit.transform.gameObject;
                    touchList.Add(recipient);

                    if (touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", touchPos, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", touchPos, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        recipient.SendMessage("OnTouchStay", touchPos, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", touchPos, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            // No longer being held down
            foreach(GameObject g in touchesOld)
            {
                if(!touchList.Contains(g))
                {
                    Vector2 touchPos = new Vector2(Input.touches[0].position.x, Input.touches[0].position.y);
                    g.SendMessage("OnTouchExit", touchPos, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
	}
}
