using System.Collections.Generic;
using UnityEngine;

public static class ShapeDefinition
{
    private static List<Shape> shapeCache;

    /**
     * Loads our available Shape Definitions on runtime.
     * Will save loaded shapes into cache for performance optimizations
     */
    public static List<Shape> GetShapeDefinitions()
    {
        if (shapeCache != null)
        {
            return shapeCache;
        }
        object[] definitions = Resources.LoadAll("ShapeDefinitions", typeof(Shape));

        List<Shape> shapes = new List<Shape>();

        foreach (object shapeDefinition in definitions)
        {
            shapes.Add((Shape)shapeDefinition);
        }

        shapeCache = shapes;

        return shapes;
    }
}
