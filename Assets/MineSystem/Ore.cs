using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshCollider))]
public abstract class Ore : MonoBehaviour
{
    public static int Score = 0; // must be destroy later
    public abstract Item OreItem { get; }
    public abstract int Health { get; }
    
    private int currentHealth;

    private void Awake()
    {
        currentHealth = Health;
    }

    public void OnHit(int damage, Pickaxe pickaxe)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Score++;
            if (this is IStackableItem)
                Inventory.AddItem(OreItem, pickaxe.DropRate);
            else Inventory.AddItem(OreItem);

            Destroy(gameObject);
        }
    }
}
