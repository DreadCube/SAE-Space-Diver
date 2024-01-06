
using UnityEngine;

/**
 * A Bullet that will be shooted out from the Spaceship
 */
public class Bullet : ShapeMonoBehaviour, IBulletCamListener
{
    private Rigidbody bulletRigidbody;

    private float maxLiveTime = 1f;
    private float speed = 2000f;

    [SerializeField]
    private AudioClip bulletSfx;

    /**
     * Yes "new" keyword. Because we don't need the Start Logic
     * from inherited ShapeMonoBehaviour in case of Bullet
     */
    private new void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();

        AudioManager.Instance.PlaySfx(bulletSfx, gameObject);

        GetComponentInChildren<TrailRenderer>().material = shapeRenderer.material;
        Destroy(gameObject, maxLiveTime);
    }

    private void FixedUpdate()
    {
        bulletRigidbody.AddRelativeForce(Vector3.forward * speed, ForceMode.VelocityChange);
    }

    void IBulletCamListener.OnBulletCamStart(Bullet targetBullet, RaycastHit targetHit)
    {
        if (targetBullet == this)
        {
            maxLiveTime = float.MaxValue;
            speed = 0f;
            return;
        }
        Destroy(gameObject);
    }

    void IBulletCamListener.OnBulletCamEnd(Bullet targetBullet, RaycastHit targetHit)
    {
        if (this == null || gameObject == null)
        {
            return;
        }
        Destroy(gameObject);
    }
}
