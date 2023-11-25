
using UnityEngine;

public class Bullet : ShapeMonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField]
    private float maxLiveTime = 1f;

    private float startTime;

    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        startTime = Time.timeSinceLevelLoad;
    }

    private void FixedUpdate()
    {
        bulletRigidbody.AddRelativeForce(Vector3.forward * 50f, ForceMode.VelocityChange);

        /**
         * We destroy the Bullet if the max live time is reached.
         * TODO: Supi fragen. FixedUpdate oder normaler Update?
         */
        if (Time.timeSinceLevelLoad - startTime > maxLiveTime)
        {
            Destroy(gameObject);
        }
    }
}
