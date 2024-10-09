using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public enum EnemyType { Normal, Dash, DashWithCooldown } // Three enemy types
    public EnemyType enemyType; // Type of enemy this instance represents
    public GameObject player;
    public float speed = 2f; // Normal speed
    public float dashSpeed = 8f; // Dash speed
    public float dashCooldown = 3f; // Time between dashes for DashWithCooldown type
    public float detectionRadius = 5f;
    public float dashDuration = 0.5f; // How long the dash lasts
    public LayerMask obstacleMask;
    
    private Vector3 startingPosition;
    private Vector3 lastSeenPosition;
    private bool isChasing = false;
    private bool isSearching = false;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    
    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (PlayerInLineOfSight() && distance <= detectionRadius)
        {
            lastSeenPosition = player.transform.position;
            isChasing = true;
            isSearching = false;
        }
        else if (isChasing)
        {
            if (transform.position == lastSeenPosition)
            {
                isSearching = true;
                isChasing = false;
            }
        }

        if (isChasing)
        {
            if (enemyType == EnemyType.Dash || enemyType == EnemyType.DashWithCooldown)
            {
                if (enemyType == EnemyType.DashWithCooldown && cooldownTimer <= 0f)
                {
                    DashToPlayer(); // Dash if cooldown allows
                }
                else if (enemyType == EnemyType.Dash)
                {
                    DashToPlayer(); // Always dash without cooldown
                }
                else
                {
                    cooldownTimer -= Time.deltaTime;
                }
            }
            else
            {
                ChasePlayer(); // Normal chasing
            }
        }
        else if (isSearching)
        {
            SearchLastSeen();
        }
        else
        {
            ReturnToStart();
        }
    }

    bool PlayerInLineOfSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.position - transform.position).normalized, detectionRadius, obstacleMask);
        return hit.collider == null || hit.collider.gameObject == player;
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, lastSeenPosition, speed * Time.deltaTime);
    }

    void DashToPlayer()
    {
        if (!isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
            cooldownTimer = dashCooldown; // Reset cooldown
        }

        if (dashTimer > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
        }
        else
        {
            isDashing = false;
        }
    }

    void SearchLastSeen()
    {
        transform.position = Vector2.MoveTowards(transform.position, lastSeenPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, lastSeenPosition) < 0.1f)
        {
            if (PlayerInLineOfSight() && Vector2.Distance(transform.position, player.transform.position) <= detectionRadius)
            {
                isChasing = true;
            }
            else
            {
                isSearching = false;
            }
        }
    }

    void ReturnToStart()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPosition, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, startingPosition) < 0.1f)
        {
            isChasing = false;
            isSearching = false;
        }
    }
}

