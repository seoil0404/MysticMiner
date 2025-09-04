using UnityEngine;

public class Coal : Item, IStackableItem
{
    public override string Name => "Coal";

    public override string Description => "common ore";

    public override ItemType Type => ItemType.Common;

    public int Count { get; set; } = 1;
}

public class CoalOre : Ore
{
    public override Item OreItem => new Coal();

    public override int Health => 5;
}
