using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image slotPrefab;
    [SerializeField] private ScrollRect scrollRect;      // 최상위 ScrollRect
    [SerializeField] private GridLayoutGroup gridLayout; // Content (GridLayoutGroup 붙은 오브젝트)

    private List<GameObject> slots = new();

    [Header("Layout Settings")]
    [SerializeField] private int columnCount = 5; // 고정 열 개수
    [SerializeField] private int padding = 20;    // 위아래 패딩

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        // 기존 슬롯 제거
        foreach (var slot in slots)
            Destroy(slot);
        slots.Clear();

        // 인벤토리 아이템 총 개수
        int itemCount =
            Inventory.Stackables.Count +
            Inventory.Equipments.Count +
            Inventory.StoredArtifacts.Count;

        // 테스트용 추가
        itemCount += 50;

        // 슬롯 생성
        for (int i = 0; i < itemCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab.gameObject, gridLayout.transform);
            slots.Add(slot);
        }

        // 레이아웃 조정
        AdjustGridLayout();
    }

    private void AdjustGridLayout()
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columnCount;

        // 셀 크기 = slotPrefab 크기
        RectTransform slotRect = slotPrefab.GetComponent<RectTransform>();
        gridLayout.cellSize = slotRect.sizeDelta;

        // 정렬
        gridLayout.childAlignment = TextAnchor.UpperCenter;

        // 패딩 적용
        gridLayout.padding.top = padding;
        gridLayout.padding.bottom = padding;

        // Content 크기 조정
        RectTransform contentRect = gridLayout.GetComponent<RectTransform>();

        int itemCount = slots.Count;
        int rowCount = Mathf.CeilToInt(itemCount / (float)columnCount);

        float height = (gridLayout.cellSize.y * rowCount) +
                       (gridLayout.spacing.y * (rowCount - 1)) +
                       gridLayout.padding.top + gridLayout.padding.bottom;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);

        // ScrollRect.content 연결 보장
        scrollRect.content = contentRect;
    }
}
