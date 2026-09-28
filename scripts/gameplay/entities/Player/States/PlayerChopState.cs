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
		Vector2 chopDirection = GetMouseDirection(Player.Position);
		Player.Direction = chopDirection;
		
		Core.Logger.Debug("About to chop");

		PlayerAnimation.PlayChop();
		
		Core.Logger.Debug("Chopped");

		await ToSignal(PlayerAnimation, AnimatedSprite2D.SignalName.AnimationFinished);

		Core.Logger.Debug("Finished animation wait");

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

	private Vector2 GetMouseDirection(Vector2 playerPosition)
	{
		Vector2 toMouse = Player.HitboxPivot.GetGlobalMousePosition() - playerPosition;
		float angle = Mathf.PosMod(Mathf.RadToDeg(toMouse.Angle()), 360f);

		if(angle > 45 && angle <= 135)
		{
			return Vector2.Down;
		}
		else if(angle > 135 && angle <= 225)
		{
			return Vector2.Left;
		}
		else if(angle > 225 && angle <= 315)
		{
			return Vector2.Up;
		}
		else
		{
			return Vector2.Right; 
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
