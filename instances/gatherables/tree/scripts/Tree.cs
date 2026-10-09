using System;
using Godot;

namespace Game.Gameplay;

public partial class Tree : StaticBody2D
{
	[ExportCategory("Tree Nodes")]
	[Export] public Sprite2D TreeSprite;
	[Export] public Area2D Hurtbox;
	[Export] public Marker2D CanopyPivot;
	[Export] public Sprite2D CanopySprite;
	[Export] public Area2D CrownArea;

	[ExportCategory("Pickup and Drops")]
	[Export] public PackedScene PickupScene;
	[Export] public ItemData ItemDrop;

	[ExportCategory("Tree vars")]
	[Export] private int _baseHealth;

	private int _currentHealth;

	private Tween _treeTween;

	public override void _Ready()
	{
		_currentHealth = _baseHealth;
		Hurtbox.AreaEntered += OnHurtboxEntered;

		CrownArea.AreaEntered += OnCrownAreaEntered;
		CrownArea.AreaExited += OnCrownAreaExited;
	}

	public void OnCrownAreaEntered(Area2D area)
    {
        CanopySprite.ZIndex = 1;
    }

	public void OnCrownAreaExited(Area2D area)
    {
        CanopySprite.ZIndex = 0;
    }

	public void OnHurtboxEntered(Area2D area)
	{
		if(area is PlayerHitbox hitbox && hitbox.Player.HeldItem.ItemName == "Axe")
		{
			DamageInfo dmgInfo = hitbox.GetDamageInfo(Position);

			Core.Logger.Info($"Chop, health = {_currentHealth}");
			_currentHealth -= dmgInfo.Amount;

			if(_currentHealth <= 0)
			{
				ChoppedDown(dmgInfo.HitDirection);
				return;
			}

			ShakeTree(dmgInfo.HitDirection);
		}
	}

	private void ChoppedDown(Vector2 hitDirection)
	{
		TreeSprite.Visible = false;
		CanopySprite.Visible = true;

		Hurtbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
		Hurtbox.SetDeferred(Area2D.PropertyName.Monitorable, false);

		CrownArea.AreaEntered -= OnCrownAreaEntered;
		CrownArea.AreaExited -= OnCrownAreaExited;
		Core.Logger.Info("Tree chopped down");

		float fallAngle = hitDirection.X >= 0 ? Mathf.DegToRad(90) : Mathf.DegToRad(-90);

		_treeTween?.Kill();
		_treeTween = CreateTween();
		
		_treeTween.TweenProperty(CanopyPivot, "rotation", fallAngle, 1.2f)
			 .SetTrans(Tween.TransitionType.Sine)
			 .SetEase(Tween.EaseType.In);

		_treeTween.Parallel().TweenProperty(CanopyPivot, "modulate:a", 0.0f, 0.3f)
			 .SetDelay(0.8f);

		_treeTween.Finished += () =>
		{
			SpawnLoot(hitDirection);
			CanopyPivot.QueueFree();
		};
	}

	private void ShakeTree(Vector2 hitDirection)
	{
		_treeTween?.Kill();
		_treeTween = CreateTween();
		
		float dir = hitDirection.X >= 0 ? Mathf.DegToRad(3) : Mathf.DegToRad(-3);

		_treeTween.TweenProperty(CanopyPivot, "rotation", dir, 0.2f)
			 .SetTrans(Tween.TransitionType.Sine)
			 .SetEase(Tween.EaseType.In);
		_treeTween.TweenProperty(CanopyPivot, "rotation", 0, 0.2f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.In);
	}

	public void SpawnLoot(Vector2 fallDirection)
	{
		if (PickupScene == null || ItemDrop == null) return;

		int dropCount = GD.RandRange(2,4);

		for(int i = 0; i < dropCount; i++)
		{
			ItemPickup drop = PickupScene.Instantiate<ItemPickup>();
			GetParent().AddChild(drop);

			float dist = (float) GD.RandRange(8, 16);
			float angle = (float) GD.RandRange(0, Mathf.Tau);
			Vector2 offset = new Vector2(Mathf.Cos(angle) * dist, Mathf.Sin(angle) * dist);

			int spawnDistOffset = 32;
			
			drop.Instantiate(ItemDrop, GlobalPosition + spawnDistOffset * fallDirection, offset);
		}
		
	}

}
