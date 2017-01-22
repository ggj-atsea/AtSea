using System.Collections;
using UnityEngine;

public class BoatController : Singleton<BoatController>, IInteractable
{
    private bool _startSwimmingToBoat = false;
    private float _velocity = 1.0f;
    private Vector3 _motion;

    [SerializeField] private GameObject _trail;

    void Start()
    {
        DayNightController.Instance.OnDawn += OnDawn;
        DayNightController.Instance.OnDusk += OnDusk;
    }

    void OnDawn(int day)
    {
        transform.position = new Vector3();
        _motion = new Vector3();
    }

    void OnDusk(int day)
    {
        LandmarkController.Instance.MovePlayer(_motion);
    }

    public void OnTouchDown(Vector2 point)
    {
        _startSwimmingToBoat = true;
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

    public void MoveTowards(Vector2 point)
    {
        _motion = new Vector3(point.x - Screen.width / 2, 0, point.y - Screen.height / 2).normalized * _velocity;
    }

    void Update()
    {
        if(_startSwimmingToBoat)
        {
            SwimToBoat();
        }

        if (_motion.magnitude > 0.1f) {
            transform.position += _motion * Time.deltaTime;


            float angle = Vector3.Angle(_motion, new Vector3(0,0,1));

            if (_motion.x < 0)
                angle = -angle;

            _trail.SetActive(true);
            _trail.transform.localEulerAngles = new Vector3(0,0,angle);
        }
        else {
            _trail.SetActive(false);
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
