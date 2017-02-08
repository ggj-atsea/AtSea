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
    
    private const int MaxContainerItems = 6;
	private const float ContainerMaxX = 12.0f;
	private const float ContainerMinX = -12.0f;
	private const float ContainerMinZ = 10.0f;
	private const float ContainerMaxZ = 18.0f;
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
		
        if(_insideContainer == null)
        {
            _insideContainer = new Dictionary<int, InventoryItem>();
        }
        _insideContainer = new Dictionary<int, InventoryItem>();
        GenerateContainerItems();
        
		DayNightController.Instance.OnDusk += OnDusk;
		speed = Random.Range(MaxSpeed, MinSpeed);
	}

	void OnDisable()
	{
		_containerStartingLocation = new Vector3(Random.Range(ContainerMinX, ContainerMaxX), 0, Random.Range(ContainerMinZ, ContainerMaxZ));
		transform.position = _containerStartingLocation;
		DayNightController.Instance.OnDusk -= OnDusk;
	}
    
    public void GenerateContainerItems()
    {
        _insideContainer = new Dictionary<int, InventoryItem>();

        int minItems = 1;
        if (Clock.Instance.Day == 1) {
            _insideContainer.Add(0, new InventoryItem("Oars"));
            minItems = 4;
        }

		var numberOfItemsInContainer = Random.Range(minItems, MaxContainerItems);

        // 1 in 8 crates will have 1 special item in it
        int specialItem = -1;
        if (Clock.Instance.Day > 1 && Random.Range(0,8) == 0)
            specialItem = Random.Range(0, numberOfItemsInContainer);

        for(int i = _insideContainer.Count; i < numberOfItemsInContainer; i++)
        {
            string item;

            if (specialItem == i) {
                int roll = Random.Range(0,5);
                switch (roll) {
                    default:
                    case 0:     item = "Bucket";    break;
                    case 1:     item = "Net";       break;
                    case 2:     item = "Sail";      break;
                    case 3:     item = "Knife";     break;
                    case 4:     item = "Pole";      break;
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
		transform.Translate(new Vector3(0, 0, speed) * Time.deltaTime);
	}

	public void OnDusk(int day)
	{
		gameObject.SetActive(false);
	}

    public void OnTouchDown(Vector2 point)
    {
		Debug.Log ("You got me");
		foreach (var item in _insideContainer) {
			
			Inventory.AddContainerItem (item.Value, item.Key);
			Debug.Log ("Adding " + item.Value.Name + " to container. At index " + item.Key);
		}

		InventoryView.Instance.PopulatePackageContents();
		InventoryView.Instance.ShowPackageInventory();

        gameObject.SetActive(false);
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
