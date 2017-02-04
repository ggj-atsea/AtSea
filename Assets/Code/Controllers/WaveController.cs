using UnityEngine;

public class WaveController : MonoBehaviour 
{
	public float height;
	public float time;
	private Cloth _cloth;
	private float TurbulenceMin = 0f;
	private float TurbulenceMax = 20f;
	private Transform _boat;
	private Transform _player;

	// Use this for initialization
	void Start ()
	{
		_cloth = GetComponent<Cloth>();

		if(transform.childCount > 0)
		{
			_boat = transform.GetChild(0);
		}
		GenerateWaves();

        DayNightController.Instance.OnDawn += OnDawn;
	}

	void OnDestroy()
	{
		if(DayNightController.HasInstance)
		{
			DayNightController.Instance.OnDawn -= OnDawn;
		}
	}

	public void AccelerationCheck()
	{
		var largestAcceleration = Mathf.Max(
			_cloth.externalAcceleration.x + _cloth.externalAcceleration.y + _cloth.externalAcceleration.z,
			_cloth.randomAcceleration.x + _cloth.randomAcceleration.y + _cloth.randomAcceleration.z
		);
        
		if(largestAcceleration > 30 && Clock.Instance.Day > 1)
		{
			Capsize();
			DetachPlayer();
		}
	}

	public void Capsize()
	{
		if (_boat != null) {
			iTween.RotateTo (_boat.gameObject, iTween.Hash ("rotation", new Vector3 (transform.position.x, transform.position.y, 180f), "easetype", iTween.EaseType.easeInOutSine, "time", 1.3f));

		}
	}

	public void DetachPlayer()
	{
		if(_boat != null)
		{
			var player = _boat.GetChild(0);
			player.SetParent(this.transform);
			player.gameObject.AddComponent<FloatController>();
			player.transform.position = new Vector3(player.transform.position.x + 5, player.transform.position.y, player.transform.position.z + 5);

			if(player.GetComponent<BoxCollider>() != null)
				player.GetComponent<BoxCollider>().enabled = true;
		}
	}

	void Update()
	{
		if (_player == null && _boat != null && _boat.FindChild("Player") != null) {
			_player = _boat.FindChild ("Player");
		}

		if (_player.parent != _boat) {
			if (_player.GetComponent<BoxCollider> () == null) {
				_player.gameObject.AddComponent<BoxCollider> ();
			}
		}
	}

	public void OnDawn(int day)
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
        if (DayNightController.Instance.IsStorm) {
			_cloth.externalAcceleration = new Vector3(
				Random.Range(TurbulenceMin, TurbulenceMax), 
				Random.Range(TurbulenceMin, TurbulenceMax), 
				Random.Range(TurbulenceMin, TurbulenceMax));
            _cloth.worldAccelerationScale = Random.Range(0f, 1f);
        }
        else {
            CalmWaves();
        }
	}

	public void CalmWaves()
	{
		_cloth.worldAccelerationScale = 0.8f;
				_cloth.randomAcceleration = new Vector3(0f, 0f, 0f);
				_cloth.externalAcceleration = new Vector3(0f, 0f, 0f);
	}
}

/*
TODO

lightning flashes

spawning by day

using equipped items

need a glow behind card

camera:  zoom out during day, in at night

adding extra item spawns
    - only if you don't have it

durability
    - breaking items
    - once that's implemented, spawning duplicates is ok

crafting
    - combine line + parachute for sail
    - line + stick for pole

hook up animations

rain effect
*/

