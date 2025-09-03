using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActionView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isActioning = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isActioning = true;
        PlayerController.PlayerContext.WorkHandler.Act();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isActioning = false;
        PlayerController.PlayerContext.WorkHandler.UnAct();
    }
}
