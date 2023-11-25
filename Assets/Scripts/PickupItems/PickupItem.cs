using UnityEngine;

public class PickupItem : ShapeMonoBehaviour
{
    [SerializeField]
    private int rotationSpeed = 30;

    [SerializeField]
    private float vacuumThreshold = 400f;

    [SerializeField]
    private float vacuumSpeed = 120f;

    private Transform target;

    private void Awake()
    {
        target = GameObject.Find("Ship").GetComponent<Transform>();
    }

    private void Update()
    {
        /**
         * This will rotate our Pickup Item
         *
         * TODO: Find a solution for Spheres. We can't really see a rotation currently on them.
         */
        transform.Rotate((Vector3.up + Vector3.right) * rotationSpeed * Time.deltaTime);

        Vector3 difference = target.transform.position - transform.position;

        /**
         * If we come under the Threshold our PickupItem will move towards the target
         */
        if (difference.sqrMagnitude <= vacuumThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, vacuumSpeed * Time.deltaTime);
        }
    }
}
