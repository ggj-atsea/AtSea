using UnityEngine;

public class WaveController : MonoBehaviour 
{
	public float height;
	public float time;
	private Cloth _cloth;
	private float TurbulenceMin = 0f;
	private float TurbulenceMax = 20f;

	// Use this for initialization
	void Start ()
	{
		_cloth = GetComponent<Cloth>();
		GenerateWaves();

		DayNightController.Instance.OnMidnight += OnMidnight;
        DayNightController.Instance.OnDawn += OnDawn;
		DayNightController.Instance.OnDusk += OnDusk;
		DayNightController.Instance.OnSunrise += OnSunrise;
		DayNightController.Instance.OnSunrise += OnSunset;
	}

	public void OnMidnight(int day)
	{
		CalmWaves();
		Turbulence();
	}

	public void OnDawn(int day)
	{
		CalmWaves();
	}

	public void OnDusk(int day)
	{
		CalmWaves();
		Turbulence();
	}

	public void OnSunrise(int day)
	{
		CalmWaves();
		Turbulence();
	}

	public void OnSunset(int day)
	{
		CalmWaves();
		Turbulence();
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
