// Assets/CameraController.cs
using UnityEngine;

/// <summary>
/// Controls the camera to follow the player from behind in a Starfox-style shooter.
/// Maintains a fixed offset from the player and looks at the player's forward direction.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Reference to the player transform to follow.
    /// </summary>
    public Transform player;
    
    /// <summary>
    /// Reference to the PlayerController component.
    /// </summary>
    private PlayerController playerController;

    /// <summary>
    /// Offset from the player to position the camera.
    /// </summary>
    public Vector3 offset = new Vector3(0, 2, -10);
    
    /// <summary>
    /// Offset when player is at slow speed.
    /// </summary>
    public Vector3 closeOffset = new Vector3(0, 1.5f, -6);
    
    /// <summary>
    /// Offset when player is at boost speed.
    /// </summary>
    public Vector3 farOffset = new Vector3(0, 3, -15);
    
    /// <summary>
    /// How smoothly the camera follows the player (lower = smoother).
    /// </summary>
    public float smoothSpeed = 0.05f;

    void Start()
    {
        // Get the PlayerController component
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    void LateUpdate()
    {
        // Follow the player after all movement is done.
        FollowPlayer();
    }

    /// <summary>
    /// Updates the camera's position to follow the player from behind.
    /// Uses player's position and rotation to calculate the camera position.
    /// </summary>
    void FollowPlayer()
    {
        if (player == null)
            return;
        
        // Determine the current offset based on player speed
        Vector3 currentOffset = offset;
        
        if (playerController != null)
        {
            float speedRatio = playerController.GetSpeedRatio();
            // Lerp between close and far offset based on speed ratio
            currentOffset = Vector3.Lerp(closeOffset, farOffset, speedRatio);
        }
        
        // Calculate desired position in world space
        Vector3 desiredPosition = player.position + player.rotation * currentOffset;
        
        // Smoothly move camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Look at a point in front of the player
        Vector3 lookAtPoint = player.position + player.forward * 10f;
        Quaternion desiredRotation = Quaternion.LookRotation(lookAtPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
    }
}