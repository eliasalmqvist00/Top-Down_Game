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

	public override void EnterState()
	{
		base.EnterState();

		//EnemyMovement.SnapPositionToGrid();

		EnemyAnimation.FrameChanged += OnFrameChanged;
		EnemyAnimation.AnimationFinished += OnAttackFinished;
		AttackRange.AreaExited += NotInRange;
		Attack();
	}

	public void Attack()
	{
		EnemyAnimation.PlayAttack();
	}

	private void NotInRange(Area2D areaa)
	{
		if(areaa is PlayerHurtbox)
		{
			StateMachine.ChangeState(StateMachine.GetNode<State>("Chase"));
		}
	}

	public void OnAttackFinished()
	{
		//StateMachine.ChangeState(StateMachine.GetNode<State>("Chase"));
	}

	private void OnFrameChanged()
	{
		if(EnemyAnimation.Animation.ToString().StartsWith("attack"))
		{
			if(EnemyAnimation.Frame == 1)
			{
				EnemyHitbox.HitboxShape.Disabled = false;
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
		EnemyHitbox.HitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		EnemyAnimation.FrameChanged -= OnFrameChanged;
		EnemyAnimation.AnimationFinished -= OnAttackFinished;
		AttackRange.AreaExited -= NotInRange;
	}

}
