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
		idle_left,
		idle_right,
		move_down,
		move_up,
		move_left,
		move_right,
		turn_down,
		turn_up,
		turn_left,
		turn_right,
		attack_down,
		attack_up,
		attack_left,
		attack_right,
		dmg_tkn_down,
		dmg_tkn_up,
		dmg_tkn_left,
		dmg_tkn_right,
		death
    }

	public enum EnemyState
	{
		Idle,
		Roaming,
		Attacking
	}

}
