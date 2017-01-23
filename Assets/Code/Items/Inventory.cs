using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
	public static Dictionary<InventoryItem, int> Items;
	public static Dictionary<int, InventoryItem> ContainerItems;
	//public GameObject inventoryPanel;
	[SerializeField] private static int maxCapacity = 25;
	[SerializeField] private static int currentCapacity = 0;

    public static event Action<string> OnItemAdded;
	
	void Start()
	{
		Items = new Dictionary<InventoryItem, int>();
		ContainerItems = new Dictionary<int, InventoryItem> ();
	}

	public static void AddItem(InventoryItem item)
	{
		foreach(var dictItem in Items)
		{
			if(string.Equals(dictItem.Key.Name, item.Name))
			{
				Items[dictItem.Key] += 1;
				Debug.Log("Found existing " + dictItem.Key.Name + ". Adding for a total of " + Items[dictItem.Key]);
				return;
			}
		}

		if(Items.Count >= 12)
		{
			UI.Instance.SetSubtitle("If only I had more space...");
			return;
		}

		Items.Add(item, 1);
		Debug.Log("Adding new " + item.Name + ". Total of " + Items[item]);

        if (OnItemAdded != null)
            OnItemAdded(item.Name);
	}
	
	public static void RemoveItem(InventoryItem item)
	{
		//if(Items[item] <= 1)
		{
			switch(item.Name)
			{
                // These items don't get consumed
                case "Compass":
                case "Map":
                case "Oar":
                    return;

				case "Food" : PlayerController.Instance.EatFood(); break;
				case "Water" : PlayerController.Instance.DrinkWater(); break;

				default : Debug.Log("What to do with this...."); break;
			}
			Items.Remove(item);
			Debug.Log("Consumed the last " + item.Name);
			return;
		}
		Items[item] -= 1;
		Debug.Log("Consumed 1 " + item.Name + ". " + Items[item] + " remaining.");
	}

	public static void AddContainerItem(InventoryItem item, int index)
	{
		ContainerItems.Add(index, item);
		Debug.Log("Adding new " + item.Name + " to container inventory. WithKey " + index);
	}

	public static void RemoveContainerItem(int index)
	{
		Debug.Log ("Attempting to remove at index " + index + ". Container items has length of " + ContainerItems.Count);

		var item = ContainerItems [index];
		ContainerItems.Remove (index);
		AddItem (item);
	}

	// public void ToggleInterface()
	// {
	// 	Debug.Log("Toggle Interface : " + inventoryPanel.activeInHierarchy);
	// 	inventoryPanel.SetActive(inventoryPanel.activeSelf ? false : true);
	// }

    // public void OnTouchDown()
    // {
    //     ToggleInterface();
    // }

    // public void OnTouchUp()
    // {
    // }

    // public void OnTouchStay()
    // {
    // }

    // public void OnTouchExit()
    // {
    // }
}
