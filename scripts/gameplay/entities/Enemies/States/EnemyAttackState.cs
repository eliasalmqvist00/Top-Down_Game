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

	public override void EnterState()
	{
		base.EnterState();
		EnemyMovement.SnapPositionToGrid();
		EnemyAnimation.FrameChanged += OnFrameChanged;
		EnemyAnimation.AnimationFinished += OnAttackFinished;
		Attack();
	}

	public void Attack()
	{
		EnemyAnimation.PlayAttack();
	}

	public void OnAttackFinished()
	{
		StateMachine.ChangeState(StateMachine.GetNode<State>("Chase"));
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
		EnemyHitbox.HitboxShape.Disabled = true;
		EnemyAnimation.FrameChanged -= OnFrameChanged;
		EnemyAnimation.AnimationFinished -= OnAttackFinished;
	}

}
