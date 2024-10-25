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

    public float spawnInterval = 1f;

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

    void SpawnPowerOrb() // spawns orb prefabs on valid tiles
    {
        Vector3Int randomTilePos = GetValidGroundTile();

        if (randomTilePos != Vector3Int.zero && powerOrbPrefab != null)
        {
            Vector3 spawnPosition = groundTilemap.CellToWorld(randomTilePos) + new Vector3(0.5f, 0.5f, 0);

            GameObject newPowerOrb = Instantiate(powerOrbPrefab, spawnPosition, Quaternion.identity);

            PowerOrbScript orbScript = newPowerOrb.GetComponent<PowerOrbScript>();
            orbScript.Initialize(monkey, powerUpController);
        }
    }

    Vector3Int GetValidGroundTile() // find tile that isn't obstacle or holding a collectible already
    {
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int i = 0; i < 100; i++) 
        {
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int randomTilePos = new Vector3Int(randomX, randomY, 0);

            if (groundTilemap.HasTile(randomTilePos) && !obstaclesTilemap.HasTile(randomTilePos))
            {
                Vector3 worldPosition = groundTilemap.CellToWorld(randomTilePos) + new Vector3(0.5f, 0.5f, 0);

                if (!IsTileOccupied(worldPosition))
                {
                    return randomTilePos;
                }
            }
        }

        return Vector3Int.zero; 
    }

    bool IsTileOccupied(Vector3 worldPosition) // detects collectibles
    {
        Collider2D collider = Physics2D.OverlapCircle(worldPosition, 0.1f);
        return collider != null;
    }
}
