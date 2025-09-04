using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public EffectData EffectData;
    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
