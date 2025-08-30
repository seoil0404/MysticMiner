using UnityEngine;

public class PlayerContext
{
    public PlayerMovementHandler MovementHandler { get; set; }
    public PlayerHealthManager HealthManager { get; set; }
    public PlayerSoundManager SoundManager { get; set; }
    public PlayerRenderManager RenderManager { get; set; }
    public PlayerCombatHandler CombatHandler { get; set; }
}

public class PlayerState
{

}
