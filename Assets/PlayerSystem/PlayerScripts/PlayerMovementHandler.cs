using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementHandler : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private Rigidbody rigidBody;

    private Vector3 lastMousePos;

    public Vector3 Velocity
    {
        get => rigidBody.linearVelocity;
        set => rigidBody.linearVelocity = value;
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleVelocity();
        HandleRotate();
    }

    private void HandleRotate()
    {
        if (Joystick.IsDragging)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            transform.Rotate(Vector3.up * delta.x * rotationSpeed, Space.World);
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Moved)
            {
                float deltaX = touch.deltaPosition.x;
                transform.Rotate(Vector3.up * deltaX * rotationSpeed, Space.World);
            }
        }
#endif
    }

    private void HandleVelocity()
    {
        Vector2 input = Joystick.Direction;

        Vector3 moveDir = transform.forward * input.y + transform.right * input.x;

        Vector3 moveRate = moveDir * moveSpeed;
        moveRate.y = Velocity.y;

        Velocity = moveRate;
    }
}
