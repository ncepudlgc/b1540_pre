using UnityEngine;
using System.Collections;

public class ZippyEnemy : Enemy
{
    /// <summary>
    /// How often the zippy enemy changes direction (in seconds).
    /// </summary>
    public float directionChangeInterval = 1f;
    
    /// <summary>
    /// Maximum speed multiplier when zipping.
    /// </summary>
    public float zipSpeedMultiplier = 2.5f;
    
    /// <summary>
    /// Timer for direction changes.
    /// </summary>
    private float directionChangeTimer;
    
    /// <summary>
    /// Current movement direction.
    /// </summary>
    private Vector3 currentDirection;
    
    /// <summary>
    /// Whether the enemy is currently zipping.
    /// </summary>
    private bool isZipping = false;
    
    /// <summary>
    /// Range of movement in X and Y directions.
    /// </summary>
    public Vector2 movementRange = new Vector2(10f, 6f);

    protected override void Start()
    {
        base.Start();
        
        // Initialize zippy-specific properties
        enemyType = "Zippy";
        health = 1;
        pointValue = 150;
        
        // Initialize direction change timer
        directionChangeTimer = Random.Range(0f, directionChangeInterval);
        
        // Set initial direction
        ChangeDirection();
    }

    protected override void Update()
    {
        // Handle direction changes
        HandleDirectionChanges();
        
        // Handle movement (don't call base.Update() as we're overriding the movement)
        HandleMovement();
        
        // Clamp position to spawn bounds
        ClampPosition();
    }

    protected override void HandleMovement()
    {
        if (player == null)
            return;
            
        // Calculate base forward movement (always moving toward player on Z-axis)
        Vector3 forwardMovement = Vector3.back * moveSpeed * Time.deltaTime;
        
        // Calculate lateral movement based on current direction and zip state
        float currentSpeed = isZipping ? moveSpeed * zipSpeedMultiplier : moveSpeed;
        Vector3 lateralMovement = currentDirection * currentSpeed * Time.deltaTime;
        
        // Combine movements
        Vector3 totalMovement = forwardMovement + lateralMovement;
        
        // Apply movement
        transform.Translate(totalMovement, Space.World);
        
        // Look in the general direction of movement
        if (totalMovement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(-totalMovement.normalized),
                Time.deltaTime * 5f
            );
        }
        
        // Always move forward slightly to keep up with player
        transform.Translate(Vector3.forward * moveSpeed * 0.5f * Time.deltaTime);
    }
    
    private void HandleDirectionChanges()
    {
        directionChangeTimer -= Time.deltaTime;
        
        if (directionChangeTimer <= 0)
        {
            ChangeDirection();
            directionChangeTimer = directionChangeInterval;
        }
    }
    
    private void ChangeDirection()
    {
        // Generate a random direction in the XY plane
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        
        currentDirection = new Vector3(randomX, randomY, 0).normalized;
    }
    
    private void ClampPosition()
    {
        if (player == null)
            return;
            
        // Get GameManager reference
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
            return;
            
        // Get current position
        Vector3 position = transform.position;
        
        // Calculate bounds using GameManager's spawn area
        float minX = -gameManager.spawnAreaTotalWidth / 2f;
        float maxX = gameManager.spawnAreaTotalWidth / 2f;
        float minY = -gameManager.spawnAreaTotalHeight / 2f;
        float maxY = gameManager.spawnAreaTotalHeight / 2f;
        
        // Clamp position
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        
        // Apply clamped position
        transform.position = position;
    }
}