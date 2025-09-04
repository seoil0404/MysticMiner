using UnityEngine;

public class PlayerMiningAnimManager : MonoBehaviour
{
    public void On()
    {
        PlayerController.PlayerContext.WorkHandler.ActivePickaxe();
    }

    public void Off()
    {
        PlayerController.PlayerContext.WorkHandler.DeActivePickaxe();
    }
}
