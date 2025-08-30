using System.Collections.Generic;

public static class Inventory
{
    // --- ���� ����� ---
    private static Dictionary<string, int> stackables = new();   // Common, Consumable
    private static List<EquipmentItem> equipments = new();       // Equipment
    private static List<ArtifactItem> artifacts = new();         // Artifact (ȿ�� �ߵ�)
    private static List<ArtifactItem> storedArtifacts = new();   // �ʰ� ��Ƽ��Ʈ (ȿ�� ����)

    // --- ��Ƽ��Ʈ ���� ---
    public static int MaxArtifacts { get; private set; } = 3;

    public static void SetMaxArtifacts(int newLimit)
    {
        if (newLimit < 0)
            throw new System.ArgumentException("MaxArtifacts�� 0 �̻��̾�� �մϴ�.");

        // ���� ���� ���� Artifact�� �ʰ��ϸ� �Ϲ� �κ��丮�� �̵�
        if (newLimit < artifacts.Count)
        {
            int excess = artifacts.Count - newLimit;
            for (int i = 0; i < excess; i++)
            {
                var lastArtifact = artifacts[^1];
                lastArtifact.OnRemovedFromInventory();
                artifacts.RemoveAt(artifacts.Count - 1);
                storedArtifacts.Add(lastArtifact); // ȿ�� ���� ���� ��ȯ
            }
        }

        MaxArtifacts = newLimit;
    }

    // --- ������ �߰� ---
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
                    return false; // �ߺ� ��� �Ұ�
                equipments.Add((EquipmentItem)item);
                return true;

            case Item.ItemType.Artifact:
                // �̹� ���� ���̸� �ź� (�ߺ� �Ұ�)
                if (artifacts.Exists(a => a.Name == item.Name) || storedArtifacts.Exists(a => a.Name == item.Name))
                    return false;

                if (artifacts.Count < MaxArtifacts)
                {
                    artifacts.Add((ArtifactItem)item);
                    ((ArtifactItem)item).OnAddedToInventory();
                }
                else
                {
                    storedArtifacts.Add((ArtifactItem)item); // �Ϲ� �κ��丮�θ� ����
                }
                return true;

            default:
                return false;
        }
    }

    // --- ������ ���� ---
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

    // --- ��ȸ�� ---
    public static IReadOnlyDictionary<string, int> Stackables => stackables;
    public static IReadOnlyList<EquipmentItem> Equipments => equipments;
    public static IReadOnlyList<ArtifactItem> Artifacts => artifacts;          // ȿ�� �ߵ� ��
    public static IReadOnlyList<ArtifactItem> StoredArtifacts => storedArtifacts; // ȿ�� ����
}
