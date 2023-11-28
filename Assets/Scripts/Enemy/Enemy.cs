using UnityEngine;

public class Enemy : ShapeMonoBehaviour
{

    private GameObject followTarget;

    private Color materialColor;

    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    private int dropAmount = 5;

    [SerializeField]
    private AudioClip explosionSfx;


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

        transform.LookAt(followTarget.transform);
    }

    public void TakeDamage(Shape otherShape)
    {
        Shape shape = GetShape();
        if (otherShape == shape)
        {
            // Same Shape Type. The Enemy doesn't get damage but will increase its scale.
            transform.localScale *= 1.5f;
            return;
        }


        // If the Enemy takes Damage: We destroy it for now. Could change in future.
        PickupItemsSpawnManager.Instance.SpawnAroundPosition(transform.position, shape, dropAmount);
        Destroy(gameObject);

        AudioManager.Instance.PlaySfx(explosionSfx);
    }
}
