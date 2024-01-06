using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System.Linq;

public class BulletCamManager : MonoBehaviour
{
    public static BulletCamManager Instance { get; private set; }

    private Camera bulletCamera;


    private IEnumerable<IBulletCamListener> listeners;

    private Bullet bullet;
    private float bulletSpeed = 1f;
    private Vector3 bulletSpawnPosition;
    private Vector3 bulletMoveDirection;
    private RaycastHit hit;

    private bool isBulletCamRunning = false;


    [SerializeField]
    private PostProcessVolume postProcessVolume;

    Bloom bloomEffect;

    public void Trigger(Bullet bullet, RaycastHit hit)
    {
        if (isBulletCamRunning)
        {
            return;
        }
        isBulletCamRunning = true;

        this.bullet = bullet;
        bulletSpawnPosition = bullet.transform.position;
        bulletMoveDirection = hit.transform.position - bullet.transform.position;
        this.hit = hit;

        bulletCamera.enabled = true;

        listeners = FindObjectsOfType<MonoBehaviour>().OfType<IBulletCamListener>();

        foreach (IBulletCamListener listener in listeners)
        {
            listener.OnBulletCamStart(bullet, hit);
        }
    }

    private void Reset()
    {
        bulletCamera.enabled = false;
        isBulletCamRunning = false;
        bloomEffect.intensity.value = 1f;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }

        bulletCamera = GetComponent<Camera>();
        bloomEffect = postProcessVolume.profile.GetSetting<Bloom>();
    }

    private void Update()
    {
        if (!bullet || hit.transform == null || hit.transform.gameObject == null)
        {
            return;
        }

        // The bullet speed will be drastically reduced if we are almost at the target
        float distanceToTarget = (hit.point - bullet.transform.position).sqrMagnitude;
        bulletSpeed = distanceToTarget < 150f ? 5f : 200f;

        bullet.transform.position += (bulletMoveDirection.normalized * bulletSpeed) * Time.deltaTime;

        float percent = CalcTraveledPercent();

        CalcCameraPositionAndRotation(percent);


        bloomEffect.intensity.value = (10f / 100f) * percent;

        if (percent >= 100)
        {
            Reset();

            foreach (IBulletCamListener listener in listeners)
            {
                listener.OnBulletCamEnd(bullet, hit);
            }
        }
    }

    /// <summary>
    /// Helper func that calculates how much we traveled (in percent) between
    /// the spawn position of the bullet and our hit point (Enemy)
    /// </summary>
    /// <returns>float percent</returns>
    private float CalcTraveledPercent()
    {
        float a = (hit.point - bulletSpawnPosition).sqrMagnitude;
        float b = (bullet.transform.position - bulletSpawnPosition).sqrMagnitude;

        float percent = (100f / a) * b;

        return percent;
    }

    /// <summary>
    /// Calculates the Bullet Camera position and rotation. The Camera rotates around
    /// the bullet while it flies to the target. The offset between camera and bullet
    /// gets higher with actual flying time / distance.
    /// </summary>
    /// <param name="percent">float percent</param>
    private void CalcCameraPositionAndRotation(float percent)
    {
        bullet.transform.LookAt(hit.transform);

        float angleStep = (360f / 100f) * percent - 90f;
        float radius = percent + 5f;


        float cos = Mathf.Cos(angleStep * Mathf.Deg2Rad) * radius;
        float sin = Mathf.Sin(angleStep * Mathf.Deg2Rad) * radius;

        Vector3 offset = new Vector3(sin, 0, cos);

        transform.position = bullet.transform.TransformPoint(offset);
        transform.LookAt(bullet.transform);
    }
}
