using UnityEngine;

public class PlayerRenderManager : MonoBehaviour
{
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Animator animator;

    private void Update()
    {
        animator.SetFloat("Speed", PlayerController.PlayerState.Speed);
        HandleRotation();
    }

    private void HandleRotation()
    {
        float angle = Mathf.Acos(Joystick.Direction.normalized.x) * Mathf.Rad2Deg;

        // acos는 0~180 범위만 나오므로 y의 부호로 방향 보정
        if (Joystick.Direction.normalized.y < 0)
            angle = 360f - angle;

        modelTransform.rotation = Quaternion.Euler(0f, -angle + 90f, 0f); 
    }

    public void Attack()
    {

    }

    public void Mine()
    {
        animator.SetBool("IsMining", true);
    }

    public void UnMine()
    {
        animator.SetBool("IsMining", false);
    }
}