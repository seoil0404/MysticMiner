using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    private Vector2 inputVector;
    private bool isDragging = false;

    private static Joystick instance { get; set; }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            return;
        }

        instance = this;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos /= background.sizeDelta / 2f;

            inputVector = (pos.magnitude > 1.0f) ? pos.normalized : pos;

            handle.anchoredPosition = new Vector2(
                inputVector.x * (background.sizeDelta.x / 2f),
                inputVector.y * (background.sizeDelta.y / 2f));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public static float Horizontal => instance.inputVector.x;
    public static float Vertical => instance.inputVector.y;
    public static bool IsDragging => instance.isDragging;
    public static Vector2 Direction => new Vector2(Horizontal, Vertical);
}
