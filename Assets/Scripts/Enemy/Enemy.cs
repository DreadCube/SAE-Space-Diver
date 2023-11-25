using UnityEngine;

public class Enemy : ShapeMonoBehaviour
{

    private GameObject followTarget;

    private Color materialColor;

    private void Awake()
    {
        followTarget = GameObject.Find("Ship");
    }


    private void Start()
    {
        shapeRenderer.material.EnableKeyword("_EMISSION");
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


    void FixedUpdate()
    {
        if (!followTarget)
        {
            return;
        }

        float delta = 30f * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, followTarget.transform.position, delta);
    }


    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
