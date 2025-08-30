public abstract class Item
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract ItemType Type { get; }

    public enum ItemType
    {
        Common,
        Equipment,
        Consumable,
        Artifact
    }
}

public abstract class CommonItem : Item
{
    public override ItemType Type => ItemType.Common;
}

public abstract class EquipmentItem : Item
{
    public override ItemType Type => ItemType.Equipment;
}

public abstract class ConsumableItem : Item
{
    public override ItemType Type => ItemType.Consumable;
    public abstract void Use();
}

public abstract class ArtifactItem : Item
{
    public override ItemType Type => ItemType.Artifact;

    public abstract void OnAddedToInventory();
    public abstract void OnRemovedFromInventory();
}
