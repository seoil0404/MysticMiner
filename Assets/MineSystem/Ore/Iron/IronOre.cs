using UnityEngine;

public class Iron : Item, IStackableItem
{
    public override string Name => "Iron";

    public override string Description => "Ore";

    public override ItemType Type => ItemType.Common;

    public int Count { get; set; } = 1;
}

public class IronOre : Ore
{
    public override Item OreItem => new Iron();

    public override int Health => 7;
}
