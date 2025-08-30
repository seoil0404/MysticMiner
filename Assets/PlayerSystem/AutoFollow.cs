using UnityEngine;

public class AutoFollow : MonoBehaviour
{
    [SerializeField] private Transform followTarget;

    private void Update()
    {
        transform.position = followTarget.position;
    }
}
