using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
        transform.position += movement * moveSpeed * Time.deltaTime;

            // Get the mouse position in world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Ensure the mouse position is in the same plane as the object (2D or top-down view)
            mousePosition.z = transform.position.z;  // Adjust z-position based on the object’s z-level (important for perspective cameras)

            // Calculate the direction from the object to the mouse
            Vector2 direction = mousePosition - transform.position;

            // Apply the rotation to the child object to face the mouse
            transform.GetChild(0).rotation = Quaternion.LookRotation(
                Vector3.forward,
                mousePosition - transform.position
            );

    }
}