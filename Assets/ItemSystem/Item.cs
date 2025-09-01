using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class Item
{
    private static Dictionary<Type, Sprite> itemSprites = new();
    public Sprite Sprite
    {
        get
        {
            if (itemSprites.TryGetValue(GetType(), out var sprite))
                return sprite;

            sprite = Resources.Load<Sprite>($"Item/Sprite/{Name}");
            itemSprites.Add(GetType(), sprite);

            return sprite;
        }
    }

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

public interface IStackableItem
{
    public int Count { get; set; }
}

public abstract class CommonItem : Item, IStackableItem
{
    public override ItemType Type => ItemType.Common;

    int IStackableItem.Count { get; set; } = 1;
}

public abstract class EquipmentItem : Item
{
    public override ItemType Type => ItemType.Equipment;
    public abstract EquipmentKind EquipmentType { get; }

    public enum EquipmentKind
    {
        Weapon,
        Pickaxe
    }
}

public abstract class Pickaxe : EquipmentItem
{
    public override EquipmentKind EquipmentType => EquipmentKind.Pickaxe;
    public abstract float MiningPower { get; }
    public abstract float MiningSpeed { get; }
    public abstract float DropRate { get; }
}

public abstract class Weapon : EquipmentItem
{
    public override EquipmentKind EquipmentType => EquipmentKind.Weapon;
    public abstract float Strengh { get; }
    public abstract float Power { get; }
    public abstract float Defense { get; }
    public abstract float Health { get; }
    public abstract float Speed { get; }
    public abstract float Mana { get; }
    public abstract float AttackSpeed { get; }
    public abstract float CriticalChance { get; }
    public abstract float CriticalMultiply { get; }
    public virtual float FixedDamage { get; } = 0f;
}

public abstract class ConsumableItem : Item, IStackableItem
{
    public override ItemType Type => ItemType.Consumable;

    int IStackableItem.Count { get; set; } = 1;

    public abstract void Use();
}

public abstract class ArtifactItem : Item
{
    public override ItemType Type => ItemType.Artifact;

    public abstract void OnAddedToInventory();
    public abstract void OnRemovedFromInventory();
}
