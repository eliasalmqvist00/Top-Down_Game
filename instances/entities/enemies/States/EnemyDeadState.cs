using System.Threading.Tasks;
using Game.Utilities;
using Godot;

namespace Game.Gameplay;

public partial class EnemyDeadState : State
{

	[ExportCategory("Nodes")]
	[Export] public Enemy Enemy;

	public override void EnterState()
	{
		base.EnterState();

		Enemy.SetPhysicsProcess(false);
		Enemy.SetProcess(false);
		Enemy.StateMachine.SetPhysicsProcess(false);
		Enemy.StateMachine.SetProcess(false);

		Death();
	}

	public async void Death()
	{   
		Enemy.EnemyHealthbar.Visible = false;

		Enemy.EnemyAnimation.PlayDeath();
		await ToSignal(Enemy.EnemyAnimation, AnimatedSprite2D.SignalName.AnimationFinished);

		if (!IsInstanceValid(Enemy)) return;

		Enemy.EnemyAnimation.Stop(); 
		if (Enemy.EnemyAnimation is AnimatedSprite2D sprite)
		{
			sprite.Frame = sprite.SpriteFrames.GetFrameCount("death") - 1;
		}

		Enemy.CollisionLayer = 2;
		Enemy.EnemyHurtbox.QueueFree();

		await ToSignal(GetTree().CreateTimer(5.0f), SceneTreeTimer.SignalName.Timeout);
		if (IsInstanceValid(Enemy))
		{
			Enemy.QueueFree();
		}
	}

}
