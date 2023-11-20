using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShapeDefinition
{
    /**
     * Loads our available Shape Definitions on runtime
     *
     * TODO: Should also be potential cached
     */
    public static List<Shape> GetShapeDefinitions()
    {
        object[] definitions = Resources.LoadAll("ShapeDefinitions", typeof(Shape));

        List<Shape> shapes = new List<Shape>();

        foreach (object shapeDefinition in definitions)
        {
            shapes.Add((Shape)shapeDefinition);
        }

        return shapes;
    }
}
