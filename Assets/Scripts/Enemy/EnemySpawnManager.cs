using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField]
    private GameObject enemyPrefab;

    private Transform player;

    private float lastSpawnTime;
    private float spawnInterval = 20f;

    private float spawnMultiplier = 1.5f;

    private int spawnAmount = 2;

    private List<GameObject> enemies = new List<GameObject>();

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

        player = GameObject.Find("Ship").transform;
    }

    private void Start()
    {
        lastSpawnTime = Time.timeSinceLevelLoad;
        SpawnCircular(spawnAmount);
    }

    private void Update()
    {

        /**
         * We have a interval based spawning. To spawn the next enemies following
         * conditions have to be met:
         * 1. We reached the spawnInterval
         * 2. We have no enemies left. For late game
         * this makes for the player a little bit easier.
         */
        if (Time.timeSinceLevelLoad - lastSpawnTime > spawnInterval && enemies.Count == 0)
        {
            spawnAmount = Mathf.RoundToInt(spawnAmount * spawnMultiplier);
            lastSpawnTime = Time.timeSinceLevelLoad;

            SpawnCircular(spawnAmount, 1000);
        }
    }

    /**
     * This will spawn our Enemies circular around the player.
     * Based on amount of enemies to spawn and the radius in which they should
     * spawn.
     */
    private void SpawnCircular(int amount, float radius = 400)
    {
        List<InventoryItem> inventoryItems = InventoryManager.Instance.GetLowestHalfAmount();

        for (int i = 0; i < amount; i++)
        {
            float nextRad = 360 / amount * i * Mathf.Deg2Rad;

            float x = Mathf.Cos(nextRad) * radius;
            float z = Mathf.Sin(nextRad) * radius;


            Vector3 spawnPosition = player.transform.position + new Vector3(x, 0, z);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            InventoryItem inventoryItem = inventoryItems[Random.Range(0, inventoryItems.Count)];

            Shape shape = inventoryItem.GetShape();

            enemy.GetComponent<Enemy>().Instantiate(shape);

            enemies.Add(enemy);
        }
    }

    public void EnemyGotDestroyed(GameObject enemy)
    {
        enemies.Remove(enemies.Find(e => e == enemy));
    }
}
