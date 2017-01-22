using System.Collections;
using UnityEngine;

public class BoatController : MonoBehaviour, IInteractable
{
    private bool _startSwimmingToBoat = false;

    public void OnTouchDown()
    {
        _startSwimmingToBoat = true;    
    }

    public void OnTouchUp()
    {
    }

    public void OnTouchStay()
    {
    }

    public void OnTouchExit()
    {
    }

    void Update()
    {
        if(_startSwimmingToBoat)
        {
          SwimToBoat();
        }
    }

    public void SwimToBoat()
    {
        if(gameObject.GetComponentInChildren<PlayerController>() != null)
        {
            return;
        }

        var player = PlayerController.Instance.transform;
        player.position = Vector3.MoveTowards(player.position, transform.position, 5 * Time.deltaTime);
    }

    public IEnumerator FlipBoat()
    {
        iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(transform.position.x, transform.position.y, 0f), "easetype", iTween.EaseType.linear, "time", 1.3f));
        yield return new WaitForSeconds(2f);
        var playerController = PlayerController.Instance;
        playerController.transform.SetParent(transform);
        
        if(playerController.GetComponent<FloatController>() != null)
          playerController.GetComponent<FloatController>().enabled = false;
        playerController.transform.localPosition = new Vector3(0, 0.81f, 0);
    }
}