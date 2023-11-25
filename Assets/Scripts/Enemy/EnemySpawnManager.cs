using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject enemyPrefab;

    private Transform player;


    private float lastSpawnTime;
    private float spawnInterval = 30f;

    private int spawnAmount = 2;

    [SerializeField]
    private float minSpawnOffset = 100;

    [SerializeField]
    private float maxSpawnOffset = 200;


    private void Awake()
    {
        player = GameObject.Find("Ship").transform;
    }

    private void Start()
    {
        lastSpawnTime = Time.timeSinceLevelLoad;
        for (int i = 0; i < spawnAmount; i++)
        {
            Spawn();
        }
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad - lastSpawnTime > spawnInterval)
        {

            spawnAmount *= 2;
            lastSpawnTime = Time.timeSinceLevelLoad;

            for (int i = 0; i < spawnAmount; i++)
            {
                Spawn();
            }
        }
    }

    private void Spawn()
    {
        if (!player)
        {
            return;
        }
        // We spawn based on the user Inventory. We spawn that
        InventoryItem inventoryItem = InventoryManager.Instance.GetInventoryItemByLowestAmount();
        Shape shape = inventoryItem.GetShape();


        /**
         *  Will result randomly 1 or -1
         */
        int randX = Random.Range(0, 2) * 2 - 1;
        int randZ = Random.Range(0, 2) * 2 - 1;

        Vector3 spawnPosition = player.position + new Vector3
            (
                Random.Range(minSpawnOffset, maxSpawnOffset) * randX,
                0,
                Random.Range(minSpawnOffset, maxSpawnOffset) * randZ
            );


        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.GetComponent<Enemy>().Instantiate(shape);

    }
}
