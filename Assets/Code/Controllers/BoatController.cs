using System.Collections;
using UnityEngine;

public class BoatController : Singleton<BoatController>, IInteractable
{
    private bool _startSwimmingToBoat = false;
    private Vector3 _motion;
    private float _decay;
    private float _velocity;

    [SerializeField] private GameObject _trail;
    [SerializeField] private GameObject _canopy;
    [SerializeField] private GameObject _light;
    [SerializeField] private ParticleSystem _trailSys;
    [SerializeField] private ParticleSystem _splashSys;

    public Vector3 Velocity {
        get { return _motion; }
    }

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
        LandmarkController.Instance.MovePlayer(transform.position);
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

    public void MoveTowards(Vector2 point, float velocity, float decay)
    {
        _velocity = velocity;
        _decay = decay;
        _motion = new Vector3(point.x - Screen.width / 2, 0, point.y - Screen.height / 2).normalized * velocity;
        Debug.Log("Moving to " + point+ " with velocity " + velocity + " and decay " + decay);
    }

    void Update()
    {
        if(_startSwimmingToBoat)
        {
            SwimToBoat();
        }

        if (_motion.magnitude > 0.1f) {
            _motion *= 1.0f - (_decay * Time.deltaTime);

            transform.position += _motion * Time.deltaTime;

            float rate = (_motion.magnitude / _velocity);
            if (rate > 1.0f)
                rate = 1.0f;

            float angle = Vector3.Angle(_motion, new Vector3(0,0,1));

            if (_motion.x < 0)
                angle = -angle;

            _trail.SetActive(true);
            _trail.transform.localEulerAngles = new Vector3(0,0,angle);

            /*
            var em = _trailSys.emission.rateOverTime;
            em.mode = ParticleSystemCurveMode.Constant;
            em.constantMin = 2.0f * rate;
            em.constantMax = 2.0f * rate;
            em = _splashSys.emission.rateOverTime;
            em.mode = ParticleSystemCurveMode.Constant;
            em.constantMin = 8.0f * rate;
            em.constantMax = 8.0f * rate;
            */
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
        iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(transform.position.x, transform.position.y, 0f), "easetype", iTween.EaseType.linear, "time", 0.5f));
		iTween.MoveTo (gameObject, iTween.Hash ("position", new Vector3 (transform.position.x, 0f, transform.position.z), "easetype", iTween.EaseType.linear, "time", 0.5f));
		yield return new WaitForSeconds(1f);
        var playerController = PlayerController.Instance;
		playerController.transform.localPosition = new Vector3(0, 0.81f, 0);
        playerController.transform.SetParent(transform);
        
        if(playerController.GetComponent<FloatController>() != null)
          playerController.GetComponent<FloatController>().enabled = false;
    }

    public void ShowCanopy()
    {
        _canopy.SetActive(true);
        _light.SetActive(false);
    }

    public void HideCanopy()
    {
        _canopy.SetActive(false);
        _light.SetActive(true);
    }
}
