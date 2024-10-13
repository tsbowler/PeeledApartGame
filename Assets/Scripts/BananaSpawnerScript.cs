using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BananaSpawnerScript : MonoBehaviour
{
    public Tilemap groundTilemap;  // Reference to ground tilemap
    public Tilemap obstaclesTilemap; // Reference to obstacle tilemap
    public GameObject bananaPrefab; // Banana prefab to spawn
    public GameObject monkey;  // Reference to the monkey
    public GeneralLogic generalLogic;  // Reference to GeneralLogic

    private float spawnInterval = 20f; // Interval in seconds to spawn a banana

    void Start()
    {
        // Start the coroutine to spawn bananas at regular intervals
        StartCoroutine(SpawnBananaRoutine());
    }

    IEnumerator SpawnBananaRoutine()
    {
        while (true) // Keep spawning bananas at intervals
        {
            SpawnBanana();
            yield return new WaitForSeconds(spawnInterval); // Wait for 20 seconds
        }
    }

    void SpawnBanana()
    {
        // Get a valid random position on the ground
        Vector3Int randomTilePos = GetRandomGroundTile();

        // Convert the tile position to world position for spawning the banana
        if (randomTilePos != Vector3Int.zero && bananaPrefab != null)
        {
            Vector3 spawnPosition = groundTilemap.CellToWorld(randomTilePos) + new Vector3(0.5f, 0.5f, 0); // Adjust to center the banana on the tile

            GameObject newBanana = Instantiate(bananaPrefab, spawnPosition, Quaternion.identity);
            
            // Initialize the banana with references to monkey and GeneralLogic
            BananaScript bananaScript = newBanana.GetComponent<BananaScript>();
            bananaScript.Initialize(monkey, generalLogic);

            Debug.Log("Banana spawned at: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("Could not find a valid ground tile to spawn a banana or bananaPrefab is missing.");
        }
    }

    Vector3Int GetRandomGroundTile()
    {
        // Get bounds of the ground tilemap
        BoundsInt bounds = groundTilemap.cellBounds;

        // Try random positions within the bounds until a valid ground tile is found
        for (int i = 0; i < 100; i++)  // Safety measure to avoid infinite loops
        {
            // Pick a random tile position within the bounds
            int randomX = Random.Range(bounds.xMin, bounds.xMax);
            int randomY = Random.Range(bounds.yMin, bounds.yMax);
            Vector3Int randomTilePos = new Vector3Int(randomX, randomY, 0);

            // Check if the random tile is a valid ground tile and not an obstacle
            if (groundTilemap.HasTile(randomTilePos) && !obstaclesTilemap.HasTile(randomTilePos))
            {
                return randomTilePos;
            }
        }

        // If no valid tile is found after 100 attempts, return Vector3Int.zero (invalid)
        return Vector3Int.zero;
    }
}
