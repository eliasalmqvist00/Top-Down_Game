using Godot;

namespace Game.Gameplay;

public partial class Tree : StaticBody2D
{
    [ExportCategory("Tree Nodes")]
    [Export] public Area2D Hurtbox;
    [Export] public Marker2D CanopyPivot;

    [ExportCategory("Tree vars")]
    [Export] private int _baseHealth;

    private int _currentHealth;

    public override void _Ready()
    {
        _currentHealth = _baseHealth;
        Hurtbox.AreaEntered += OnHurtboxEntered;
    }

    public void OnHurtboxEntered(Area2D area)
    {
        if(area is PlayerHitbox hitbox)
        {
            _currentHealth -= hitbox.GetDamageInfo(Position).Amount;

            if(_currentHealth <= 0)
            {
                ChoppedDown();
            }
        }
    }

    private void ChoppedDown()
    {
        Hurtbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
        Hurtbox.SetDeferred(Area2D.PropertyName.Monitorable, false);

        // TODO:
        // Rotate canopy pivot
        // Leave trunk
        // Canopy dissapears, leaving logs to pick up
    }

}