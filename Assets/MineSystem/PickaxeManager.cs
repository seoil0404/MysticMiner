using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PickaxeManager : MonoBehaviour
{
    private Pickaxe pickaxe;

    private Rigidbody rigidBody;
    private SphereCollider sphereCollider;

    public void Initialize(Pickaxe pickaxe)
    {
        this.pickaxe = pickaxe;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = pickaxe.Radius;
        sphereCollider.isTrigger = true;

        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Ore>(out var ore))
        {
            ore.OnHit(pickaxe.MiningPower, pickaxe);
        }
    }
}
