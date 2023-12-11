using UnityEngine;

/**
 * The ShapeMonoBehaviour represents the base of all our Shape Game Objects:
 * Enemy, PickupItem, Bullet
 */
public class ShapeMonoBehaviour : MonoBehaviour
{
    protected Shape shape;

    protected Renderer shapeRenderer;

    protected Transform shipTransform;

    /**
     * Made with help from here:
     * https://discussions.unity.com/t/setting-emission-color-programatically/152813
     * 
     * We can set Emission and EmissionColor per code. This enables us the fancy
     * glow effect on Shape Objects.
     */
    public static void SetMaterialEmission(Material material, Color emissionColor, float glowMultiplier = 2f)
    {
        // Copied from: https://discussions.unity.com/t/setting-emission-color-programatically/152813/3
        material.EnableKeyword("_EMISSION");
        // Copied from: https://discussions.unity.com/t/setting-emission-color-programatically/152813/2
        material.SetColor("_EmissionColor", emissionColor * glowMultiplier);
    }

    /**
     * The Init creates the child game Object
     * based on the shape, sets proper emission color / scale and the correct collider
     * for our needs.
     */
    public void Init(Shape shape, bool withCollider = true, bool withTrigger = true)
    {
        this.shape = shape;

        GameObject child = Instantiate(shape.GameObject, transform);

        shapeRenderer = child.GetComponent<Renderer>();

        shapeRenderer.material.color = Color.black;

        SetMaterialEmission(shapeRenderer.material, shape.Color);

        if (!withCollider)
        {
            return;
        }

        if (shape.Type == Shape.ShapeType.Cube)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = withTrigger;
        }
        else
        {
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = withTrigger;
        }
    }

    public Shape GetShape() => shape;

    protected void Start()
    {
        shipTransform = GameObject.FindWithTag("Ship").GetComponent<Transform>();
    }
}
