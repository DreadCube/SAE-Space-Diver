
using UnityEngine;

public class Bullet : ShapeMonoBehaviour
{
    private Rigidbody bulletRigidbody;

    [SerializeField]
    private float maxLiveTime = 3f;

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


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            // The Enemy is immutable agains same shape Type
            if (enemy.GetShape() != GetShape())
            {
                other.GetComponent<Enemy>().TakeDamage();
            }

            Destroy(gameObject);
        }
    }
}
