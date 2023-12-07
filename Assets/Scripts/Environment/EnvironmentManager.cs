using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    [SerializeField]
    private GameObject laserPrefab;

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
    void Start()
    {
        for (int y = minY; y <= maxY; y += step)
        {
            GameObject laser = Instantiate(laserPrefab, transform);

            laser.transform.position = new Vector3(-worldDimension, y, 0);
            laser.transform.Rotate(Quaternion.Euler(90f, 0f, 0f).eulerAngles);
            laser.transform.localScale = new Vector3(laserThickness, worldDimension - laserThickness / 2, laserThickness);

            /**
             * We don't need Colliders on each laser because the player stays always on the same y achsis.
             * So we only add a colldier for the lasers on the zero y achsis.
             */
            if (y == 0)
            {
                AddCollider(laser);
            }
        }

        for (int y = minY; y <= maxY; y += step)
        {
            GameObject laser = Instantiate(laserPrefab, transform);
            laser.transform.position = new Vector3(0, y, -worldDimension);
            laser.transform.Rotate(Quaternion.Euler(0f, 0f, 90f).eulerAngles);
            laser.transform.localScale = new Vector3(laserThickness, worldDimension - laserThickness / 2, laserThickness);

            if (y == 0)
            {
                AddCollider(laser);
            }
        }

        for (int y = minY; y <= maxY; y += step)
        {
            GameObject laser = Instantiate(laserPrefab, transform);
            laser.transform.position = new Vector3(worldDimension, y, 0);
            laser.transform.Rotate(Quaternion.Euler(90f, 0f, 0f).eulerAngles);
            laser.transform.localScale = new Vector3(laserThickness, worldDimension - laserThickness / 2, laserThickness);

            if (y == 0)
            {
                AddCollider(laser);
            }
        }

        for (int y = minY; y <= maxY; y += step)
        {
            GameObject laser = Instantiate(laserPrefab, transform);
            laser.transform.position = new Vector3(0, y, worldDimension);
            laser.transform.Rotate(Quaternion.Euler(0f, 0f, 90f).eulerAngles);
            laser.transform.localScale = new Vector3(laserThickness, worldDimension - laserThickness / 2, laserThickness);

            if (y == 0)
            {
                AddCollider(laser);
            }
        }
    }

    private void AddCollider(GameObject wall)
    {
        wall.AddComponent<CapsuleCollider>();
        wall.GetComponent<CapsuleCollider>().isTrigger = true;
    }
}
