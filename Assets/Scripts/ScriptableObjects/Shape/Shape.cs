using UnityEngine;

/**
 * A ScriptableObject that represents a Shape
 * 
 * NOTE: ScriptableObject class Attributes are generally public. But i tried to find
 * a way to make the setting private over code (but still possible to set them over Inspector)
 * 
 * The Solution was to use a special form of Field-Targeted Attributes. Found that out through:
 * https://stackoverflow.com/a/72568220
 */
[CreateAssetMenu(fileName = "Shape", menuName = "ScriptableObjects/Shape", order = 1)]
public class Shape : ScriptableObject
{
    public enum ShapeType
    {
        Cube,
        Circle
    }

    /**
     * ShapeType: Cube or Circle (Sphere)
     */
    [field: SerializeField] public ShapeType Type { get; private set; }

    /**
     * The Color of the Shape. Will be used for:
     * 1. On Instances of Bullet / Enemy / PickupItem => Emission Color
     * 2. On Inventory / UI Item => imageTintColor 
     */
    [field: SerializeField] public Color Color { get; private set; }

    /**
     * UITexture: The UI item image
     * TODO: maybe refactor to sprite?
     */
    [field: SerializeField] public Texture2D UITexture { get; private set; }

    /**
     * GamObject: Represents actual visual gameObject for Bullet / Enemy / PickupItem
     */
    [field: SerializeField] public GameObject GameObject { get; private set; }


    public float Hue
    {
        get
        {
            Color.RGBToHSV(Color, out float h, out _, out _);

            return h;
        }
    }
}