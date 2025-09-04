using UnityEngine;

public class Copper : Item, IStackableItem
{
    public override string Name => "Copper";

    public override string Description => "Ore";

    public override ItemType Type => ItemType.Common;

    public int Count { get; set; } = 1;
}

public class CopperOre : Ore
{
    public override Item OreItem => new Copper();

    public override int Health => 5;
}
