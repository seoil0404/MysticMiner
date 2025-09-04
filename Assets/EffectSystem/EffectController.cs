using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}
