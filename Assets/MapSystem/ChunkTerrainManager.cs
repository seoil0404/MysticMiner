using System.Collections.Generic;
using UnityEngine;

public class ChunkTerrainManager : MonoBehaviour
{
    [Header("Terrain Settings")]
    public int chunkSize = 32;    
    public float noiseScale = 20f;
    public float heightScale = 5f;
    public int viewDistance = 2;  

    [Header("References")]
    public Transform player;
    public Material terrainMaterial;

    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    private Queue<GameObject> chunkPool = new Queue<GameObject>();

    private Vector2Int lastPlayerChunk = Vector2Int.zero;

    private void Start()
    {
        Vector2Int currentChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.z / chunkSize)
        );

        UpdateChunks(currentChunk);
        lastPlayerChunk = currentChunk;
    }

    void Update()
    {
        if (player == null) return;

        Vector2Int currentChunk = new Vector2Int(
            Mathf.FloorToInt(player.position.x / chunkSize),
            Mathf.FloorToInt(player.position.z / chunkSize)
        );

        // 플레이어가 새로운 청크에 들어갔을 때만 로딩 수행
        if (currentChunk != lastPlayerChunk)
        {
            UpdateChunks(currentChunk);
            lastPlayerChunk = currentChunk;
        }
    }

    void UpdateChunks(Vector2Int currentChunk)
    {

        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int z = -viewDistance; z <= viewDistance; z++)
            {
                Vector2Int coord = currentChunk + new Vector2Int(x, z);
                if (!activeChunks.ContainsKey(coord))
                {
                    GameObject chunk = GetChunkFromPool();
                    UpdateChunkMesh(chunk, coord);
                    activeChunks.Add(coord, chunk);
                }
            }
        }

        List<Vector2Int> removeList = new List<Vector2Int>();
        foreach (var kvp in activeChunks)
        {
            if (Vector2Int.Distance(kvp.Key, currentChunk) > viewDistance)
            {
                ReturnChunkToPool(kvp.Value);
                removeList.Add(kvp.Key);
            }
        }
        foreach (var key in removeList) activeChunks.Remove(key);
    }

    GameObject GetChunkFromPool()
    {
        if (chunkPool.Count > 0)
        {
            GameObject chunk = chunkPool.Dequeue();
            chunk.SetActive(true);
            return chunk;
        }
        else
        {
            GameObject newChunk = new GameObject("Chunk");
            newChunk.transform.parent = transform;

            MeshFilter mf = newChunk.AddComponent<MeshFilter>();
            MeshRenderer mr = newChunk.AddComponent<MeshRenderer>();
            MeshCollider mc = newChunk.AddComponent<MeshCollider>();

            mr.material = terrainMaterial;
            return newChunk;
        }
    }

    void ReturnChunkToPool(GameObject chunk)
    {
        chunk.SetActive(false);
        chunkPool.Enqueue(chunk);
    }

    void UpdateChunkMesh(GameObject chunk, Vector2Int coord)
    {
        chunk.name = $"Chunk_{coord.x}_{coord.y}";
        chunk.transform.position = new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);

        Mesh mesh = GenerateChunkMesh(coord);

        MeshFilter mf = chunk.GetComponent<MeshFilter>();
        MeshCollider mc = chunk.GetComponent<MeshCollider>();

        mf.sharedMesh = mesh;
        mc.sharedMesh = mesh;
    }

    Mesh GenerateChunkMesh(Vector2Int coord)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        int[] triangles = new int[chunkSize * chunkSize * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        int vertIndex = 0, triIndex = 0;
        for (int z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++)
            {
                int worldX = x + coord.x * chunkSize;
                int worldZ = z + coord.y * chunkSize;

                float y = Mathf.PerlinNoise(worldX / noiseScale, worldZ / noiseScale) * heightScale;
                vertices[vertIndex] = new Vector3(x, y, z);
                uvs[vertIndex] = new Vector2((float)x / chunkSize, (float)z / chunkSize);

                if (x < chunkSize && z < chunkSize)
                {
                    triangles[triIndex] = vertIndex;
                    triangles[triIndex + 1] = vertIndex + chunkSize + 1;
                    triangles[triIndex + 2] = vertIndex + 1;

                    triangles[triIndex + 3] = vertIndex + 1;
                    triangles[triIndex + 4] = vertIndex + chunkSize + 1;
                    triangles[triIndex + 5] = vertIndex + chunkSize + 2;

                    triIndex += 6;
                }
                vertIndex++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
