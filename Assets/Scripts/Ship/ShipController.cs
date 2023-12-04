using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Range(0f, 200f)]
    [SerializeField]
    private float verticalForceAmount;

    [Range(0f, 10f)]
    [SerializeField]
    private float rollTorqueAmount;

    [Range(0f, 10f)]
    [SerializeField]
    private float pitchTorqueAmount;

    [SerializeField]
    private ParticleSystem boostMain;

    [SerializeField]
    private ParticleSystem boostLeft;

    [SerializeField]
    private ParticleSystem boostRight;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float shootInterval = 0.1f;

    [SerializeField]
    private float shootHitDistance = 500f;

    private float vertInput;

    private float horizInput;

    private float maxPitchAngle = 45f;

    private float lastTimeShooted;

    private bool shootInput;

    private Rigidbody rigidBody;

    [SerializeField]
    private AudioClip pickupSfx;

    [SerializeField]
    private AudioClip explosionSfx;



    private GameObject laser;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        PrepareLaser();
    }


    private void PrepareLaser()
    {
        laser = GetComponentInChildren<LineRenderer>().gameObject;
        laser.transform.localScale = new Vector3(0, 0, shootHitDistance);
    }

    private void Update()
    {
        vertInput = Input.GetAxis("Vertical");
        horizInput = Input.GetAxis("Horizontal");
        shootInput = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        HandlePhysics();
        HandleParticles();
        HandleShooting();
    }

    /**
     * Handles Shooting behaviour
     * 
     * Every time we are allowed to shoot by the shoot Interval we Shoot a Bullet.
     * Also we check over Raycast if we did hit something.
     */
    private void HandleShooting()
    {
        if (shootInput && Time.timeSinceLevelLoad - lastTimeShooted >= shootInterval)
        {
            lastTimeShooted = Time.timeSinceLevelLoad;

            InventoryItem activeInventoryItem = InventoryManager.Instance.GetActiveInventoryItem();

            // Check if we have enough ammo of active Type
            if (activeInventoryItem.GetAmount() <= 0)
            {
                return;
            }

            RaycastHit hit;

            Shoot();

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, shootHitDistance))
            {
                onHit(hit);
            }
        }
    }

    private void HandlePhysics()
    {
        // Acceleration forward
        //if (vertInput >= 0f)
        //{
        rigidBody.AddForce(transform.forward * vertInput * verticalForceAmount, ForceMode.Force);
        //}

        // Roll and Pitch with Torque sideways
        Vector3 rollTorqueSum = transform.up * horizInput * rollTorqueAmount;
        rigidBody.AddTorque(rollTorqueSum, ForceMode.Acceleration);

        Vector3 pitchTorqueSum = transform.forward * horizInput * -pitchTorqueAmount;
        rigidBody.AddTorque(pitchTorqueSum, ForceMode.Acceleration);


        // If we don't have left / right input, we smoothly rotate the pitch rotation back to normal
        if (horizInput == 0f)
        {
            float lerpedAngle = Mathf.LerpAngle(transform.localEulerAngles.z, 0f, Time.fixedDeltaTime * 0.6f);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, lerpedAngle);
        }
    }

    /// <summary>
    /// Controls the Particles on the boosts. They will be activated on their proper
    /// user input andd deactivated if theres no proper input:
    ///
    /// vertical input: activate main Boost
    /// left Input: activate right Boost
    /// right Input: activate left Boost
    /// </summary>
    private void HandleParticles()
    {
        if (vertInput <= 0f)
        {
            boostMain.Stop();
        }
        else
        {
            boostMain.Play();
        }

        if (horizInput > 0f)
        {
            boostLeft.Play();
            boostRight.Stop();
        }
        else if (horizInput < 0f)
        {
            boostLeft.Stop();
            boostRight.Play();
        }
        else
        {
            boostLeft.Stop();
            boostRight.Stop();
        }
    }


    /// <summary>
    /// We use LateUpdate for 2 things:
    /// 1. We clamp the pitch angle to defined maxPitchAngle to not overshoot the ships rotation
    /// 2. We freeze the X rotation so the ship doesn't go down with the nose if we rotate it
    /// </summary>
    private void LateUpdate()
    {
        float clampedAngleZ = ClampAngle(transform.localEulerAngles.z, -maxPitchAngle, maxPitchAngle);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, clampedAngleZ);
    }

    /// <summary>
    ///  Clamps provided angle to min / max angle
    ///  
    ///  Copied from: https://gist.github.com/johnsoncodehk/2ecb0136304d4badbb92bd0c1dbd8bae
    /// </summary>
    private float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickupItem")
        {
            PickupItem pickupItem = other.gameObject.GetComponent<PickupItem>();
            InventoryManager.Instance.AddItem(pickupItem);
            Destroy(other.gameObject);

            AudioManager.Instance.PlaySfx(pickupSfx, gameObject);
        }

        if (other.tag == "Enemy" || other.tag == "Wall")
        {
            Destroy(gameObject);

            PopupManager.Instance.ShowDeathOverlay(GameLoopManager.Instance.StopDrawRoundTime());

            AudioManager.Instance.PlaySfx(explosionSfx);
        }
    }

    private void Shoot()
    {
        InventoryItem activeInventoryItem = InventoryManager.Instance.GetActiveInventoryItem();

        // Decrease amount by one
        activeInventoryItem.Decrease();


        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().Instantiate(activeInventoryItem.GetShape(), false);

        // Redraw the UI
        // TODO: also here. if possible find a way to not redraw the whole UI.
        GameLoopManager.Instance.DrawInventoryUI();
    }

    /**
     * onHit will check if we hitted a enemy and inform the corresponding enemy
     * that he receives now damage
     */
    private void onHit(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag == "Enemy")
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            enemy.TakeDamage(InventoryManager.Instance.GetActiveInventoryItem().GetShape());
        }
    }
}
