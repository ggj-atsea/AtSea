using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : Singleton<Inventory>
{
	public static Dictionary<InventoryItem, int> Items;
	public static Dictionary<int, InventoryItem> ContainerItems;

    public static event Action<string> OnItemAdded;
	public static event Action<string> OnItemRemoved;
	
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
        if (item.IsEquippable()) {
            return;
        }

		switch(item.Name)
		{
			case "Food" : PlayerController.Instance.EatFood(); break;
			case "Water" : PlayerController.Instance.DrinkWater(); break;

			default : Debug.Log("What to do with this...."); break;
		}

		try
		{
			if(Items[item] <= 1)
			{
				Items.Remove(item);	
			}
			else{
				Items[item] -= 1;
			}
		}
		catch (System.Exception ex)
		{
			Debug.Log("Exception hit removing item in Inventory.cs. " + ex);
			throw;
		}
		
		Debug.Log("Consumed 1 " + item.Name);

		if (OnItemRemoved != null)
			OnItemRemoved(item.Name);
	}

    public static void ClearContainer()
    {
        ContainerItems.Clear();
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

    public List<InventoryItem> GetEquippedItems() {
        var result = new List<InventoryItem>();

        foreach (var kvp in Items) {
            var item = kvp.Key;

            if (item.IsEquippable()) {
                result.Add(item);
            }
        }

        return result;
    }
}
