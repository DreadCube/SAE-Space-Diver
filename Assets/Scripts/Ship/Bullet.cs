
using UnityEngine;

/**
 * A Bullet that will be shooted out from the Spaceship
 */
public class Bullet : ShapeMonoBehaviour
{
    private Rigidbody bulletRigidbody;

    private float maxLiveTime = 1f;

    [SerializeField]
    private AudioClip bulletSfx;

    /**
     * Yes "new" keyboard. Because we don't need the Start Logic
     * from inherited ShapeMonoBehaviour in case of Bullet
     */
    private new void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();

        AudioManager.Instance.PlaySfx(bulletSfx, gameObject);

        Destroy(gameObject, maxLiveTime);
    }

    private void FixedUpdate()
    {
        bulletRigidbody.AddRelativeForce(Vector3.forward * 2000f, ForceMode.VelocityChange);
    }
}
