using System.Collections.Generic;
using UnityEngine;

public class ChunkTerrainManager : MonoBehaviour
{
    [Header("Terrain Settings")]
    public int chunkSize = 32;
    public float noiseScale = 20f;
    public float heightScale = 5f;
    public int viewDistance = 2;
    [SerializeField] private int seed = 0;
    [SerializeField] private float offsetX = 1000f;
    [SerializeField] private float offsetZ = 1000f;
    [SerializeField] private float baseHeight = -10f;
    [SerializeField] private float peakThreshold = 0.7f;
    [SerializeField] private float peakMultiplier = 3f;

    [Header("References")]
    public Transform player;
    public Material terrainMaterial;
    [SerializeField] private bool useColliders = false; // 필요 시만 MeshCollider 켜기

    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    private Queue<GameObject> chunkPool = new Queue<GameObject>();

    private Vector2Int lastPlayerChunk = Vector2Int.zero;

    // --- 노이즈 오프셋 (Start에서 한 번만 계산) ---
    private float noiseOffsetX;
    private float noiseOffsetZ;

    private void Start()
    {
        System.Random prng = new System.Random(seed);
        noiseOffsetX = prng.Next(-100000, 100000) + offsetX;
        noiseOffsetZ = prng.Next(-100000, 100000) + offsetZ;

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
            mr.material = terrainMaterial;

            if (useColliders)
            {
                newChunk.AddComponent<MeshCollider>();
            }

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
        mf.sharedMesh = mesh;

        if (useColliders)
        {
            MeshCollider mc = chunk.GetComponent<MeshCollider>();
            if (mc != null) mc.sharedMesh = mesh;
        }
    }

    Mesh GenerateChunkMesh(Vector2Int coord)
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[(chunkSize + 1) * (chunkSize + 1)];
        int[] triangles = new int[chunkSize * chunkSize * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        int vertIndex = 0, triIndex = 0;

        int octaves = 4;
        float persistence = 0.5f;
        float lacunarity = 2.0f;

        for (int z = 0; z <= chunkSize; z++)
        {
            for (int x = 0; x <= chunkSize; x++)
            {
                int worldX = x + coord.x * chunkSize;
                int worldZ = z + coord.y * chunkSize;

                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (worldX + noiseOffsetX) / noiseScale * frequency;
                    float sampleZ = (worldZ + noiseOffsetZ) / noiseScale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleZ);
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                float y = noiseHeight * heightScale;

                if (noiseHeight > peakThreshold)
                {
                    float t = (noiseHeight - peakThreshold) / (1f - peakThreshold);
                    y += t * heightScale * (peakMultiplier - 1f);
                }

                y += baseHeight;

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
