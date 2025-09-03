using UnityEngine;

public static class EquipmentInventory
{
    private static EquipmentItem mainEquipment = null;

    public static EquipmentItem MainEquipment
    {
        get => mainEquipment;
        set
        {
            if(mainEquipment != null)
                Inventory.AddItem(mainEquipment);

            Inventory.RemoveItem(value);
            mainEquipment = value;
            PlayerController.PlayerContext.RenderManager.OnEquip(mainEquipment);
        }
    }
}
