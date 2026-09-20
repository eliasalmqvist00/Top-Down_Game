using Godot;
using Game.Utilities;

namespace Game.Gameplay;
public partial class PlayerAttackState : State
{
	[ExportCategory("State Vars")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;
	[Export] public StateMachine StateMachine;
	[Export] public CollisionShape2D PlayerHitbox;

	public override void EnterState()
	{
		base.EnterState();
        Attack();
	}

    private async void Attack()
    {
        Vector2 attackDirection = GetMouseDirection(Player.Position);
		Player.Direction = attackDirection;

	    PlayerHitbox.Disabled = false;
		PlayerAnimation.PlayAttack();

        await ToSignal(PlayerAnimation, EntityAnimation.SignalName.AnimationFinished);

        PlayerHitbox.Disabled = true;
        OnAttackFinished();
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

	public void OnAttackFinished()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}
