using Game.HUD;
using Game.Utilities;
using Godot;
using System;

namespace Game.Gameplay;

public partial class Player : Entity
{

	[ExportGroup("Player Nodes")]
	[Export] public StateMachine StateMachine;
	[Export] public Marker2D HitboxPivot;
	[Export] public PlayerHurtbox PlayerHurtbox;
	[Export] public PlayerAnimation PlayerAnimation;

	[ExportGroup("Player Vars")]
	[Export] public int PlayerBaseMovementSpeed = 5;
	[Export] public double AttackCoolDown = 0.0;
	[Export] public int AttackDamage = 5;
	[Export] public int KnockbackForce = 4;

	[ExportCategory("Inventory")]
	[Export] private InventoryUI _inventoryUI;
	[Export] private Toolbar _toolbar;
	[Export] InventoryData InventoryData;
	public ItemData HeldItem;

	// --- Tweens ---
	private Tween _dmgFlashTween;
	public bool IsAttacking = false;

	
	public override void _Ready()
	{
		MaxHealth = 30;
		base._Ready();
		MovementSpeed = PlayerBaseMovementSpeed;

		PlayerHurtbox.ReceivedDamage += OnDamageReceived;

		InitializeInventory();

		StateMachine.ChangeState(StateMachine.GetNode<State>("Roam"));
	}

	public void InitializeInventory()
	{
		GameEvents.OnItemPickedUp += InventoryData.AddItem;
		GameEvents.OnActiveItemChanged += OnItemEquipped;

		InventoryData = InventoryData.LoadOrCreate(GameSavemanager.InventorySavePath);

		_inventoryUI.BindInventory(InventoryData);
		_toolbar.BindInventory(InventoryData);

		_inventoryUI.InitializeInventoryUI();
		_toolbar.InitializeToolbar();

		HeldItem = _toolbar.GetCurrentItem();
	}

	public override void _Process(double delta)
	{
		HitboxPivot.Rotation = Direction.Angle() + Vector2.Up.Angle();
	}

	public void OnItemEquipped(ItemData item)
	{
		HeldItem = item;
		GD.Print($"Player equipped: {HeldItem?.ItemName ?? "Hands empty"}");
	}

	private void OnDamageReceived(DamageInfo dmgInfo)
	{
		if(!IsAlive) return;

		CurrentHealth -= dmgInfo.Amount;
		Core.Logger.Info($"Player health = {CurrentHealth}");

		DamageFlash();
		//Position += dmgInfo.KnockbackForce;

		if(CurrentHealth <= 0)
		{
			OnDeath();
		}
	}

	public override void OnDeath()
	{
		if(!IsAlive) return;
		IsAlive = false;

		_dmgFlashTween?.Kill();

		PlayerHurtbox.SetDeferred(Area2D.PropertyName.Monitoring, false);
		PlayerHurtbox.SetDeferred(Area2D.PropertyName.Monitorable, false);

		PlayerAnimation.Modulate = Colors.White;

		StateMachine.ChangeState(StateMachine.GetNode<State>("Dead"));
	}
	public void DamageFlash()
	{
		// Cancel any existing flash tween so it doesn't fight the new one
		_dmgFlashTween?.Kill();
		_dmgFlashTween = CreateTween();

		// 10x overbright dmgFlash (or use Colors.Red for a red tint)
		PlayerAnimation.Modulate = Colors.Red;

		// Tween back to normal over 0.15 seconds
		_dmgFlashTween.TweenProperty(PlayerAnimation, "modulate", Colors.White, 0.15f);
	}

}
