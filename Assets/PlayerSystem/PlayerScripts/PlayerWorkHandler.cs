using UnityEngine;

public class PlayerWorkHandler : MonoBehaviour
{
    public EquipmentItem EquipmentItem;

    public void Act()
    {
        switch(EquipmentItem.EquipmentType)
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
        switch (EquipmentItem.EquipmentType)
        {
            case EquipmentItem.EquipmentKind.Weapon:
                break;
            case EquipmentItem.EquipmentKind.Pickaxe:
                UnMine();
                break;
        }
    }

    private void Attack()
    {

    }

    private void Mine()
    {
        PlayerController.PlayerContext.RenderManager.Mine();
    }

    private void UnMine()
    {
        PlayerController.PlayerContext.RenderManager.UnMine();
    }
}
