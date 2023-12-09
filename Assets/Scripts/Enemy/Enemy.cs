using UnityEngine;

public class Enemy : ShapeMonoBehaviour
{
    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    private int dropAmount = 5;

    [SerializeField]
    private AudioClip explosionSfx;

    [SerializeField]
    private GameObject deathParticlesPrefab;

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
        Destroy(gameObject);

        // We drop Ammo
        PickupItemsSpawnManager.Instance.SpawnAroundPosition(transform.position, shape, dropAmount);

        AudioManager.Instance.PlaySfx(explosionSfx);

        // Create Death Particle Effect
        GameObject particles = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        particles.GetComponent<DeathParticles>().Init(shape.Color, transform.localScale.y / 2);
    }

    /**
     * A spawned Enemy will always follow the spaceship
     */
    private void FixedUpdate()
    {
        if (!shipTransform)
        {
            return;
        }
        float delta = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, shipTransform.position, delta);

        transform.LookAt(shipTransform);
    }
}
