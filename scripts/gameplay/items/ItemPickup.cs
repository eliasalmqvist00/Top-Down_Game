using Game.Gameplay;
using Godot;
using System;

namespace Game.Gameplay;

public partial class ItemPickup : Area2D
{
	[ExportCategory("Nodes")]
	[Export] private ItemData _item;
	[Export] private Sprite2D _icon;
	[Export] private CollisionShape2D _collisionShape;

	private bool _canBePickedUp = false;
	
	public override void _Ready()
	{
		_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		SetDeferred(Area2D.PropertyName.Monitoring, true);
		SetDeferred(Area2D.PropertyName.Monitorable, true);

		AreaEntered += ItemInRange;
	}

	public void Instantiate(ItemData item, Vector2 spawnPosition, Vector2 targetOffset)
	{
		_item = item;
		_icon.Texture = item.ItemIcon;
		GlobalPosition = spawnPosition;

		PlaySpawnBounce(targetOffset);
	}

	public void ItemInRange(Area2D area)
	{
		if(area is PlayerHurtbox hurtbox)
		{
			Tween pickupTween = CreateTween();
			pickupTween.TweenProperty(this, "position", hurtbox.Position, 1.2f)
			 .SetTrans(Tween.TransitionType.Sine)
			 .SetEase(Tween.EaseType.In);

			GameEvents.EmitItemPickedUp(_item);
			QueueFree();
		}
	}

	private void PlaySpawnBounce(Vector2 targetOffset)
	{
		Vector2 targetPosition = GlobalPosition + targetOffset;

		Tween tween = CreateTween().SetParallel(true);

		tween.TweenProperty(this, "global_position", targetPosition, 0.4f)
			 .SetTrans(Tween.TransitionType.Quad)
			 .SetEase(Tween.EaseType.Out);

		Tween arcTween = CreateTween();
		arcTween.TweenProperty(_icon, "position:y", -14f, 0.2f)
				.SetTrans(Tween.TransitionType.Quad)
				.SetEase(Tween.EaseType.Out);
		arcTween.TweenProperty(_icon, "position:y", 0f, 0.2f)
				.SetTrans(Tween.TransitionType.Bounce)
				.SetEase(Tween.EaseType.Out);

		tween.Chain().Finished += () =>
		{
			_canBePickedUp = true;
			_collisionShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			SetDeferred(Area2D.PropertyName.Monitoring, true);
			SetDeferred(Area2D.PropertyName.Monitorable, true);
		};
	}
}
