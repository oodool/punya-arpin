using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float walkSpeed = 2f; // Speed at which enemy moves
    public float detectionRadius = 5f; // Radius in which enemy detects player
    public float idleTime = 2f; // Time spent idling before moving again
    public LayerMask obstacleLayer; // Layer for obstacles blocking the raycast
    private Transform player; // Reference to the player's transform
    private Vector3 initialPosition; // Starting position of the enemy
    private float idleTimer; // Timer to track idle duration
    private enum State { Idle, Walk }; // States for FSM
    private State currentState;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find player by tag
        initialPosition = transform.position; // Store the enemy's initial position
        currentState = State.Idle; // Set the initial state to Idle
        idleTimer = idleTime; // Initialize idle timer
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walk:
                WalkTowardsPlayer();
                break;
        }
    }

    void Idle()
    {
        idleTimer -= Time.deltaTime; // Decrease idle timer

        // When idle timer reaches zero, check if player is in detection radius
        if (idleTimer <= 0)
        {
            if (PlayerInLineOfSight())
            {
                // If player is within detection radius and no obstacles in the way, switch to Walk state
                currentState = State.Walk;
            }
            else
            {
                // Reset idle timer for the next idle period
                idleTimer = idleTime;
            }
        }
    }

    void WalkTowardsPlayer()
{
    if (!PlayerInLineOfSight())
    {
        currentState = State.Idle;
        idleTimer = idleTime;
        Debug.Log("Player out of sight. Switching to Idle.");
    }
    else
    {
        Debug.Log("Moving towards player."); // Debug to see if it's moving
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * walkSpeed * Time.deltaTime;
    }
}

    // Function to check if player is in line of sight using raycasting
    bool PlayerInLineOfSight()
{
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if (distanceToPlayer <= detectionRadius)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.position - transform.position).normalized, detectionRadius, obstacleLayer);
        
        if (hit.collider != null)
        {
            Debug.Log("Ray hit: " + hit.collider.gameObject.name); // This will log what the ray hits
            return false; // If it hits something, return false
        }
        else
        {
            Debug.Log("Player in sight!"); // Debug when player is in sight
            return true;
        }
    }

    return false; // Player is out of range
}


    // Optional: For visualizing detection radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
