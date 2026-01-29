using UnityEngine;

/// <summary>
/// Controls enemy behavior in the Space Kitsune prototype.
/// Handles movement and collision detection.
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Speed at which the enemy moves.
    /// </summary>
    public float moveSpeed = 5f;
    
    /// <summary>
    /// Points awarded when this enemy is destroyed.
    /// </summary>
    public int pointValue = 100;
    
    /// <summary>
    /// Health points of the enemy.
    /// </summary>
    public int health = 1;
    
    /// <summary>
    /// Reference to the player transform.
    /// </summary>
    protected Transform player;
    
    /// <summary>
    /// Enemy type identifier.
    /// </summary>
    public string enemyType = "Basic";

    protected virtual void Start()
    {
        // Find the player in the scene
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        // Handle enemy movement or actions each frame.
        HandleMovement();
    }

    /// <summary>
    /// Handles the enemy's movement logic.
    /// Moves the enemy toward the player at a constant speed.
    /// </summary>
    protected virtual void HandleMovement()
    {
        // Simple movement: move toward the player
        if (player != null)
        {
            // Move toward the player
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            // If player not found, just move backward (toward player's starting position)
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Handles what happens when the enemy is hit by a projectile or the player.
    /// </summary>
    public void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Destroy the projectile
            Destroy(collision.gameObject);
            
            // Take damage
            TakeDamage(1);
        }
        // Check if the collision is with the player
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Destroy the enemy
            Destroy(gameObject);
            
            // You could add player damage logic here
        }
    }
    
    /// <summary>
    /// Applies damage to the enemy and destroys it if health reaches zero.
    /// </summary>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            // TODO: Add score logic
            Destroy(gameObject);
        }
    }
}