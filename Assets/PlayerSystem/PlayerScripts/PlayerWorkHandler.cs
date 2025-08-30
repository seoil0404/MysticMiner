using UnityEngine;

public class PlayerWorkHandler : MonoBehaviour
{
    private bool isMining = false;

    public void Attack()
    {

    }

    public void Mine()
    {
        PlayerController.PlayerContext.RenderManager.Mine();
    }

    public void UnMine()
    {
        PlayerController.PlayerContext.RenderManager.UnMine();
    }
}
