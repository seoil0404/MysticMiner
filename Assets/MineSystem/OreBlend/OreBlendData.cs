using UnityEngine;

[CreateAssetMenu(fileName = "OreBlendData", menuName = "Scriptable Objects/OreBlendData")]
public class OreBlendData : ScriptableObject
{
    public Ore[] Ores;

    public Ore GetRandomOre()
    {
        int randomIndex = Random.Range(0, Ores.Length);

        return Ores[randomIndex];
    }
}
