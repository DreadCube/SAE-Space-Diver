using UnityEngine;

public class Enemy : ShapeMonoBehaviour
{

    private GameObject followTarget;

    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    private int dropAmount = 5;

    [SerializeField]
    private AudioClip explosionSfx;


    [SerializeField]
    GameObject deathParticles;


    private void Awake()
    {
        followTarget = GameObject.Find("Ship");
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

        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particles.GetComponent<DeathParticles>().Init(shape.Color, transform.localScale.y / 2);
    }
}
