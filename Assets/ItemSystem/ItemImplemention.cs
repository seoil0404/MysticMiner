using UnityEngine;

public class CommonPick : Pickaxe
{
    public override int MiningPower => 1;

    public override float MiningSpeed => 5;

    public override int DropRate => 1;

    public override string Name => "CommonPick";

    public override string Description => "Common Pick for test";

    public override float Radius => 2f;
}