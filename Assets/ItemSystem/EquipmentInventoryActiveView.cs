using UnityEngine;

public class EquipmentInventoryActiveView : MonoBehaviour
{
    [SerializeField] private EquipmentInventoryView inventoryView;
    public void ActiveEquiptmentInventory()
    {
         inventoryView.gameObject.SetActive(true);
    }
}
