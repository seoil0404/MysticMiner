using UnityEngine;

public class EquipmentInventoryActiveView : MonoBehaviour
{
    public void ActiveEquiptmentInventory()
    {
        EquipmentInventoryView.Instance.gameObject.SetActive(true);
    }
}
