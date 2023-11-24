using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Shape shape;

    /**
     * The Instantiate of PickupItem creates the child game Object
     * based on the shape, sets proper color / scale and the correct collider / rigidbody
     * for our needs.
     */
    public void Instantiate(Shape shape)
    {
        GameObject child = Instantiate(shape.GameObject, transform);

        Renderer renderer = child.GetComponent<Renderer>();

        renderer.material.SetColor("_Color", shape.Color);

        transform.localScale = new Vector3(5, 5, 5);

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
}
