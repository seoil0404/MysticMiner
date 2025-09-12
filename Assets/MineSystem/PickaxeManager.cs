using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PickaxeManager : MonoBehaviour
{
    private Pickaxe pickaxe;

    private Rigidbody rigidBody;
    private SphereCollider sphereCollider;

    private List<OrePair> oreList = new();

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

    private void Update()
    {
        HandleMine();
    }

    private void HandleMine()
    {
        if (PlayerController.PlayerState.IsPickaxeActive)
        {
            oreList.RemoveAll(pair => pair == null || pair.Ore == null || pair.OreCollider == null);

            foreach (var pair in oreList)
            {
                Mine(pair);
            }

            oreList.Clear();
        }
    }

    public void Mine(OrePair pair)
    {
        if (pair.OreCollider == null)
            return;

        Vector3 contactPoint = transform.position;

        Instantiate(EffectManager.Instance.EffectData.MineEffect).transform.position = contactPoint;

        pair.Ore.OnHit(pickaxe.MiningPower, pickaxe);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Ore>(out var ore))
        {
            foreach (var pair in oreList)
            {
                if (pair.OreCollider = other) return;
            }
            oreList.Add(new OrePair(ore, other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Ore>(out var ore))
        {
            for(int index = 0; index < oreList.Count; index++)
            {
                if (oreList[index].OreCollider == other)
                {
                    oreList.RemoveAt(index);
                    break;
                }
            }
        }
    }
}

public class OrePair
{
    public Ore Ore;
    public Collider OreCollider;

    public OrePair(Ore ore, Collider oreCollider)
    {
        Ore = ore;
        OreCollider = oreCollider;
    }
}