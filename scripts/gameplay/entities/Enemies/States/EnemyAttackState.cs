using System.Threading.Tasks;
using Game.Core;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyAttackState : State
{
	[Signal] public delegate void AnimationEventHandler(string animationType);

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public EnemyAnimation EnemyAnimation;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public EnemyHitbox EnemyHitbox;
	[Export] public Area2D AttackRange;

	private bool _inRange = false;

	public override void EnterState()
	{
		base.EnterState();

		EnemyMovement.SnapPositionToGrid();

		EnemyAnimation.FrameChanged += OnFrameChanged;
		AttackRange.AreaExited += NotInRange;
		
		_inRange = true;

		Attack();
	}

	public async void Attack()
	{
		while(_inRange)
        {
			EnemyAnimation.PlayAttack();
			await ToSignal(GetTree().CreateTimer(1.2f), SceneTreeTimer.SignalName.Timeout); 
        }
	}

	private void NotInRange(Area2D areaa)
	{
		if(areaa is PlayerHurtbox)
		{
			_inRange = false;
			StateMachine.ChangeState(StateMachine.GetNode<State>("Chase"));
		}
	}

	private void OnFrameChanged()
	{
		if(EnemyAnimation.Animation.ToString().StartsWith("attack"))
		{
			if(EnemyAnimation.Frame == 1)
			{
				EnemyHitbox.HitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
			}
			else
			{
				EnemyHitbox.HitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			}
		}
	}

	public override void ExitState()
	{
		base.ExitState();
		_inRange = false;

		EnemyHitbox.HitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);

		EnemyAnimation.FrameChanged -= OnFrameChanged;
		AttackRange.AreaExited -= NotInRange;
	}

}
