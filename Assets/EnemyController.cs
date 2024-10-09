using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionTimeout = 1f;
    public float returnSpeed = 1f;
    public Vector2 startPosition;

    private bool isDetected = false;
    private Transform player;
    private float lastDetectionTime;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (isDetected)
        {
            ChasePlayer();
        }
        else if (Time.time - lastDetectionTime < detectionTimeout)
        {
            SlowDown();
        }
        else
        {
            ReturnToStart();
        }
    }

    public void SetDetected(bool detected, Transform playerTransform)
    {
        isDetected = detected;
        if (detected)
        {
            player = playerTransform;
            lastDetectionTime = Time.time;
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void SlowDown()
    {
        // Gradually slow down when losing detection
        moveSpeed = Mathf.Lerp(moveSpeed, 0, Time.deltaTime);
    }

    private void ReturnToStart()
    {
        // Move back to the starting position
        transform.position = Vector2.MoveTowards(transform.position, startPosition, returnSpeed * Time.deltaTime);
    }
}