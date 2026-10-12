using Godot;
using Game.Utilities;

namespace Game.Gameplay;
public partial class PlayerChopState : State
{
	[ExportCategory("State Vars")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;
	[Export] public CollisionShape2D PlayerHitbox;

	public override void EnterState()
	{
		base.EnterState();
		PlayerAnimation.FrameChanged += OnFrameChange;
		Chop();
	}

	private async void Chop()
	{
		Vector2 chopDirection = Modules.GetQuantizedMouseDirection(Player, Player.Position);
		Player.Direction = chopDirection;

		PlayerAnimation.PlayChop();

		await ToSignal(PlayerAnimation, AnimatedSprite2D.SignalName.AnimationFinished);

		PlayerHitbox.Disabled = true;
		OnChopFinished();
	}

	public void OnFrameChange()
	{
		if(PlayerAnimation.Animation.ToString().StartsWith("axe"))
		{
			if(PlayerAnimation.Frame == 1)
			{
				PlayerHitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			}
			else
			{
				PlayerHitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			}
		}
	}

	public void OnChopFinished()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
	}

	public override void ExitState()
	{
		base.ExitState();
		PlayerAnimation.FrameChanged -= OnFrameChange;
		PlayerHitbox.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
}
