using Game.Gameplay;
using Godot;
using System;

public partial class Rock : Gatherable, IGatherable
{
	[ExportCategory("Rock Nodes")]
	[Export] public Sprite2D RockSprite;
	[Export] public Area2D Hurtbox;

	public RequiredTool RequiredTool => throw new NotImplementedException();

	private Tween _rockTween;

	public override void _Ready()
	{
		base._Ready();
		RockSprite.RegionEnabled = true;
		Hurtbox.AreaEntered += DamageTaken;
	}

	public void DamageTaken(Area2D area)
	{
		if(area is PlayerHitbox hitbox)
		{
			Game.Core.Logger.Debug("Rock damage taken");

			DamageInfo dmgInfo = hitbox.GetDamageInfo(Position);
			CurrentHealth -= hitbox.GetDamageInfo(Position).Amount;

			Rect2 RockRegion = RockSprite.RegionRect;
			RockRegion.Position = new Vector2(RockRegion.Position.X + 32, RockRegion.Position.Y);
			RockSprite.RegionRect = RockRegion;
		}

		if(CurrentHealth <= 0)
        {
            Callable.From(() => SpawnLoot(GlobalPosition)).CallDeferred();
			ClearObject();
        }
	}

	public void ClearObject()
    {
        Hurtbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
		Hurtbox.SetDeferred(Area2D.PropertyName.Monitorable, false);

		Hurtbox.AreaEntered -= DamageTaken;

		QueueFree();
    }

}
