using UnityEngine;

public class CommonPick : Pickaxe
{
    public override int MiningPower => 1;

    public override float MiningSpeed => 12;

    public override int DropRate => 1;

    public override string Name => "CommonPick";

    public override string Description => "for starter";

    public override float Radius => 2f;
}