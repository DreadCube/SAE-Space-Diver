using UnityEngine;

/**
 * A Pickup Item represents a Item (or Bullet) that the Player
 * can collect with his spaceship
 */
public class PickupItem : ShapeMonoBehaviour
{
    [SerializeField]
    private int rotationSpeed = 30;

    [SerializeField]
    private float vacuumThreshold = 400f;

    [SerializeField]
    private float moveSpeed = 120f;

    private void Update()
    {
        if (!shipTransform)
        {
            return;
        }

        // This will rotate our Pickup Item (Just that something is happening with them)
        transform.Rotate((Vector3.up + Vector3.right) * rotationSpeed * Time.deltaTime);

        Vector3 difference = shipTransform.position - transform.position;

        // If we come under the Threshold our PickupItem will move towards the Ship
        if (difference.sqrMagnitude <= vacuumThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, shipTransform.position, moveSpeed * Time.deltaTime);
        }
    }
}
