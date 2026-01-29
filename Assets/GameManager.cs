// Assets/GameManager.cs
using UnityEngine;

/// <summary>
/// Manages the game state, including spawning enemies in the player's field of view.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the basic enemy prefab to spawn.
    /// </summary>
    public GameObject enemyPrefab;
    
    /// <summary>
    /// Reference to the sniper enemy prefab to spawn.
    /// </summary>
    public GameObject sniperEnemyPrefab;
    
    /// <summary>
    /// Reference to the zippy enemy prefab to spawn.
    /// </summary>
    public GameObject zippyEnemyPrefab;

    /// <summary>
    /// Probability of spawning a sniper enemy (0-1).
    /// </summary>
    public float sniperEnemyProbability = 0.2f;
    
    /// <summary>
    /// Probability of spawning a zippy enemy (0-1).
    /// </summary>
    public float zippyEnemyProbability = 0.3f;

    /// <summary>
    /// Reference to the player transform for spawning logic.
    /// </summary>
    public Transform player;

    /// <summary>
    /// How often to spawn enemies (in seconds).
    /// </summary>
    public float spawnInterval = 2f;

    /// <summary>
    /// Timer to track when to spawn the next enemy.
    /// </summary>
    private float spawnTimer;
    
    /// <summary>
    /// Distance in front of the player to spawn enemies.
    /// </summary>
    public float spawnDistance = 50f;
    
    /// <summary>
    /// Renamed and adjusted for world-space clamping
    /// </summary>
    public float spawnAreaTotalWidth = 36f; // Total width for spawning (e.g., matches 2 * shipScreenBounds.x)
    public float spawnAreaTotalHeight = 20f; // Total height for spawning (e.g., matches 2 * shipScreenBounds.y)

    void Start()
    {
        // Initialize game state and start enemy spawning.
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        // Handle enemy spawning each frame.
        HandleEnemySpawning();
    }

    /// <summary>
    /// Spawns an enemy at a random position in the player's field of view.
    /// </summary>
    void SpawnEnemy()
    {
        if (player == null)
            return;
            
        // Calculate a random X and Y position within the world-aligned spawn area.
        // Assumes the playable area is centered at world X=0, Y=0.
        float randomX = Random.Range(-spawnAreaTotalWidth / 2f, spawnAreaTotalWidth / 2f);
        float randomY = Random.Range(-spawnAreaTotalHeight / 2f, spawnAreaTotalHeight / 2f);
        
        // Spawn 'spawnDistance' in front of the player's current Z position, but at the randomX, randomY world coordinates.
        Vector3 spawnPosition = new Vector3(randomX, randomY, player.position.z + spawnDistance);
        
        // Instantiate the enemy facing towards the player's current X,Y at the player's Z depth.
        Vector3 targetPointForLook = player.position; // Enemy aims at the player's current position
        Quaternion spawnRotation = Quaternion.LookRotation(targetPointForLook - spawnPosition);
        
        // Determine which enemy type to spawn
        GameObject enemyToSpawn = DetermineEnemyType();
        
        if (enemyToSpawn != null)
        {
            GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, spawnRotation);
            
            // Destroy enemy after 15 seconds if it hasn't been destroyed already
            // This prevents enemies from accumulating if they're missed
            Destroy(enemy, 15f);
        }
    }
    
    /// <summary>
    /// Determines which enemy type to spawn based on probabilities.
    /// </summary>
    GameObject DetermineEnemyType()
    {
        float randomValue = Random.value;
        
        // Check for sniper enemy
        if (randomValue < sniperEnemyProbability && sniperEnemyPrefab != null)
        {
            return sniperEnemyPrefab;
        }
        // Check for zippy enemy
        else if (randomValue < sniperEnemyProbability + zippyEnemyProbability && zippyEnemyPrefab != null)
        {
            return zippyEnemyPrefab;
        }
        // Default to basic enemy
        else if (enemyPrefab != null)
        {
            return enemyPrefab;
        }
        
        // Fallback if no prefabs are assigned
        return enemyPrefab;
    }

    /// <summary>
    /// Handles the timing and logic for spawning enemies.
    /// Decrements timer and spawns enemies when timer reaches zero.
    /// </summary>
    void HandleEnemySpawning()
    {
        // Decrement the timer
        spawnTimer -= Time.deltaTime;
        
        // Check if it's time to spawn an enemy
        if (spawnTimer <= 0)
        {
            // Spawn an enemy
            SpawnEnemy();
            
            // Reset the timer
            spawnTimer = spawnInterval;
        }
    }
}