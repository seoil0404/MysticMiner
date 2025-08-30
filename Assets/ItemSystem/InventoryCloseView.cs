using UnityEngine;

public class InventoryCloseView : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;

    public void CloseInventory()
    {
        inventoryView.gameObject.SetActive(false);
    }
}
