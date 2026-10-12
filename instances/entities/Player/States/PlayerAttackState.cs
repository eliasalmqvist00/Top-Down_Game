using Godot;
using Game.Utilities;

namespace Game.Gameplay;
public partial class PlayerAttackState : State
{
	[ExportCategory("State Vars")]
	[Export] public Player Player;
	[Export] public PlayerAnimation PlayerAnimation;
	[Export] public CollisionShape2D PlayerHitbox;

	public override void EnterState()
	{
		base.EnterState();
		PlayerAnimation.FrameChanged += OnFrameChange;
        Attack();
	}

    private async void Attack()
    {
        Vector2 attackDirection = Modules.GetQuantizedMouseDirection(Player, Player.Position);
		Player.Direction = attackDirection;

	    
		PlayerAnimation.PlayAttack();

        await ToSignal(PlayerAnimation, PlayerAnimation.SignalName.AnimationFinished);

        PlayerHitbox.Disabled = true;
        OnAttackFinished();
    }

	public void OnFrameChange()
    {
        if(PlayerAnimation.Animation.ToString().StartsWith("attack"))
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

	public void OnAttackFinished()
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
