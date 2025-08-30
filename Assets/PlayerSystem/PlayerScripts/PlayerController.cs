using UnityEngine;

[RequireComponent(typeof(PlayerMovementHandler))]
[RequireComponent(typeof(PlayerHealthManager))]
[RequireComponent(typeof(PlayerWorkHandler))]
[RequireComponent(typeof(PlayerSoundManager))]
[RequireComponent(typeof(PlayerRenderManager))]
public class PlayerController : MonoBehaviour
{
    private static PlayerController instance { get; set; }

    public static PlayerContext PlayerContext => instance.playerContext;
    public static PlayerState PlayerState => instance.playerState;

    private PlayerMovementHandler movementHandler;
    private PlayerHealthManager healthManager;
    private PlayerSoundManager soundManager;
    private PlayerRenderManager renderManager;
    private PlayerWorkHandler workHandler;

    private PlayerContext playerContext = new();
    private PlayerState playerState = new();

    private void Awake()
    {
        SetSingleTone();

        movementHandler = GetComponent<PlayerMovementHandler>();
        healthManager = GetComponent<PlayerHealthManager>();
        soundManager = GetComponent<PlayerSoundManager>();
        renderManager = GetComponent<PlayerRenderManager>();
        workHandler = GetComponent<PlayerWorkHandler>();

        playerContext.MovementHandler = movementHandler;
        playerContext.HealthManager = healthManager;
        playerContext.SoundManager = soundManager;
        playerContext.RenderManager = renderManager;
        playerContext.WorkHandler = workHandler;
        
    }

    private void SetSingleTone()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
