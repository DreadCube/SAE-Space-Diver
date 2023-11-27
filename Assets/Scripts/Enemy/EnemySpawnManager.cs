using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    private Transform player;

    private float lastSpawnTime;
    private float spawnInterval = 20f;

    private int spawnAmount = 2;


    private void Awake()
    {
        player = GameObject.Find("Ship").transform;
    }

    private void Start()
    {
        lastSpawnTime = Time.timeSinceLevelLoad;
        SpawnCircular(spawnAmount);
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad - lastSpawnTime > spawnInterval)
        {
            spawnAmount = Mathf.RoundToInt(spawnAmount * 1.5f);
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
        }
    }
}
