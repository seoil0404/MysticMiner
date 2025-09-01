using UnityEditor.UI;
using UnityEngine;

public class EquipmentInventoryView : MonoBehaviour
{
    [SerializeField] private EquipmentInventorySelectedItemView selectedItemView;

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
        selectedItemView.Refresh();
    }
}
