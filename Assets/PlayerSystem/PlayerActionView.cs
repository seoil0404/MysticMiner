using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isActioning = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isActioning = true;
        if(PlayerController.PlayerState.EquipmentState == PlayerState.EquipmentStateType.Tool)
        {
            PlayerController.PlayerContext.WorkHandler.Mine();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isActioning = false;
        if (PlayerController.PlayerState.EquipmentState == PlayerState.EquipmentStateType.Tool)
        {
            PlayerController.PlayerContext.WorkHandler.UnMine();
        }
    }
}
