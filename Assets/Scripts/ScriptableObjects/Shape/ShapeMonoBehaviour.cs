using UnityEngine;

public class ShapeMonoBehaviour : MonoBehaviour
{
    protected Shape shape;

    protected Renderer shapeRenderer;

    /**
     * The Instantiate of PickupItem creates the child game Object
     * based on the shape, sets proper color / scale and the correct collider
     * for our needs.
     */
    public void Instantiate(Shape shape, bool withCollider = true)
    {
        GameObject child = Instantiate(shape.GameObject, transform);

        shapeRenderer = child.GetComponent<Renderer>();

        shapeRenderer.material.color = Color.black;

        /**
         * 
         * Made with help from here:
         * https://discussions.unity.com/t/setting-emission-color-programatically/152813
         * 
         * We can set Emission and EmissionColor per code. This enables us the fancy
         * glow effect on Shape Objects.
         */
        shapeRenderer.material.EnableKeyword("_EMISSION");
        shapeRenderer.material.SetColor("_EmissionColor", shape.Color * 2);


        if (shape.Type == Shape.ShapeType.Cube)
        {
            if (withCollider)
            {
                BoxCollider collider = gameObject.AddComponent<BoxCollider>();
                collider.isTrigger = true;
            }
        }
        else
        {
            if (withCollider)
            {
                SphereCollider collider = gameObject.AddComponent<SphereCollider>();
                collider.isTrigger = true;
            }
        }

        this.shape = shape;
    }

    public Shape GetShape() => shape;

}
