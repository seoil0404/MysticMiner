using UnityEngine;

public class PlayerRenderManager : MonoBehaviour
{
    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform modelArmTransform;
    [SerializeField] private Animator animator;

    private GameObject currentItemModel = null;

    private EquipmentItem currentEquipmentItem = null;

    private void Update()
    {
        animator.SetFloat("Speed", PlayerController.PlayerState.Speed);
        HandleRotation();
    }

    private void HandleRotation()
    {
        if (Mathf.Approximately(Joystick.Direction.magnitude, 0)) return;

        float angle = Mathf.Acos(Joystick.Direction.normalized.x) * Mathf.Rad2Deg;

        // acos는 0~180 범위만 나오므로 y의 부호로 방향 보정
        if (Joystick.Direction.normalized.y < 0)
            angle = 360f - angle;

        modelTransform.localRotation = Quaternion.Euler(0f, -angle + 90f, 0f); 
    }

    public void Attack()
    {

    }

    public void Mine()
    {
        if(currentEquipmentItem is Pickaxe pickaxe)
            animator.speed = pickaxe.MiningSpeed;

        animator.SetBool("IsMining", true);
    }

    public void UnMine()
    {
        animator.speed = 1;
        animator.SetBool("IsMining", false);
    }

    public void OnEquip(EquipmentItem item)
    {
        if(currentItemModel != null) Destroy(currentItemModel);

        currentEquipmentItem = item;

        GameObject itemModel = Instantiate(item.Model, modelArmTransform);
        currentItemModel = itemModel;

        PlayerController.PlayerContext.WorkHandler.OnEquip(item, itemModel);
    }
}