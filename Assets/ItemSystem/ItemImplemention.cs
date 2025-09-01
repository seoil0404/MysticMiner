using UnityEngine;

public class CommonSword : ConsumableItem
{

    public override string Name => "CommonSword";

    public override string Description => "It is CommonSword As Test Item";

    public override void Use()
    {
        
    }
}

public class UnCommonSword : Weapon
{
    public override float Strengh => 1;

    public override float Power => 1;

    public override float Defense => 1;

    public override float Health => 1;

    public override float Speed => 1;

    public override float Mana => 1;

    public override float AttackSpeed => 1;

    public override float CriticalChance => 1;

    public override float CriticalMultiply => 1;

    public override string Name => "UncommonSword";

    public override string Description => "It is UncommonSword As Test Item";
}

