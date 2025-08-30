using UnityEngine;

public class PlayerRenderManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Mine()
    {
        animator.SetBool("IsMining", true);
    }

    public void UnMine()
    {
        animator.SetBool("IsMining", false);
    }
}