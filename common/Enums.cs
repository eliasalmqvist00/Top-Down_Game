namespace Game.Core
{
	public enum LogLevel
	{
		DEBUG,
		INFO,
		WARNING,
		ERROR
	}

	public enum ECharacterAnimation
    {
        idle_down,
		idle_up,
		idle_side,
		move_down,
		move_up,
		move_side,
		turn_down,
		turn_up,
		turn_side,
		attack_down,
		attack_up,
		attack_side,
		axe_down,
		axe_up,
		axe_side,
		slingshot_down,
		slingshot_side,
		slingshot_up,
		death
    }

	public enum EnemyState
	{
		Idle,
		Roaming,
		Attacking
	}

}
