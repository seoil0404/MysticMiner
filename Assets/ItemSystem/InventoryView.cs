using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image slotPrefab;
    [SerializeField] private ScrollRect scrollRect;      // �ֻ��� ScrollRect
    [SerializeField] private GridLayoutGroup gridLayout; // Content (GridLayoutGroup ���� ������Ʈ)

    private List<GameObject> slots = new();

    [Header("Layout Settings")]
    [SerializeField] private int columnCount = 5; // ���� �� ����
    [SerializeField] private int padding = 20;    // ���Ʒ� �е�

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        // ���� ���� ����
        foreach (var slot in slots)
            Destroy(slot);
        slots.Clear();

        // �κ��丮 ������ �� ����
        int itemCount =
            Inventory.Stackables.Count +
            Inventory.Equipments.Count +
            Inventory.StoredArtifacts.Count;

        // �׽�Ʈ�� �߰�
        itemCount += 50;

        // ���� ����
        for (int i = 0; i < itemCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab.gameObject, gridLayout.transform);
            slots.Add(slot);
        }

        // ���̾ƿ� ����
        AdjustGridLayout();
    }

    private void AdjustGridLayout()
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columnCount;

        // �� ũ�� = slotPrefab ũ��
        RectTransform slotRect = slotPrefab.GetComponent<RectTransform>();
        gridLayout.cellSize = slotRect.sizeDelta;

        // ����
        gridLayout.childAlignment = TextAnchor.UpperCenter;

        // �е� ����
        gridLayout.padding.top = padding;
        gridLayout.padding.bottom = padding;

        // Content ũ�� ����
        RectTransform contentRect = gridLayout.GetComponent<RectTransform>();

        int itemCount = slots.Count;
        int rowCount = Mathf.CeilToInt(itemCount / (float)columnCount);

        float height = (gridLayout.cellSize.y * rowCount) +
                       (gridLayout.spacing.y * (rowCount - 1)) +
                       gridLayout.padding.top + gridLayout.padding.bottom;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);

        // ScrollRect.content ���� ����
        scrollRect.content = contentRect;
    }
}
