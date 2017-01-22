using UnityEngine;

public enum ContainerSupply
{
	Food,
	Water,
	Item
}

public class FloatingContainers : MonoBehaviour, IInteractable
{
	private float speed;

	[SerializeField] private ContainerSupply _containerContents;
	[SerializeField] private PlayerController _player;
	[SerializeField] private Vector3 _containerStartingLocation;

	private const float ContainerMaxX = 15.0f;
	private const float ContainerMinX = -15.0f;
	private const float ContainerMinZ = 16.0f;
	private const float ContainerMaxZ = 25.0f;
	private const float ContainerResetPoint = -5.0f;
	private const float MinSpeed = -0.5f;
	private const float MaxSpeed = -3f;

	void Update()
	{
		MoveContainer();
		DisableOffScreen();
	}

	void OnEnable()
	{
		DayNightController.Instance.OnDusk += OnMidnight;
		speed = Random.Range(MaxSpeed, MinSpeed);
		_containerStartingLocation = new Vector3(Random.Range(ContainerMinX, ContainerMaxX), 0, Random.Range(ContainerMinZ, ContainerMaxZ));
		transform.position = _containerStartingLocation;
	}

	void OnDisable()
	{
		DayNightController.Instance.OnDusk -= OnMidnight;
	}

	public void DisableOffScreen()
	{
		if(transform.localPosition.z < ContainerResetPoint)
		{
			gameObject.SetActive(false);
		}
	}

	public void MoveContainer()
	{
		transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
	}

	public void OnMidnight(int day)
	{
		gameObject.SetActive(false);
	}

    public void OnTouchDown()
    {
		switch(_containerContents)
		{
			case ContainerSupply.Food:
				_player.EatFood();
				break;
			case ContainerSupply.Water:
				_player.DrinkWater();
				break;
			case ContainerSupply.Item:
				break;
			default: Debug.Log("What is this container doing here...?");
				break;
		}
        gameObject.SetActive(false);
    }

    public void OnTouchExit()
    {
    }

    public void OnTouchStay()
    {
    }

    public void OnTouchUp()
    {
    }
}
