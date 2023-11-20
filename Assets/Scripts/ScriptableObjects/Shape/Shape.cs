using UnityEngine;

[CreateAssetMenu(fileName = "Shape", menuName = "ScriptableObjects/Shape", order = 1)]
public class Shape : ScriptableObject
{
    public enum ShapeType
    {
        Cube,
        Circle
    }

    [field: SerializeField] public ShapeType Type { get; private set; }

    [field: SerializeField] public Color Color { get; private set; }

    [field: SerializeField] public Texture2D UITexture { get; private set; }

    public float Hue
    {
        get
        {
            Color.RGBToHSV(Color, out float h, out _, out _);

            return h;
        }
    }
}