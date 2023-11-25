using UnityEngine;

public class Enemy : ShapeMonoBehaviour
{

    private GameObject followTarget;

    private Color materialColor;

    [SerializeField]
    private float speed = 30f;

    [SerializeField]
    private int dropAmount = 5;


    private void Awake()
    {
        followTarget = GameObject.Find("Ship");
    }

    private void Start()
    {
        materialColor = shapeRenderer.material.color;
    }

    private void Update()
    {
        /**
         * Ping Pong between material color and white for the Enemy. Makes it a litle bit easier to see the difference
         * betweeen Enemy and PickupItem from Distance.
         * 
         * TODO: Maybe there are better ways. Still not really cool.
         */
        float a = Mathf.PingPong(Time.time, 1);
        shapeRenderer.material.color = a >= 0.7 ? Color.white : materialColor;
    }

    private void FixedUpdate()
    {
        if (!followTarget)
        {
            return;
        }

        float delta = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, delta);
    }

    private void OnDestroy()
    {
        PickupItemsSpawnManager.Instance.SpawnAroundPosition(transform.position, shape, dropAmount);
    }


    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
