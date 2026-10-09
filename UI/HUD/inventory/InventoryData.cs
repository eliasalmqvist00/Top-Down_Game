using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace Game.Gameplay;

[GlobalClass]
public partial class InventoryData : Resource
{
    public event Action<InventorySlotData, int> SlotUpdated; // Signaling the slotdata and index of slot

    [ExportCategory("Slots Array")]
    [Export] public Array<InventorySlotData> Slots { get; set; } = new();
    
    private const int NumberOfSlots = 30;
    private bool IsFull = false;

    public static InventoryData LoadOrCreate(string path, int slotCount = 30)
    {
        if (FileAccess.FileExists(path))
        {
            var loadedInventory = ResourceLoader.Load<InventoryData>(path);
            if (loadedInventory != null)
            {
                bool wasEmpty = loadedInventory.Slots.Count < slotCount;

                loadedInventory.EnsureSlots(slotCount);

                if (wasEmpty)
                {
                    loadedInventory.Save(path);
                    GD.Print($"[Inventory] Populated and saved {slotCount} slots into previously empty file at {path}");
                }
            }
            return loadedInventory;
        }

        var newInventory = new InventoryData();
        newInventory.EnsureSlots(slotCount);

        newInventory.Save(path);

        return newInventory;
    }

    public void EnsureSlots(int count)
    {
        if(Slots.Count == 0)
        {
            Core.Logger.Debug("No slots, esnuring slots...");
            for(int i = 0; i < NumberOfSlots; i++)
            {
                Slots.Add(new InventorySlotData());
            }
        }
    }

    public void Save(string path)
    {
        Error err = ResourceSaver.Save(this, path);
        if (err != Error.Ok)
        {
            GD.PrintErr($"[Inventory] Failed to save inventory to {path}: {err}");
        }
        else
        {
            GD.Print($"[Inventory] Successfully saved inventory to {path}");
        }
    }

    public void AddItem(ItemData item)
    {
        TryAddItem(item, 1);
    }

    public void TryAddItem(ItemData item, int quantity)
    {
        var itemResult = FindSlotWithItem(item);

        if(itemResult.hasItem && item.IsStackable)
        {
            int newQuantity = Slots[itemResult.idx].ItemQuantity + quantity;

            Slots[itemResult.idx].ItemQuantity += quantity;
            SlotUpdated(Slots[itemResult.idx], itemResult.idx);
        }
        else if(itemResult.hasItem && !item.IsStackable)
        {
            Slots[FindFirstEmptySlot()].Item = item;
        }
        else if(!itemResult.hasItem && !IsFull)
        {
            int emptyIdx = FindFirstEmptySlot();
            Slots[emptyIdx].Item = item;
            Slots[emptyIdx].ItemQuantity = quantity;
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
            if(Slots[i].Item == item && Slots[i].ItemQuantity < item.StackQuantity)
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