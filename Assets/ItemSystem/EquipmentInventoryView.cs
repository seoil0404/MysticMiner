using UnityEngine;

public class EquipmentInventoryView : MonoBehaviour
{
    [SerializeField] private EquipmentInventorySelectedItemView selectedItemView;
    [SerializeField] private EquipmentInventoryItemView mainEquipmentItemView;

    public EquipmentInventorySelectedItemView SelectedItemView;

    public static EquipmentInventoryView Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;

        Refresh();
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        mainEquipmentItemView.Item = EquipmentInventory.MainEquipment;
        selectedItemView.Refresh();
    }
}
