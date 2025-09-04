using UnityEngine;

public class Stone : Item, IStackableItem
{
    public override string Name => "Stone";

    public override string Description => "Ore..?";

    public override ItemType Type => ItemType.Common;

    public int Count { get; set; } = 1;
}

public class StoneOre : Ore
{
    public override Item OreItem => new Stone();

    public override int Health => 5;
}
