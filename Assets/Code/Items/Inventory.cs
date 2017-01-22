using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IInteractable
{
	public List<InventoryItem> inventory;
	public GameObject inventoryPanel;
	[SerializeField] private int maxCapacity = 25;
	[SerializeField] private int currentCapacity = 0;
	
	public void AddItem(InventoryItem item)
	{
		if(item.Weight + currentCapacity > maxCapacity)
		{
			Debug.Log("If only I had more space...");
			return;
		}

		inventory.Add(item);
	}
	
	public void RemoveItem(InventoryItem item)
	{
		inventory.Remove(item);
	}

	public void ToggleInterface()
	{
		Debug.Log("Toggle Interface : " + inventoryPanel.activeInHierarchy);
		inventoryPanel.SetActive(inventoryPanel.activeSelf ? false : true);
	}

    public void OnTouchDown()
    {
        ToggleInterface();
    }

    public void OnTouchUp()
    {
    }

    public void OnTouchStay()
    {
    }

    public void OnTouchExit()
    {
    }
}
