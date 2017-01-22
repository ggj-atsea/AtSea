using System;
using UnityEngine;

public class InventoryItem : MonoBehaviour, IInteractable
{       
    [SerializeField] public string Name;
    [SerializeField] public int Width;
    [SerializeField] public int Height;
    [SerializeField] public int Weight;
    [SerializeField] private int itemDurability = 100;
    [SerializeField] private int durabilityReduction;
    
    public InventoryItem(string itemName, int temWidth, int itemHeight, int  itemWeight)
    {
        this.Name = itemName;
        this.Width = temWidth;
        this.Height = itemHeight;
        this.Weight = itemWeight;
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

    public void OnTouchDown()
    {
        ReduceDurability();
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
