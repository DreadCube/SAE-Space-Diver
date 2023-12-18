using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField, Range(0f, 200f)]
    private float verticalForceAmount;

    [SerializeField, Range(0f, 10f)]
    private float rollTorqueAmount;

    [SerializeField, Range(0f, 10f)]
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

    private int Energy = 100;

    private float energyLossTime = 0f;

    private bool isDead = false;

    private Rigidbody rigidBody;

    [SerializeField]
    private AudioClip pickupSfx;

    [SerializeField]
    private AudioClip explosionSfx;

    [SerializeField]
    private AudioClip shootEmptySfx;

    [SerializeField]
    private GameObject deathParticles;

    private GameObject laser;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        PrepareLaser();
    }

    /**
     * PrepareLaser's job is to set the local Scale of the laser
     * to the Shoot hit distance
     */
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

        HandleEnergy();
    }

    /**
     * Handles the Energy Logic. Our Ship loses Energy over time.
     * 
     * It loses more Energy if we accelerate horizontal or vertical
     */
    private void HandleEnergy()
    {
        if (vertInput != 0f || horizInput != 0)
        {
            energyLossTime += 2 * Time.deltaTime;
        }
        else
        {
            energyLossTime += Time.deltaTime;
        }

        /*
         * One second of Energy "loss" collected.
         * We decrease the Energy amount by one
         */
        if (energyLossTime >= 1f)
        {
            energyLossTime -= 1f;
            DecreaseEnergyBy(1);
        }

        // If the Energy reaches zero, the player dies.
        if (Energy <= 0)
        {
            HandleDeath();
        }
    }

    /**
     * Decrease the Energy by provided amount
     * 
     * Energy will not go under zero.
     */
    private void DecreaseEnergyBy(int amount)
    {
        Energy = Mathf.Max(0, Energy - amount);
        GameLoopManager.Instance.DrawEnergy(Energy);
    }

    /**
     * Increase the Energy by provided amount
     * 
     * Energyl will not go over 100 (percent)
     */
    private void IncreaseEnergyBy(int amount)
    {
        Energy = Mathf.Min(100, Energy + amount);
        GameLoopManager.Instance.DrawEnergy(Energy);
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
     * Every time we are allowed to shoot by the shootInterval we 
     * shoot a Bullet (if enough ammo)
     * 
     * Also we check over a Raycast if we did hit something.
     */
    private void HandleShooting()
    {
        if (!shootInput || Time.timeSinceLevelLoad - lastTimeShooted < shootInterval)
        {
            return;
        }

        lastTimeShooted = Time.timeSinceLevelLoad;

        InventoryItem activeInventoryItem = InventoryManager.Instance.GetActiveInventoryItem();

        // Check if we have enough ammo of active Type
        if (activeInventoryItem.GetAmount() <= 0)
        {
            AudioManager.Instance.PlaySfx(shootEmptySfx);
            return;
        }

        ShootBullet();

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, shootHitDistance))
        {
            OnRaycastHit(hit);
        }
    }

    /**
     * Handles general physics of the spaceship
     */
    private void HandlePhysics()
    {
        // Acceleration forward and backwards
        rigidBody.AddForce(transform.forward * vertInput * verticalForceAmount, ForceMode.Force);

        // Roll and Pitch with Torque sideways
        Vector3 rollTorqueSum = transform.up * horizInput * rollTorqueAmount;
        rigidBody.AddTorque(rollTorqueSum, ForceMode.Acceleration);
        Vector3 pitchTorqueSum = transform.forward * horizInput * -pitchTorqueAmount;
        rigidBody.AddTorque(pitchTorqueSum, ForceMode.Acceleration);


        // If we don't have left / right input, we smoothly rotate the pitch rotation back to normal (identity)
        if (horizInput == 0f)
        {
            float lerpedAngle = Mathf.LerpAngle(transform.localEulerAngles.z, 0f, Time.fixedDeltaTime * 0.6f);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, lerpedAngle);
        }
    }

    /// <summary>
    /// Controls the Particles on the boosts.
    ///
    /// positive vertical input: activate main Boost
    /// zero vertical input: activate Main boost but with lower speed (little idle animation)
    /// left Input: activate right Boost
    /// right Input: activate left Boost
    /// </summary>
    private void HandleParticles()
    {
        if (vertInput < 0f)
        {
            boostMain.Stop();
        }
        else
        {
            boostMain.Play();

            ParticleSystem.MainModule main = boostMain.main;

            if (vertInput > 0f)
            {
                main.startSpeed = 75f;
            }
            else
            {
                main.startSpeed = 2f;
            }
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

    /**
     * If a trigger happens with a PickupItem:
     *   1. Destroy the Pickup Item
     *   2. Add the Pickup Item into the inventory
     *   
     * If a trigger happens with a Wall:
     *   1. We die. Call Death Logic.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickupItem")
        {
            PickupItem pickupItem = other.gameObject.GetComponent<PickupItem>();
            InventoryManager.Instance.AddItem(pickupItem);
            Destroy(other.gameObject);

            AudioManager.Instance.PlaySfx(pickupSfx, gameObject);

            IncreaseEnergyBy(5);
        }

        if (other.tag == "Wall")
        {
            HandleDeath();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            HandleDeath();
        }
    }

    /**
     * HandleDeath will be called if the player (ship) dies
     * 
     * 1. We spanw Death Particles
     * 2. Play proper explosion SFX
     * 3. Inform the GameLoopManager that we finished the round
     * 4. Destroy the Ship
     */
    private void HandleDeath()
    {
        /*
         * Prevents multiple HandleDeath calls trough collisions with enemies
         * at the same time
         */
        if (isDead)
        {
            return;
        }
        isDead = true;

        GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
        particles.GetComponent<DeathParticles>().Init(Color.white, 10);

        AudioManager.Instance.PlaySfx(explosionSfx);

        GameLoopManager.Instance.HandleFinish();

        Destroy(gameObject);
    }

    /**
     * Instantiates a visual Bullet and updates the inventory accordingly
     */
    private void ShootBullet()
    {
        InventoryItem activeInventoryItem = InventoryManager.Instance.GetActiveInventoryItem();

        // Decrease amount by one
        activeInventoryItem.Decrease();

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<Bullet>().Init(activeInventoryItem.GetShape(), false);

        // Redraw the UI
        // TODO: also here. if possible find a way to not redraw the whole UI.
        GameLoopManager.Instance.DrawInventoryUI();
    }

    /**
     * onHit will check if we hitted a enemy and inform the corresponding enemy
     * that he receives now damage
     */
    private void OnRaycastHit(RaycastHit hit)
    {
        if (hit.transform.gameObject.tag == "Enemy")
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            enemy.TakeDamage(InventoryManager.Instance.GetActiveInventoryItem().GetShape());
        }
    }
}
