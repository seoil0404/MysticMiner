using System.Collections.Generic;
using UnityEngine;

public class OreGenerator : MonoBehaviour
{
    [SerializeField] private OreBlendData oreBlendData;
    [SerializeField] private float maxGenerateRange;
    [SerializeField] private float generateRate;
    [SerializeField] private Vector3 generatePositionOffset;
    [SerializeField] private Transform generateParent;

    private HashSet<Vector3> generatedChunks = new();

    public void GenerateOre(Vector3[] vertices, Vector3 positionOffset)
    {
        if (generatedChunks.Contains(positionOffset)) return;
        generatedChunks.Add(positionOffset);

        foreach (var vertex in vertices)
        {
            if(vertex.y < maxGenerateRange && Random.Range(0, 100f) < generateRate)
            {
                Ore ore = Instantiate(oreBlendData.GetRandomOre(), generateParent);
                ore.transform.position = vertex + generatePositionOffset + positionOffset;
            }
        }
    }
}
