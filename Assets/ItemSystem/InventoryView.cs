using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryItemView itemViewPrefab;
    [SerializeField] private ScrollRect scrollRect;      
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private InventorySelectedItemView selectedItemView;

    private List<InventoryItemView> itemViews = new();

    [Header("Layout Settings")]
    [SerializeField] private int columnCount = 5; 
    [SerializeField] private int padding = 20;

    public static InventoryView Instance { get; private set; }
    public InventorySelectedItemView SelectedItemView => selectedItemView;

    private void Awake()
    {
        if(Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    private void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (var itemView in itemViews)
            Destroy(itemView.gameObject);
        itemViews.Clear();

        foreach(var itemList in Inventory.Data.Values)
        {
            foreach(var item in itemList)
            {
                var itemView = Instantiate(itemViewPrefab, gridLayout.transform);
                itemView.Item = item;

                itemViews.Add(itemView);
            }
        }

        AdjustGridLayout();

        selectedItemView.Refresh();
    }

    private void AdjustGridLayout()
    {
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columnCount;

        // 셀 크기 = slotPrefab 크기
        RectTransform slotRect = itemViewPrefab.GetComponent<RectTransform>();
        gridLayout.cellSize = slotRect.sizeDelta;

        // 패딩 적용
        gridLayout.padding.top = padding;
        gridLayout.padding.bottom = padding;

        // Content 크기 조정
        RectTransform contentRect = gridLayout.GetComponent<RectTransform>();

        int itemCount = itemViews.Count;
        int rowCount = Mathf.CeilToInt(itemCount / (float)columnCount);

        float height = (gridLayout.cellSize.y * rowCount) +
                       (gridLayout.spacing.y * (rowCount - 1)) +
                       gridLayout.padding.top + gridLayout.padding.bottom;

        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, height);

        // ScrollRect.content 연결 보장
        scrollRect.content = contentRect;
    }
}
