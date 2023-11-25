using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        List<Shape> shapes = ShapeDefinition.GetShapeDefinitions();

        Shape randomShape = shapes[Random.Range(0, shapes.Count)];


        GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 300), Quaternion.identity);

        enemy.GetComponent<Enemy>().Instantiate(randomShape);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
