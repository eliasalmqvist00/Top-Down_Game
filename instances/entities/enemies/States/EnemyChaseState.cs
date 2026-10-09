using System;
using System.Threading.Tasks;
using Game.Core;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyChaseState : State
{

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;
	[Export] public EnemyMovement EnemyMovement;
	[Export] public Area2D AttackRange;
	
	private Tween _chaseTween;


	public override void EnterState()
	{
		base.EnterState();
		AttackRange.AreaEntered += PlayerInRange;
		Enemy.MovementSpeed = 3;
	}

	public void PlayerInRange(Area2D area)
	{
		if(area is PlayerHurtbox)
		{
			StateMachine.ChangeState(StateMachine.GetNode<State>("Attack"));
		}
	}

	public override void _Process(double delta)
	{
		BeginChase();
	}

	private async void BeginChase()
	{
		await Chase();
	}

	public async Task Chase()
	{
		Enemy.EnemyAnimation.PlayWalk();
		
		_chaseTween = CreateTween();
		_chaseTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
		_chaseTween.TweenProperty(Enemy, "position", GetTargetPlayerPosition(), 1f / Enemy.MovementSpeed);

		await ToSignal(_chaseTween, Tween.SignalName.Finished);

		EnemyMovement.SnapPositionToGrid();
	}

	private Vector2 GetTargetPlayerPosition()
	{
		Vector2 targetDir = (Enemy.Player.Position - Enemy.Position).Normalized();
		Enemy.Direction = targetDir;

		Vector2 targetPos = Enemy.Position + targetDir * Globals.Instance.GRID_SIZE;

		return targetPos;
	}

	public override void ExitState()
	{
		base.ExitState();
		_chaseTween.Kill();
		AttackRange.AreaEntered -= PlayerInRange;
	}

}
