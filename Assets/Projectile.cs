// Assets/Projectile.cs
using UnityEngine;


public class Projectile : MonoBehaviour
{
    // Variables to be set when projectile is instantiated
    private bool isPlayerProjectile;
    private float speed;
    private int damage = 1;
    
    // Constants for projectile properties
    public float playerProjectileSpeed = 60f; // Increased from 20f for easier enemy shooting
    public float enemyProjectileSpeed = 15f;
    
    // References to components
    private Renderer projectileRenderer;
    private Rigidbody rb;


    private void Awake()
    {
        projectileRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }


    private void Start()
    {
        // Set color based on source
        if (projectileRenderer != null)
        {
            if (isPlayerProjectile)
            {
                projectileRenderer.material.color = Color.blue;
            }
            else
            {
                projectileRenderer.material.color = Color.red;
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision with different objects
        if (collision.gameObject.CompareTag("Projectile"))
        {
            // Destroy both projectiles on collision
            Destroy(collision.gameObject);
            Destroy(gameObject);
            return;
        }

        if (isPlayerProjectile)
        {
            // Player projectiles damage enemies
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {
            // Enemy projectiles damage player
            if (collision.gameObject.CompareTag("Player"))
            {
                // TODO: Add player damage logic when health system is implemented
                Debug.Log("Player hit by enemy projectile!");
                Destroy(gameObject);
            }
        }
    }


    public void Initialize(bool fromPlayer, Vector3 direction)
    {
        isPlayerProjectile = fromPlayer;
        
        // Set appropriate speed based on source
        speed = isPlayerProjectile ? playerProjectileSpeed : enemyProjectileSpeed;
        
        // Set color based on source
        if (projectileRenderer != null)
        {
            if (isPlayerProjectile)
            {
                projectileRenderer.material.color = Color.blue;
            }
            else
            {
                projectileRenderer.material.color = Color.red;
            }
        }
        
        // Apply velocity in the specified direction
        if (rb != null)
        {
            rb.linearVelocity = direction.normalized * speed;
        }
    }
}