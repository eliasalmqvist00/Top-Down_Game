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
	[Export] public EnemyHitbox EnemyHitbox;
	

	public override void _Ready()
	{
		Enemy.MovementSpeed = 10;
	}

	public override void _Process(double delta)
	{
		Chase(delta);
		if(PlayerInRange())
		{
            Enemy.ActiveTween.Kill();
			Attack();
		}
	}

	public async void Chase(double delta)
	{

		Enemy.EnemyAnimation.PlayWalk();
		
		Enemy.ActiveTween = CreateTween();
		Enemy.ActiveTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
		Enemy.ActiveTween.TweenProperty(Enemy, "position", GetTargetPlayerPosition(), 0.5f);

		await ToSignal(Enemy.ActiveTween, Tween.SignalName.Finished);

        SnapPositionToGrid();
	}

	public bool PlayerInRange()
	{
		return EnemyHitbox.HitboxEntered;
	}

	public async void Attack()
	{
        SnapPositionToGrid();

        //Enemy.EnemyAnimation.IsAttacking = true;
		EmitSignal(SignalName.Animation, "attack");

        Enemy.ActiveTween = CreateTween();
		Enemy.ActiveTween.SetTrans(Tween.TransitionType.Linear).SetEase(Tween.EaseType.InOut);
		Enemy.ActiveTween.TweenProperty(Enemy, "position", GetTargetPlayerPosition(), 1.4f);

        await ToSignal(Enemy.ActiveTween, Tween.SignalName.Finished);

        SnapPositionToGrid();
	}

    private Vector2 GetTargetPlayerPosition()
    {
        Vector2 targetDir = (Enemy.Player.Position - Enemy.Position).Normalized();
		Enemy.Direction = targetDir;

		Vector2 targetPos = Enemy.Position + targetDir * Globals.Instance.GRID_SIZE;
		
		return targetPos;
    }
	private void SnapPositionToGrid()
	{
		Enemy.Position = new Vector2(
			Mathf.Round(Enemy.Position.X / Core.Globals.Instance.GRID_SIZE) * Core.Globals.Instance.GRID_SIZE,
			Mathf.Round(Enemy.Position.Y / Core.Globals.Instance.GRID_SIZE) * Core.Globals.Instance.GRID_SIZE
		);
	}

}
