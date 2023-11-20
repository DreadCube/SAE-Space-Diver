using System.Collections.Generic;
using UnityEngine;

public class PickupItemsSpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupItem;

    private void Awake()
    {
        List<Shape> shapes = ShapeDefinition.GetShapeDefinitions();

        for (int i = 0; i < 10; i++)
        {
            Shape randomShape = shapes[Random.Range(0, shapes.Count)];
            Spawn(randomShape);
        }
    }

    /**
     * Spawns a Pickup Item at a random position.
     * Instantiate of PickupItem will be called to render / set correct Shape
     * Informations.
     */
    private void Spawn(Shape shape)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));

        GameObject item = Instantiate(pickupItem, spawnPosition, Quaternion.identity);
        item.tag = "PickupItem";

        item.GetComponent<PickupItem>().Instantiate(shape);
    }
}
