using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerOrbSpawner : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap obstaclesTilemap;
    public GameObject powerOrbPrefab;
    public GameObject monkey;
    public PowerUpController powerUpController;  // Reference to the PowerUpController

    public float spawnInterval = 10f;

    void Start()
    {
        StartCoroutine(SpawnPowerOrbRoutine());
    }

    IEnumerator SpawnPowerOrbRoutine()
    {
        while (true)
        {
            SpawnPowerOrb();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnPowerOrb()
    {
        Vector3Int randomTilePos = GetRandomGroundTile();

        if (randomTilePos != Vector3Int.zero && powerOrbPrefab != null)
        {
            Vector3 spawnPosition = groundTilemap.CellToWorld(randomTilePos) + new Vector3(0.5f, 0.5f, 0);

            GameObject newPowerOrb = Instantiate(powerOrbPrefab, spawnPosition, Quaternion.identity);

            PowerOrbScript orbScript = newPowerOrb.GetComponent<PowerOrbScript>();
            
            // Now pass both the monkey and powerUpController to the Initialize method
            orbScript.Initialize(monkey, powerUpController);

            Debug.Log("PowerOrb spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("No valid tile for PowerOrb or prefab missing.");
        }
    }

    Vector3Int GetRandomGroundTile()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int i = 0; i < 100; i++)
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int randomTilePos = new Vector3Int(randomX, randomY, 0);

            if (groundTilemap.HasTile(randomTilePos) && !obstaclesTilemap.HasTile(randomTilePos))
            {
                return randomTilePos;
            }
        }

        return Vector3Int.zero;
    }
}
