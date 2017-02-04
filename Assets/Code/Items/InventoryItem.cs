using System;
using UnityEngine;

public class InventoryItem : IInteractable
{       
    [SerializeField] public string Name;
    [SerializeField] private int itemDurability = 100;
    [SerializeField] private int durabilityReduction = 25;
    
    public InventoryItem(string itemName)
    {
        this.Name = itemName;
    }
    
    public void ReduceDurability()
	{
		itemDurability -= durabilityReduction;
		Debug.Log("New Durability : " + itemDurability);
		if(itemDurability <= 0)
		{
			BreakItem();
		}
	}

	public void BreakItem()
	{
		Debug.Log("Item has been destroyed");
	}

    public void OnTouchDown(Vector2 point)
    {
        ReduceDurability();
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

    public bool IsEquippable() {
        return (Name == "Sail" ||
                Name == "Oars" ||
                Name == "Pole" ||
                Name == "Knife" ||
                Name == "Bucket" ||
                Name == "Net"
        );
    }
}
