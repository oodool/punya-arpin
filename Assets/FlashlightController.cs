using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D flashlight;
    public float detectionAngle = 45f;
    public float detectionRadius = 5f;
    public LayerMask enemyLayer;
    public KeyCode toggleKey = KeyCode.F;

    private bool isOn = true;

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            DetectEnemies();
        }
        
        else
        {
            ClearDetectedEnemies();
        }
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashlight.enabled = isOn;
    }

    private void DetectEnemies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        foreach (Collider2D hitCollider in hitColliders)
        {
            Vector2 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(transform.right, directionToEnemy);

            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                if (angle <= detectionAngle / 2)
                {
                    enemy.SetDetected(true, transform.parent);
                }
                else
                {
                    enemy.SetDetected(false, null);
                }
            }
        }
    }

    private void ClearDetectedEnemies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            EnemyController enemy = hitCollider.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.SetDetected(false, null);
            }
        }
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;
    //     // Vector3 direction = Quaternion.Euler(0, 0, -detectionAngle / 2) * transform.right;

    //     for (int i = 0; i <= detectionAngle; i++)
    //     {
    //         Gizmos.DrawLine(transform.position, transform.position + direction * detectionRadius);
    //         direction = Quaternion.Euler(0, 0, 1) * direction;
    //     }
    // }
}