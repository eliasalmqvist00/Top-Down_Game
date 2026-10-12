using Game.Gameplay;
using Game.Utilities;
using Godot;
using System;

public partial class SlingshotState : State
{
	[ExportCategory("SlingshotState Nodes")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;

	[ExportCategory("Projectile Scene")]
	[Export] public PackedScene ProjectileScene;

	private Vector2 _fireDirection;


	public override void EnterState()
	{
		base.EnterState();
		PlayerAnimation.FrameChanged += OnFrameChanged;

		Slingshot();
	}

	private async void Slingshot()
	{
		_fireDirection = Modules.GetMouseDirection(Player, Player.Position);
		Player.Direction = Modules.GetQuantizedMouseDirection(Player, Player.Position);

		PlayerAnimation.PlaySlingshot();
		await ToSignal(PlayerAnimation, AnimatedSprite2D.SignalName.AnimationFinished);

		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
	}

	private void OnFrameChanged()
	{
		if(PlayerAnimation.Frame == 2)
		{
			FireProjectile();
		}
	}

	private void FireProjectile()
	{
		int acorns = GameEvents.EmitFindItemAndQuantity("Acorn");

		if(acorns > 0)
		{
			GameEvents.EmitItemUSed("Acorn", 1);
			
			SlingshotProjectile projectile = ProjectileScene.Instantiate<SlingshotProjectile>();

			Node2D container = GetNode<Node2D>("/root/MainGame/Control/SubViewportContainer/SubViewport/World");
			container.CallDeferred(Node.MethodName.AddChild, projectile);

			int projectileSpawnOffset = 16;
			projectile.Instialize(Player.CollisionShape.GlobalPosition + Player.Direction * projectileSpawnOffset, _fireDirection);
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		PlayerAnimation.FrameChanged -= OnFrameChanged;
	}


}
