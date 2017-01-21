using System;

public class InventoryItem : DestructableItem, IInteractable
{
	public int itemSize;
	
    public InventoryItem(int itemSize)
    {
        this.itemSize = itemSize;
    }

    public void OnTouchDown()
    {
        ReduceDurability();
    }

    public void OnTouchExit()
    {
        throw new NotImplementedException();
    }

    public void OnTouchStay()
    {
        throw new NotImplementedException();
    }

    public void OnTouchUp()
    {
        throw new NotImplementedException();
    }
}
