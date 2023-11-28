
using UnityEngine;

public class Bullet : ShapeMonoBehaviour
{
    private Rigidbody bulletRigidbody;

    private float maxLiveTime = 1f;

    [SerializeField]
    private AudioClip bulletSfx;

    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();

        AudioManager.Instance.PlaySfx(bulletSfx, gameObject);

        Destroy(gameObject, maxLiveTime);
    }

    private void FixedUpdate()
    {
        bulletRigidbody.AddRelativeForce(Vector3.forward * 1000f, ForceMode.VelocityChange);
    }
}
