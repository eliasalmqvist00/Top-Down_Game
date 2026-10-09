using System;
using Godot.Collections;
using Godot;

namespace Game.Gameplay;

public partial class Gatherable : StaticBody2D
{
	[ExportCategory("Pickup and Drops")]
	[Export] public PackedScene ItemDropScene;
	[Export] public Dictionary<ItemData, Vector2> ItemDrops {get; set;} = new();


	[ExportCategory("Gatherable vars")]
	[Export] public int BaseHealth;

	public int CurrentHealth;

	public override void _Ready()
	{
		CurrentHealth = BaseHealth;
	}

	public void SpawnLoot(Vector2 spawnPosition)
	{
		if (ItemDropScene == null || ItemDrops == null) return;
		
		foreach(ItemData drop in ItemDrops.Keys)
		{
			Core.Logger.Debug("Found drop");
			int dropCount = GD.RandRange((int)ItemDrops[drop].X, (int)ItemDrops[drop].Y);

			for(int i = 0; i < dropCount; i++)
			{
				ItemDrop itemDrop = ItemDropScene.Instantiate<ItemDrop>();
				GetParent().AddChild(itemDrop);

				float dist = (float) GD.RandRange(8, 16);
				float angle = (float) GD.RandRange(0, Mathf.Tau);
				Vector2 offset = new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);
				
				itemDrop.Instantiate(drop, spawnPosition, offset);
			}
		}
		
	}

}
