using UnityEngine;

/**
 * The EnvironmentManager handles and creates environment parts. In our case
 * the Laser Walls.
 */
public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    [SerializeField]
    private GameObject laserWallPrefab;

    [SerializeField]
    private float worldDimension = 1000f;

    private float laserThickness = 3f;

    private int minY = -40;
    private int maxY = 40;
    private int step = 20;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /**
     * Creates our laser walls based on the world Dimension
     */
    private void Start()
    {
        CreateLaserWall(new Vector3(-worldDimension, 0, 0));
        CreateLaserWall(new Vector3(0, 0, -worldDimension));
        CreateLaserWall(new Vector3(worldDimension, 0, 0));
        CreateLaserWall(new Vector3(0, 0, worldDimension));
    }

    /**
     * Creates a laser wall on specified position
     */
    private void CreateLaserWall(Vector3 position)
    {
        /**
         * calculates rotation based on position. Some examples:
         * 
         * position: new Vector3(-1000, 0, 0) => Euler(90, 0, 0)
         * position: new Vector3(1000, 0, 0) => Euler(90, 0, 0)
         * position: new Vector3(0, 0, -1000) => Euler(0, 0, 90)
         * position: new Vector3(0, 0, 1000) => Euler(0, 0, 90)
         */
        float rotationX = Mathf.Clamp01(Mathf.Abs(position.x)) * 90;
        float rotationZ = Mathf.Clamp01(Mathf.Abs(position.z)) * 90;
        Vector3 rotation = Quaternion.Euler(rotationX, 0, rotationZ).eulerAngles;

        for (int y = minY; y <= maxY; y += step)
        {
            GameObject laser = Instantiate(laserWallPrefab, transform);

            laser.transform.position = position + new Vector3(0, y, 0);
            laser.transform.Rotate(rotation);
            laser.transform.localScale = new Vector3(laserThickness, worldDimension - laserThickness / 2, laserThickness);

            /**
             * We don't need Colliders on each laser because the player stays always on the same y achsis.
             * So we only add a colldier for the lasers on the zero y achsis. Prevents unnecessary colliders...
             */
            if (y == 0)
            {
                AddCollider(laser);
            }
        }
    }

    /**
     * Adds a collider to the specified laser
     */
    private void AddCollider(GameObject laser)
    {
        laser.AddComponent<CapsuleCollider>();
        laser.GetComponent<CapsuleCollider>().isTrigger = true;
    }
}
