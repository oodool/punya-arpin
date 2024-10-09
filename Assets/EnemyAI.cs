using UnityEngine;
  // <-- Add this

public class EnemyAI : MonoBehaviour
{
    public Transform player;  // Player reference
    public UnityEngine.Rendering.Universal.Light2D flashlight;  // Reference to the flashlight (2D light)
    public float chaseSpeed = 3.5f;  // Chase speed
    public float returnSpeed = 2.0f;  // Return speed to original position
    public bool isFlashlightOn = false;  // State of the flashlight
    
    private Vector3 startPosition;  // Initial position of the enemy
    private bool isChasing = false;  // Chase state
    private bool isReturning = false;  // Return state

    void Start()
    {
        startPosition = transform.position;  // Store the initial position
    }

    void Update()
    {
        // Check if the flashlight is on and if the enemy is in the light cone
        if (isFlashlightOn && IsEnemyInLightCone())
        {
            isChasing = true;
            isReturning = false;
        }
        else if (!isFlashlightOn || !IsEnemyInLightCone())
        {
            isChasing = false;
            isReturning = true;
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else if (isReturning)
        {
            ReturnToStart();
        }
    }

    // Method to check if the enemy is inside the flashlight's light cone
    bool IsEnemyInLightCone()
    {
        // Get the direction from the flashlight to the enemy
        Vector3 dirToEnemy = (transform.position - flashlight.transform.position).normalized;
        
        // Get the direction the flashlight is facing
        Vector3 flashlightDir = flashlight.transform.right;

        // Check the angle between the flashlight direction and the enemy direction
        float angleToEnemy = Vector3.Angle(flashlightDir, dirToEnemy);

        // Check if the enemy is within the flashlight's outer cone angle
        if (angleToEnemy < flashlight.pointLightOuterAngle / 2)
        {
            // Optionally, check if enemy is within a distance that matches the flashlight radius
            float distanceToEnemy = Vector3.Distance(flashlight.transform.position, transform.position);
            return distanceToEnemy <= flashlight.pointLightOuterRadius;
        }

        return false;
    }

    // Method for chasing the player
    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;
    }

    // Method for returning to the start position
    void ReturnToStart()
    {
        float distanceToStart = Vector3.Distance(transform.position, startPosition);
        if (distanceToStart > 0.1f)
        {
            Vector3 direction = (startPosition - transform.position).normalized;
            transform.position += direction * returnSpeed * Time.deltaTime;
        }
        else
        {
            isReturning = false;
        }
    }
}
