using UnityEngine;

public class TreasurePlacer : MonoBehaviour
{
    [Header("Treasure Settings")]
    public GameObject treasureBoxPrefab;

    [Header("Fixed Position")]
    public float spawnX = 0f;
    public float spawnY = 0f;
    public float spawnZ = 0f;

    void Start()
    {
        PlaceTreasureBox();
    }

    void PlaceTreasureBox()
    {
        Vector3 spawnPos = new Vector3(spawnX, spawnY, spawnZ);
        Instantiate(treasureBoxPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Treasure placed at: " + spawnPos);
    }
}