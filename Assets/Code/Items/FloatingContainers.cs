using UnityEngine;
using System.Collections.Generic;

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
    [SerializeField] private Dictionary<int, InventoryItem> _insideContainer;

    private Vector3 _startingPos0 = new Vector3(5.0f, 0.0f, 4.0f);
    private Vector3 _startingPos1 = new Vector3(-4.0f, 0.0f, -5.0f);
    
    private const int MaxContainerItems = 6;
	private const float ContainerMaxX = 12.0f;
	private const float ContainerMinX = -12.0f;
	private const float ContainerMinZ = 10.0f;
	private const float ContainerMaxZ = 18.0f;
	private const float ContainerMinZNeg = -6.0f;
	private const float ContainerMaxZNeg = -12.0f;
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
		if(_player == null)
		{
			_player = transform.parent.GetComponentInChildren<PlayerController>();
		}
		
        _insideContainer = new Dictionary<int, InventoryItem>();
        GenerateContainerItems(-1);
        
		DayNightController.Instance.OnDusk += OnDusk;
		speed = Random.Range(MaxSpeed, MinSpeed);

        if (Random.Range(0,2) == 0) {
            _containerStartingLocation = new Vector3(Random.Range(ContainerMinX, ContainerMaxX), 0, Random.Range(ContainerMinZ, ContainerMaxZ));
            transform.position = _containerStartingLocation;
        }
        else {
            speed = -speed;
            _containerStartingLocation = new Vector3(Random.Range(ContainerMinX, ContainerMaxX), 0, Random.Range(ContainerMinZNeg, ContainerMaxZNeg));
            transform.position = _containerStartingLocation;
        }
	}

    public void Day0(int index)
    {
        if (index == 0) {
            transform.position = _startingPos0 + new Vector3(Random.Range(-3.0f,3.0f), 0,
                                                             Random.Range(-3.0f,3.0f));
            GenerateContainerItems(0);
        }
        else {
            transform.position = _startingPos1 + new Vector3(Random.Range(-3.0f,3.0f), 0,
                                                             Random.Range(-3.0f,3.0f));
            GenerateContainerItems(1);
        }

        speed = MinSpeed * 0.1f;
    }

	void OnDisable()
	{
		DayNightController.Instance.OnDusk -= OnDusk;
	}
    
    public void GenerateContainerItems(int fixedItem)
    {
        _insideContainer = new Dictionary<int, InventoryItem>();

        int minItems = 1;
        if (fixedItem == 0) {
            _insideContainer.Add(0, new InventoryItem("Oars"));
            minItems = 3;
        }
        else if (fixedItem == 1) {
            _insideContainer.Add(0, new InventoryItem("Compass"));
            minItems = 3;
        }

		var numberOfItemsInContainer = Random.Range(minItems, MaxContainerItems);

        // 1 in 8 crates will have 1 special item in it
        int specialItem = -1;
        if (Clock.Instance.Day > 0 && Random.Range(0,4) == 0)
            specialItem = Random.Range(0, numberOfItemsInContainer);

        for(int i = _insideContainer.Count; i < numberOfItemsInContainer; i++)
        {
            string item;

            if (specialItem == i) {
                int roll = Random.Range(0,4);
                switch (roll) {
                    default:
                    case 0:     item = "Bucket";    break;
                    case 1:     item = "Net";       break;
                    case 2:     item = "Sail";      break;
                    //case 3:     item = "Knife";     break;
                    case 3:     item = "Pole";      break;
                    //case 2:     item = "Oars";      break;
                }
            }
            else {
                item = (Random.Range(0,2) == 0) ? "Food" : "Water";
            }

            //var item = Random.Range(0, 2) == 0 ? "Food" : "Water";
			try{
				_insideContainer.Add(i, new InventoryItem(item));
			}
			catch(System.Exception ex){
				Debug.Log("Exception hit generating items and adding to dictionary. Item: " + item + " i: " + i + " " + ex.Message);
			}
            
        }

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
        // Our own velocity
        var vel = new Vector3(0,0,speed);

        // Minus the ship's motion
        vel -= BoatController.Instance.Velocity * 4;

        transform.Translate(vel * Time.deltaTime);

	}

	public void OnDusk(int day)
	{
		gameObject.SetActive(false);
	}

    public void OnTouchDown(Vector2 point)
    {
        Inventory.ClearContainer();

		Debug.Log ("You got me");
		foreach (var item in _insideContainer) {
			
			Inventory.AddContainerItem (item.Value, item.Key);
			Debug.Log ("Adding " + item.Value.Name + " to container. At index " + item.Key);
		}

		InventoryView.Instance.PopulatePackageContents();
		InventoryView.Instance.ShowPackageInventory();

        //gameObject.SetActive(false);
    }

    public void OnTouchExit(Vector2 point)
    {
    }

    public void OnTouchStay(Vector2 point)
    {
    }

    public void OnTouchUp(Vector2 point)
    {
    }
}
