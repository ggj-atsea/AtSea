using UnityEngine;

public class WaveController : MonoBehaviour 
{
	public float height;
	public float time;
	private Cloth _cloth;
	private float TurbulenceMin = 0f;
	private float TurbulenceMax = 20f;
	[SerializeField] private float CapsizeSpeed = 10f;
	private Transform _boat;

	// Use this for initialization
	void Start ()
	{
		_cloth = GetComponent<Cloth>();
		if(transform.childCount > 0)
		{
			_boat = transform.GetChild(0);
		}
		GenerateWaves();

		DayNightController.Instance.OnMidnight += OnMidnight;
        DayNightController.Instance.OnDawn += OnDawn;
		DayNightController.Instance.OnDusk += OnDusk;
		DayNightController.Instance.OnSunrise += OnSunrise;
		DayNightController.Instance.OnSunrise += OnSunset;
	}

	void OnDestroy()
	{
		if(DayNightController.HasInstance)
		{
			DayNightController.Instance.OnMidnight -= OnMidnight;
			DayNightController.Instance.OnDawn -= OnDawn;
			DayNightController.Instance.OnDusk -= OnDusk;
			DayNightController.Instance.OnSunrise -= OnSunrise;
			DayNightController.Instance.OnSunrise -= OnSunset;
		}
	}

	public void AccelerationCheck()
	{
		var largestAcceleration = Mathf.Max(
			_cloth.externalAcceleration.x + _cloth.externalAcceleration.y + _cloth.externalAcceleration.z,
			_cloth.randomAcceleration.x + _cloth.randomAcceleration.y + _cloth.randomAcceleration.z
		);

		if(largestAcceleration > 50)
		{
			Capsize();
			DetachPlayer();
		}
	}

	public void Capsize()
	{
		if(_boat != null)
			iTween.RotateTo(_boat.gameObject, iTween.Hash("rotation", new Vector3(transform.position.x, transform.position.y, 180f), "easetype", iTween.EaseType.easeInOutSine, "time", 1.3f));
	}

	public void DetachPlayer()
	{
		if(_boat != null)
		{
			var player = _boat.GetChild(0);
			player.SetParent(this.transform);
			player.gameObject.AddComponent<FloatController>();
			player.transform.position = new Vector3(player.transform.position.x + 5, player.transform.position.y, player.transform.position.z + 5);
			player.GetComponent<BoxCollider>().enabled = true;
		}
	}

	public void OnMidnight(int day)
	{
		CalmWaves();
		Turbulence();
		AccelerationCheck();
	}

	public void OnDawn(int day)
	{
		CalmWaves();
	}

	public void OnDusk(int day)
	{
		CalmWaves();
		Turbulence();
		AccelerationCheck();
	}

	public void OnSunrise(int day)
	{
		CalmWaves();
		Turbulence();
		AccelerationCheck();
	}

	public void OnSunset(int day)
	{
		CalmWaves();
		Turbulence();
		AccelerationCheck();
	}

	public void GenerateWaves()
	{
		iTween.MoveBy(this.gameObject, iTween.Hash("y", height, "time", time, "looptype", "pingpong", "easetype", iTween.EaseType.easeInOutSine));
	}

	public void Turbulence()
	{
		var turbulenceFactor = Random.Range(0, 3);

		switch(turbulenceFactor)
		{
			case 0: 
				_cloth.externalAcceleration = new Vector3(
					Random.Range(TurbulenceMin, TurbulenceMax), 
					Random.Range(TurbulenceMin, TurbulenceMax), 
					Random.Range(TurbulenceMin, TurbulenceMax));
				break;
			case 1:
				_cloth.randomAcceleration = new Vector3(
					Random.Range(TurbulenceMin, TurbulenceMax), 
					Random.Range(TurbulenceMin, TurbulenceMax), 
					Random.Range(TurbulenceMin, TurbulenceMax));
				break;
			case 2:
				_cloth.worldAccelerationScale = Random.Range(0f, 1f);
				break;
			case 3: 
				CalmWaves();
				break;
			default:
				CalmWaves();
				break;
		}
	}

	public void CalmWaves()
	{
		_cloth.worldAccelerationScale = 0.8f;
				_cloth.randomAcceleration = new Vector3(0f, 0f, 0f);
				_cloth.externalAcceleration = new Vector3(0f, 0f, 0f);
	}
}
