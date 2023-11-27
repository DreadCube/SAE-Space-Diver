using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPrefab;

    [SerializeField]
    private float worldDimension = 1000f;

    private float wallThickness = 5f;
    private float wallHeight = 100f;

    /**
     * Creates our "walls" based on the world Dimension
     */
    void Start()
    {
        GameObject wall = Instantiate(wallPrefab, transform);
        wall.transform.position = new Vector3(-worldDimension, 0, 0);
        wall.transform.localScale = new Vector3(wallThickness, wallHeight, worldDimension * 2);

        GameObject wall2 = Instantiate(wallPrefab, transform);
        wall2.transform.position = new Vector3(0, 0, -worldDimension);
        wall2.transform.localScale = new Vector3(worldDimension * 2, wallHeight, wallThickness);

        GameObject wall3 = Instantiate(wallPrefab, transform);
        wall3.transform.position = new Vector3(worldDimension, 0, 0);
        wall3.transform.localScale = new Vector3(wallThickness, wallHeight, worldDimension * 2);

        GameObject wall4 = Instantiate(wallPrefab, transform);
        wall4.transform.position = new Vector3(0, 0, worldDimension);
        wall4.transform.localScale = new Vector3(worldDimension * 2, wallHeight, wallThickness);
    }
}
