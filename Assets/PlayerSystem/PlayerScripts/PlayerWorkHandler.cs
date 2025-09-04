using UnityEngine;

public class PlayerWorkHandler : MonoBehaviour
{
    public void Act()
    {
        if (EquipmentInventory.MainEquipment == null) return;

        switch(EquipmentInventory.MainEquipment.EquipmentType)
        {
            case EquipmentItem.EquipmentKind.Weapon:
                Attack();
                break;
            case EquipmentItem.EquipmentKind.Pickaxe:
                Mine();
                break;
        }
    }

    public void UnAct()
    {
        if (EquipmentInventory.MainEquipment == null) return;

        switch (EquipmentInventory.MainEquipment.EquipmentType)
        {
            case EquipmentItem.EquipmentKind.Weapon:
                break;
            case EquipmentItem.EquipmentKind.Pickaxe:
                UnMine();
                break;
        }
    }

    public void OnEquip(EquipmentItem item, GameObject itemModel)
    {
        if (item is Pickaxe pick)
        {
            PickaxeManager pickaxeManager = itemModel.AddComponent<PickaxeManager>();
            pickaxeManager.Initialize(pick);
        }
        else throw new System.NotImplementedException();
    }

    private void Attack()
    {

    }

    private void Mine()
    {
        PlayerController.PlayerState.IsMining = true;
        PlayerController.PlayerContext.RenderManager.Mine();
    }

    private void UnMine()
    {
        PlayerController.PlayerState.IsMining = false;
        PlayerController.PlayerContext.RenderManager.UnMine();
    }
}
