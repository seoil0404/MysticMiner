using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PickaxeManager : MonoBehaviour
{
    private Pickaxe pickaxe;

    private Rigidbody rigidBody;
    private SphereCollider sphereCollider;

    private Ore ore;

    public void Initialize(Pickaxe pickaxe)
    {
        this.pickaxe = pickaxe;

        rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = true;

        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = pickaxe.Radius;
        sphereCollider.center = new Vector3(0, 2, 0);
        sphereCollider.isTrigger = true;

        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void Mine()
    {
        Vector3 contactPoint = oreCollider.ClosestPoint(transform.position);

        Instantiate(EffectManager.Instance.EffectData.MineEffect).transform.position = contactPoint;

        oreCollider.GetComponent<Ore>().OnHit(pickaxe.MiningPower, pickaxe);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (oreCollider.TryGetComponent<Ore>(out var ore))
        {
            oreCollider = ore;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
