using UnityEngine;

public class InventoryActiveView : MonoBehaviour
{
    [SerializeField] private InventoryView inventoryView;

    public void ActiveInventory()
    {
        inventoryView.gameObject.SetActive(true);
    }
}
