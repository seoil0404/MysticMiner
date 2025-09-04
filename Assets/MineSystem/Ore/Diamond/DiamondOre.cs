using UnityEngine;

public class Diamond : Item, IStackableItem
{
    public override string Name => "Diamond";

    public override string Description => "Ore";

    public override ItemType Type => ItemType.Common;

    public int Count { get; set; } = 1;
}

public class DiamondOre : Ore
{
    public override Item OreItem => new Diamond();

    public override int Health => 10;
}
