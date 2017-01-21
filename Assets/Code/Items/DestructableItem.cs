using UnityEngine;

public abstract class DestructableItem : MonoBehaviour
{
	[SerializeField] private int itemDurability = 100;
	[SerializeField] private int durabilityReduction = 5;
	
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
}
