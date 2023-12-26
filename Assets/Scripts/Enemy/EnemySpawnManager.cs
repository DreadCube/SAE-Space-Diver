using UnityEngine;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour, IBulletCamListener
{
    public static EnemySpawnManager Instance { get; private set; }

    [SerializeField]
    private GameObject enemyPrefab;

    private Transform shipTransform;

    private float lastSpawnTime;
    private float roundTime;

    private float spawnInterval = 20f;

    private float spawnMultiplier = 1.5f;

    private int spawnAmount = 2;

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

    private void Start()
    {
        shipTransform = GameObject.FindWithTag("Ship").GetComponent<Transform>();
        lastSpawnTime = 0;

        SpawnRandom(spawnAmount);
    }

    private void Update()
    {
        roundTime += Time.deltaTime;

        /**
         * We have a interval based spawning. Enemies wil spawn all 20 seconds
         * The amount that will be spawned will be increased every cyclus
         */
        if (roundTime - lastSpawnTime > spawnInterval)
        {
            spawnAmount = Mathf.RoundToInt(spawnAmount * spawnMultiplier);
            lastSpawnTime = roundTime;
            spawnInterval += spawnInterval * 0.1f;

            Debug.Log("SPAWN!");
            SpawnRandom(spawnAmount);
        }
    }

    /**
     * This will spawn our Enemiues random in the world Dimension
     */
    private void SpawnRandom(int amount)
    {
        if (!shipTransform)
        {
            return;
        }
        List<InventoryItem> inventoryItems = InventoryManager.Instance.GetLowestHalfAmount();

        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition;

            do
            {
                spawnPosition = new Vector3(Random.Range(-900, 901), 0, Random.Range(-900, 901));

                /*
                * Will ensure that the spawn position of the enemy has some offset from the player
                * We regenerate a new one if not
                */
            } while ((shipTransform.position - spawnPosition).sqrMagnitude < 50000);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            InventoryItem inventoryItem = inventoryItems[Random.Range(0, inventoryItems.Count)];

            Shape shape = inventoryItem.GetShape();

            enemy.GetComponent<Enemy>().Init(shape, withTrigger: false);
        }
    }

    void IBulletCamListener.OnBulletCamStart(Bullet targetBullet, RaycastHit targetHit)
    {
        gameObject.SetActive(false);
    }

    void IBulletCamListener.OnBulletCamEnd(Bullet targetBullet, RaycastHit targetHit)
    {
        gameObject.SetActive(true);
    }
}
