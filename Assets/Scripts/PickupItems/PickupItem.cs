using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Shape shape;

    [SerializeField]
    private int rotationSpeed = 30;

    [SerializeField]
    private float vacuumThreshold = 400f;

    [SerializeField]
    private float vacuumSpeed = 120f;

    private Transform target;

    /**
     * The Instantiate of PickupItem creates the child game Object
     * based on the shape, sets proper color / scale and the correct collider
     * for our needs.
     */
    public void Instantiate(Shape shape)
    {
        GameObject child = Instantiate(shape.GameObject, transform);

        Renderer renderer = child.GetComponent<Renderer>();

        renderer.material.SetColor("_Color", shape.Color);

        if (shape.Type == Shape.ShapeType.Cube)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
        else
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
        }

        this.shape = shape;
    }

    public Shape GetShape() => shape;

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
