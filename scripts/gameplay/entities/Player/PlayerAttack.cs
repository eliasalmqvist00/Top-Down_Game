using Godot;

namespace Game.Gameplay;
	
public partial class PlayerAttack : Marker2D
{
	[Signal] public delegate void AnimationEventHandler(string animationType);

	[ExportCategory("Nodes")]
	[Export] public Player Player;
	[Export] public Marker2D HitboxPivot;
	[Export] public CollisionShape2D PlayerHitbox;
	[Export] public PlayerAnimation PlayerAnimation;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		Attack();
	}

	public async void Attack()
	{
		if(Input.IsActionJustPressed("ui_attack"))
		{
			//IsAttacking = true;
			PlayerHitbox.Disabled = false;
			PlayerAnimation.IsAttacking = true;

			Vector2 attackDirection = GetMouseDirection(Player.Position);
			Player.Direction = attackDirection;
			HitboxPivot.Rotation = attackDirection.Angle() + Vector2.Up.Angle();
			
			EmitSignal(SignalName.Animation, "attack");

			await ToSignal(PlayerAnimation, AnimationMixer.SignalName.AnimationFinished);

			PlayerHitbox.Disabled = true;
			PlayerAnimation.IsAttacking = false;

			//PlayerInput.HoldTime = 0.0;
		}
	}

	private Vector2 GetMouseDirection(Vector2 playerPosition)
	{
		Vector2 toMouse = GetGlobalMousePosition() - playerPosition;
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


}
