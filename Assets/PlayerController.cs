// Assets/PlayerController.cs
using UnityEngine;

/// <summary>
/// Controls the player ship's movement, shooting, and reticle in a Starfox-style shooter.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Speed at which the player moves.
    /// </summary>
    public float moveSpeed = 10f;

    /// <summary>
    /// Reference to the projectile prefab to instantiate when shooting.
    /// </summary>
    public GameObject projectilePrefab;

    /// <summary>
    /// The position from which projectiles are fired.
    /// </summary>
    public Transform firePoint;

    /// <summary>
    /// Reference to the reticle GameObject.
    /// </summary>
    public GameObject reticle;
    
    /// <summary>
    /// Speed at which projectiles travel.
    /// </summary>
    public float projectileSpeed = 20f;
    
    /// <summary>
    /// Distance at which the reticle is positioned in front of the player.
    /// </summary>
    public float reticleDistance = 15f;
    
    /// <summary>
    /// Cooldown between shots in seconds.
    /// </summary>
    public float shootCooldown = 0.25f;
    
    /// <summary>
    /// Timer to track shooting cooldown.
    /// </summary>
    private float shootTimer = 0f;
    
    /// <summary>
    /// Normal forward speed of the player ship.
    /// </summary>
    public float forwardSpeed = 5f;
    
    /// <summary>
    /// Speed when boosting.
    /// </summary>
    public float boostSpeed = 10f;
    
    /// <summary>
    /// Speed when slowing down.
    /// </summary>
    public float slowSpeed = 2.5f;
    
    /// <summary>
    /// Current speed of the player.
    /// </summary>
    private float currentSpeed;
    
    /// <summary>
    /// Maximum amount of energy resource.
    /// </summary>
    public float maxEnergy = 100f;
    public float minEnergy = 75f;
    
    /// <summary>
    /// Current amount of energy resource.
    /// </summary>
    private float currentEnergy;
    
    /// <summary>
    /// Rate at which energy regenerates per second.
    /// </summary>
    public float energyRegenRate = 5f;
    
    /// <summary>
    /// Cost of using boost per second.
    /// </summary>
    public float boostCost = 10f;
    
    /// <summary>
    /// Cost of using brake per second.
    /// </summary>
    public float brakeCost = 10f;
    
    /// <summary>
    /// The target position for the reticle (moves with input).
    /// </summary>
    private Vector3 reticleTargetPosition;

    /// <summary>
    /// Maximum distance the reticle can move from the center in X and Y.
    /// </summary>
    public Vector2 reticleBounds = new Vector2(40f, 22f);

    /// <summary>
    /// How much the ship tilts toward the reticle (degrees).
    /// </summary>
    public float tiltAmount = 30f;

    /// <summary>
    /// Inverted by default
    /// </summary>
    public bool invertYAxis = true;

    public Vector2 shipScreenBounds = new Vector2(18f, 10f); // Max X/Y deviation for ship from center

    private Rigidbody rb;
    private bool isBoosting = false;
    private bool isSlowing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        if (reticle == null && projectilePrefab != null)
        {
            Debug.LogWarning("Reticle not assigned to PlayerController!");
        }
        reticleTargetPosition = transform.position + transform.forward * reticleDistance;
        if (reticle != null)
        {
            if (!GameObject.Find("Reticle"))
            {
                reticle = Instantiate(reticle, reticleTargetPosition, Quaternion.identity);
            }
            reticle.transform.position = reticleTargetPosition;
        }
        
        // Initialize current speed and energy
        currentSpeed = forwardSpeed;
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        HandleReticleMovement();
        HandleShipAimOnly();
        HandleShooting();
        HandleSpeedControl();
        UpdateReticle();
        RegenerateEnergy();
        if (shootTimer > 0)
            shootTimer -= Time.deltaTime;
    }

    void LateUpdate()
    {
        ClampShipPosition();
    }

    void FixedUpdate()
    {
        // Always move forward on rails at the current speed
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    /// <summary>
    /// Handles speed control based on input (boost and slow down).
    /// </summary>
    void HandleSpeedControl()
    {
        // Boost when Shift is pressed and we have enough energy
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3")) && currentEnergy >= minEnergy)
        {
            currentSpeed = boostSpeed;
            currentEnergy -= boostCost * Time.deltaTime;
            isBoosting = true;
            isSlowing = false;
        }
        // Slow down when Ctrl is pressed and we have enough energy
        else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetButton("Fire2")) && currentEnergy >= minEnergy)
        {
            currentSpeed = slowSpeed;
            currentEnergy -= brakeCost * Time.deltaTime;
            isBoosting = false;
            isSlowing = true;
        }
        // Return to normal speed when neither is pressed or out of energy
        else
        {
            currentSpeed = forwardSpeed;
            isBoosting = false;
            isSlowing = false;
        }
    }
    
    /// <summary>
    /// Regenerates energy over time when not using abilities.
    /// </summary>
    void RegenerateEnergy()
    {
        if (currentEnergy < maxEnergy && !isBoosting && !isSlowing)
        {
            currentEnergy = Mathf.Min(maxEnergy, currentEnergy + energyRegenRate * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Gets the current energy ratio (0-1).
    /// </summary>
    public float GetEnergyRatio()
    {
        return currentEnergy / maxEnergy;
    }

    /// <summary>
    /// Gets the current speed ratio (0-1) where 0 is slowest and 1 is fastest.
    /// </summary>
    public float GetSpeedRatio()
    {
        return (currentSpeed - slowSpeed) / (boostSpeed - slowSpeed);
    }

    /// <summary>
    /// Moves the reticle target position based on input.
    /// </summary>
    void HandleReticleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (invertYAxis)
            verticalInput = -verticalInput;
        // Move the reticle in a plane in front of the player
        Vector3 inputDelta = new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;
        reticleTargetPosition += transform.right * inputDelta.x + transform.up * inputDelta.y;
        // Clamp reticle within bounds relative to the center plane
        Vector3 center = transform.position + transform.forward * reticleDistance;
        Vector3 offset = reticleTargetPosition - center;
        offset.x = Mathf.Clamp(offset.x, -reticleBounds.x, reticleBounds.x);
        offset.y = Mathf.Clamp(offset.y, -reticleBounds.y, reticleBounds.y);
        offset.z = 0;
        reticleTargetPosition = center + offset;
    }

    /// <summary>
    /// Smoothly moves the ship toward the reticle's position (Starfox style).
    /// </summary>
    void HandleShipAimOnly()
    {
        // Ship stays on rails, only rotates to aim at reticle
        Vector3 aimDirection = (reticleTargetPosition - transform.position).normalized;
        // Only allow aiming within a forward-facing cone (no turning around)
        float maxAngle = 80f;
        float angle = Vector3.Angle(transform.forward, aimDirection);
        if (angle > maxAngle)
        {
            aimDirection = Vector3.Slerp(transform.forward, aimDirection, maxAngle / angle);
        }
        // Tilt for visual effect
        float tiltX = -(reticleTargetPosition.y - (transform.position.y + transform.forward.y * reticleDistance)) / reticleBounds.y * tiltAmount;
        float tiltZ = -(reticleTargetPosition.x - (transform.position.x + transform.forward.x * reticleDistance)) / reticleBounds.x * tiltAmount;
        Quaternion tilt = Quaternion.Euler(tiltX, 0, tiltZ);
        // Set rotation: look toward aimDirection, then apply tilt
        Quaternion look = Quaternion.LookRotation(aimDirection, Vector3.up);
        transform.rotation = look * tilt;
    }

    /// <summary>
    /// Handles shooting projectiles when the spacebar or gamepad A button is pressed.
    /// Instantiates projectiles from firePoint and applies forward force.
    /// </summary>
    void HandleShooting()
    {
        // Check for input and cooldown
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) && shootTimer <= 0)
        {
            // Instantiate projectile at firePoint
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Initialize projectile
            Projectile projectileComponent = projectile.GetComponent<Projectile>();
            if (projectileComponent != null)
            {
                projectileComponent.Initialize(true, (reticleTargetPosition - firePoint.position).normalized);
            }
            
            // Reset cooldown timer
            shootTimer = shootCooldown;
            
            // Destroy projectile after 5 seconds to avoid cluttering the scene
            Destroy(projectile, 5f);
        }
    }

    /// <summary>
    /// Updates the position of the reticle at a fixed distance in front of the player's ship.
    /// </summary>
    void UpdateReticle()
    {
        if (reticle != null)
        {
            reticle.transform.position = reticleTargetPosition;
            reticle.transform.rotation = Quaternion.identity;
        }
    }

    void ClampShipPosition()
    {
        Vector3 pos = transform.position;
        // Clamp X as before
        pos.x = Mathf.Clamp(pos.x, -shipScreenBounds.x, shipScreenBounds.x);
        // Clamp Y, but ensure the ship cannot go below a minimum Y (e.g., bottom of the screen)
        float minY = -shipScreenBounds.y + 1.0f; // Add a small offset to keep ship visible
        float maxY = shipScreenBounds.y;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
