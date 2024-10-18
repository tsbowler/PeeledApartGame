using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PowerOrbSpawner : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap obstaclesTilemap;
    public GameObject powerOrbPrefab;
    public GameObject monkey;
    public PowerUpController powerUpController;

    public float spawnInterval = 11.7f;

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
        Vector3Int randomTilePos = GetValidGroundTile(); // Use updated method to find a valid, unoccupied tile

        if (randomTilePos != Vector3Int.zero && powerOrbPrefab != null)
        {
            Vector3 spawnPosition = groundTilemap.CellToWorld(randomTilePos) + new Vector3(0.5f, 0.5f, 0);

            GameObject newPowerOrb = Instantiate(powerOrbPrefab, spawnPosition, Quaternion.identity);

            PowerOrbScript orbScript = newPowerOrb.GetComponent<PowerOrbScript>();
            orbScript.Initialize(monkey, powerUpController);
        }
    }

    Vector3Int GetValidGroundTile()
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int i = 0; i < 100; i++)  // Try up to 100 times to find an unoccupied tile
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int randomTilePos = new Vector3Int(randomX, randomY, 0);

            if (groundTilemap.HasTile(randomTilePos) && !obstaclesTilemap.HasTile(randomTilePos))
            {
                Vector3 worldPosition = groundTilemap.CellToWorld(randomTilePos) + new Vector3(0.5f, 0.5f, 0);

                // Check if the tile is occupied by any object
                if (!IsTileOccupied(worldPosition))
                {
                    return randomTilePos; // Return this tile if it's valid and unoccupied
                }
            }
        }

        return Vector3Int.zero; // Return invalid position if no valid tile found
    }

    bool IsTileOccupied(Vector3 worldPosition)
    {
        Collider2D collider = Physics2D.OverlapCircle(worldPosition, 0.1f);
        return collider != null;
    }
}
