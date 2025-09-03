using UnityEngine;
using UnityEngine.Rendering;

public abstract class Ore : MonoBehaviour
{
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
            Inventory.AddItem(OreItem, pickaxe.DropRate);

            Destroy(gameObject);
        }
    }
}
