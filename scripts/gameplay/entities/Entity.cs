using Godot;

namespace Game.Gameplay;

public partial class Entity : CharacterBody2D
{   
    [ExportCategory("Entity Nodes")]
    [Export] public CollisionShape2D CollisionShape;
    [Export] public EntityMovement EntityMovement;
    
    
    [ExportCategory("Entity Vars")]
    [Export] public int MaxHealth;
    [Export] public int MovementSpeed;

    [Export] public Vector2 Direction;


    public int CurrentHealth;
    public bool IsAlive {get; set;} = true;

    public override void _Ready()
    {
        CurrentHealth = MaxHealth;
        MovementSpeed = 1;
    }

    public virtual void OnDeath()
    {
        QueueFree();
    }

    public int GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void SetHealth(int newHealth)
    {
        CurrentHealth = newHealth;
    }

    public Vector2 GetDirection()
    {
        return Direction;
    }

    public void SetDirection(Vector2 newDirection)
    {
        Direction = newDirection;
    }

}