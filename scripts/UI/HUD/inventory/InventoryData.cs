using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Game.Gameplay;

[GlobalClass]
public partial class InventoryData : Resource
{
    public static event Action<int, ItemData> SlotUpdated;

    [ExportCategory("Slots Array")]
    [Export] public Array<InventortSlotData> Slots { get; set; } = new();
    
    private const int NumberOfSlots = 20;
    private bool IsFull = false;

    public InventoryData()
    {
        if(Slots.Count == 0)
        {
            for(int i = 0; i < NumberOfSlots; i++)
            {
                Slots.Add(new InventortSlotData());
            }
        }
    }

    public void TryAddItem(ItemData item, int quantity)
    {
        var itemResult = FindSlotWithItem(item);

        if(itemResult.hasItem && item.IsStackable)
        {
            int newQuantity = Slots[itemResult.idx].ItemQuantity += quantity;
            SlotUpdated(itemResult.idx, item);
        }
        else if(itemResult.hasItem && !item.IsStackable)
        {
            Slots[FindFirstEmptySlot()].Item = item;
        }
        else if(!itemResult.hasItem && !IsFull)
        {
            Slots[FindFirstEmptySlot()].Item = item;
            Slots[FindFirstEmptySlot()].ItemQuantity = quantity;
        }
    }

    public int FindFirstEmptySlot()
    {
        for(int i = 0; i < Slots.Count; i++)
        {
            if(Slots[i].Item == null) return i;
        }
        return Slots.Count;
    }

	public (bool hasItem, int idx) FindSlotWithItem(ItemData item)
	{
		if(item == null) return (false, -1);
		
        for(int i = 0; i < Slots.Count; i++)
        {
            if(Slots[i].Item == item)
            {
                return (true, i);
            }
        }
        return (false, -1);
	}

	private bool HasItem(ItemData item)
	{
        var result = FindSlotWithItem(item);
		return result.hasItem;
	}

    
}