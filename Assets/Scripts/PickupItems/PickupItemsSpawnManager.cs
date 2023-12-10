using UnityEngine;

public class PickupItemsSpawnManager : MonoBehaviour
{
    public static PickupItemsSpawnManager Instance { get; private set; }

    [SerializeField]
    private GameObject pickupItemPrefab;

    [SerializeField]
    private int spawnMaxOffset = 10;

    /**
     * Spawns x amount of pickup Items based on the shape on provided position
     */
    public void SpawnAroundPosition(Vector3 position, Shape shape, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-1 * spawnMaxOffset, spawnMaxOffset), 0, Random.Range(-1 * spawnMaxOffset, spawnMaxOffset));
            Vector3 spawnPosition = position + offset;

            GameObject item = Instantiate(pickupItemPrefab, spawnPosition, Quaternion.identity);
            item.tag = "PickupItem";

            item.GetComponent<PickupItem>().Init(shape);
        }
    }

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
}
