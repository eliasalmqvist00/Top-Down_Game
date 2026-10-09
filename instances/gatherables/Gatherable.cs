using Godot;

namespace Game.Gameplay;

public partial class Gatherable : StaticBody2D
{
    [ExportCategory("Pickup and Drops")]
	[Export] public PackedScene ItemDropScene;
	[Export] public ItemData ItemDrop;
    [Export] public int MinDrop;
    [Export] public int MaxDrop;


	[ExportCategory("Gatherable vars")]
	[Export] public int BaseHealth;
    
    public int CurrentHealth;

    public override void _Ready()
    {
        CurrentHealth = BaseHealth;
    }

    public void SpawnLoot(Vector2 spawnPosition)
	{
		if (ItemDropScene == null || ItemDrop == null) return;

		int dropCount = GD.RandRange(MinDrop, MaxDrop);

		for(int i = 0; i < dropCount; i++)
		{
			ItemDrop drop = ItemDropScene.Instantiate<ItemDrop>();
			GetParent().AddChild(drop);

			float dist = (float) GD.RandRange(8, 16);
			float angle = (float) GD.RandRange(0, Mathf.Tau);
			Vector2 offset = new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);
			
			drop.Instantiate(ItemDrop, spawnPosition, offset);
		}
		
	}

}