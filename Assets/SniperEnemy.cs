using UnityEngine;
using System.Collections;

public class SniperEnemy : Enemy
{
    /// <summary>
    /// Projectile prefab for the sniper to shoot.
    /// </summary>
    public GameObject projectilePrefab;
    
    /// <summary>
    /// How often the sniper shoots (in seconds).
    /// </summary>
    public float shootInterval = 2f;
    
    /// <summary>
    /// Preferred distance from the player.
    /// </summary>
    public float preferredDistance = 30f;
    
    /// <summary>
    /// Timer for shooting.
    /// </summary>
    private float shootTimer;
    
    /// <summary>
    /// Position from which projectiles are fired.
    /// </summary>
    public Transform firePoint;

    protected override void Start()
    {
        base.Start();
        
        // Initialize sniper-specific properties
        enemyType = "Sniper";
        health = 2;
        pointValue = 200;
        
        // Initialize shooting timer
        shootTimer = Random.Range(0f, shootInterval);
        
        // Create fire point if not assigned
        if (firePoint == null)
        {
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.parent = transform;
            firePointObj.transform.localPosition = new Vector3(0, 0, 0);
            firePoint = firePointObj.transform;
        }
    }

    protected override void Update()
    {
        base.Update();
        
        // Handle shooting
        if (shootTimer <= 0)
        {
            HandleShooting();
            shootTimer = shootInterval;
        }
        else
        {
            shootTimer -= Time.deltaTime;
        }
    }

    protected override void HandleMovement()
    {
        if (player == null)
            return;
            
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // If too close to player, move backward
        if (distanceToPlayer < preferredDistance)
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        // If at good distance, stay in place and aim at player
        else
        {
            // Look at player's position
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0; // Keep level on y-axis
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
    
    private void HandleShooting()
    {
        if (projectilePrefab == null || player == null)
            return;
            
        // Calculate direction to player
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;
        
        // Create projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(directionToPlayer));
        
        // Initialize projectile
        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        if (projectileComponent != null)
        {
            projectileComponent.Initialize(false, directionToPlayer);
        }
        
        // Destroy projectile after 5 seconds if it hasn't hit anything
        Destroy(projectile, 5f);
    }
}