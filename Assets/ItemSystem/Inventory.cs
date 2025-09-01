using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;

public static class Inventory
{
    private static Dictionary<Type, List<Item>> data = new() 
    {
        { typeof(CommonSword), new() },
        { typeof(UnCommonSword), new() }
    };

    public static IReadOnlyDictionary<Type, IReadOnlyList<Item>> Data
    {
        get => data.ToDictionary(
            keyValuePair => keyValuePair.Key, 
            keyValuePair => (IReadOnlyList<Item>)keyValuePair.Value.AsReadOnly()
            );
    }

    public static int SlotCount
    {
        get
        {
            int count = 0;

            foreach( var item in data.Values )
            {
                count += item.Count;
            }

            return count;
        }
    }

    public static void AddItem(Item item)
    {
        if(data.TryGetValue(item.GetType(), out var list))
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return;
            }

            if(item is IStackableItem stackableItem)
            {
                ((IStackableItem)list[0]).Count += stackableItem.Count;
            }
            else list.Add(item);

            return;
        }
        throw new NullReferenceException("Inventory don't have every item: " + nameof(item));
    }

    public static void RemoveItem(Item item)
    {
        if (data.TryGetValue(item.GetType(), out var list))
        {
            if (list.Count == 0) throw new NullReferenceException("Tried Remove Item that does NOT EXIST: " + nameof(item));

            if (item is IStackableItem stackableItem)
            {
                ((IStackableItem)list[0]).Count -= stackableItem.Count;
                if(((IStackableItem)list[0]).Count == 0) list.Clear();
            }
            else list.Remove(item);

            return;
        }
        throw new NullReferenceException("Inventory don't have every item: " + nameof(Item));
    }

    public static void DecreaseItem(IStackableItem item, int count)
    {
        IStackableItem currentItem = ((IStackableItem)data[item.GetType()][0]);

        currentItem.Count -= count;
        if (currentItem.Count== 0) data[item.GetType()].Clear();
    }
}
