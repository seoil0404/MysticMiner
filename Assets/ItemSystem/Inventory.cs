using System.Collections.Generic;

public static class Inventory
{
    // --- 내부 저장소 ---
    private static Dictionary<string, int> stackables = new();   // Common, Consumable
    private static List<EquipmentItem> equipments = new();       // Equipment
    private static List<ArtifactItem> artifacts = new();         // Artifact (효과 발동)
    private static List<ArtifactItem> storedArtifacts = new();   // 초과 아티팩트 (효과 없음)

    // --- 아티팩트 제한 ---
    public static int MaxArtifacts { get; private set; } = 3;

    public static void SetMaxArtifacts(int newLimit)
    {
        if (newLimit < 0)
            throw new System.ArgumentException("MaxArtifacts는 0 이상이어야 합니다.");

        // 현재 보유 중인 Artifact가 초과하면 일반 인벤토리로 이동
        if (newLimit < artifacts.Count)
        {
            int excess = artifacts.Count - newLimit;
            for (int i = 0; i < excess; i++)
            {
                var lastArtifact = artifacts[^1];
                lastArtifact.OnRemovedFromInventory();
                artifacts.RemoveAt(artifacts.Count - 1);
                storedArtifacts.Add(lastArtifact); // 효과 없는 보관 전환
            }
        }

        MaxArtifacts = newLimit;
    }

    // --- 아이템 추가 ---
    public static bool AddItem(Item item)
    {
        switch (item.Type)
        {
            case Item.ItemType.Common:
            case Item.ItemType.Consumable:
                if (stackables.ContainsKey(item.Name))
                    stackables[item.Name]++;
                else
                    stackables[item.Name] = 1;
                return true;

            case Item.ItemType.Equipment:
                if (equipments.Exists(e => e.Name == item.Name))
                    return false; // 중복 장비 불가
                equipments.Add((EquipmentItem)item);
                return true;

            case Item.ItemType.Artifact:
                // 이미 보유 중이면 거부 (중복 불가)
                if (artifacts.Exists(a => a.Name == item.Name) || storedArtifacts.Exists(a => a.Name == item.Name))
                    return false;

                if (artifacts.Count < MaxArtifacts)
                {
                    artifacts.Add((ArtifactItem)item);
                    ((ArtifactItem)item).OnAddedToInventory();
                }
                else
                {
                    storedArtifacts.Add((ArtifactItem)item); // 일반 인벤토리로만 저장
                }
                return true;

            default:
                return false;
        }
    }

    // --- 아이템 제거 ---
    public static bool RemoveItem(Item item)
    {
        switch (item.Type)
        {
            case Item.ItemType.Common:
            case Item.ItemType.Consumable:
                if (stackables.ContainsKey(item.Name))
                {
                    stackables[item.Name]--;
                    if (stackables[item.Name] <= 0)
                        stackables.Remove(item.Name);
                    return true;
                }
                return false;

            case Item.ItemType.Equipment:
                return equipments.Remove((EquipmentItem)item);

            case Item.ItemType.Artifact:
                if (artifacts.Remove((ArtifactItem)item))
                {
                    ((ArtifactItem)item).OnRemovedFromInventory();
                    return true;
                }
                return storedArtifacts.Remove((ArtifactItem)item);

            default:
                return false;
        }
    }

    // --- 조회용 ---
    public static IReadOnlyDictionary<string, int> Stackables => stackables;
    public static IReadOnlyList<EquipmentItem> Equipments => equipments;
    public static IReadOnlyList<ArtifactItem> Artifacts => artifacts;          // 효과 발동 중
    public static IReadOnlyList<ArtifactItem> StoredArtifacts => storedArtifacts; // 효과 없음
}
