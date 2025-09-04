using UnityEngine;

public class PlayerContext
{
    public PlayerMovementHandler MovementHandler { get; set; }
    public PlayerHealthManager HealthManager { get; set; }
    public PlayerSoundManager SoundManager { get; set; }
    public PlayerRenderManager RenderManager { get; set; }
    public PlayerWorkHandler WorkHandler { get; set; }
}

public class PlayerState
{
    public float Speed => PlayerController.PlayerContext.MovementHandler.Speed;
    public bool IsMining = false;
    private bool isPickaxeActive = false;
    public bool IsPickaxeActive = false;
}
