using System.Collections.Generic;
using UnityEngine;
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
    }

    private void Update()
    {
        if (!bullet || hit.transform == null || hit.transform.gameObject == null)
        {
            return;
        }

        float distanceToTarget = (hit.point - bullet.transform.position).sqrMagnitude;


        if (distanceToTarget < 150f)
        {
            bulletSpeed = 5f;
        }
        else
        {
            bulletSpeed = 200f;
        }

        bullet.transform.position += (bulletMoveDirection.normalized * bulletSpeed) * Time.deltaTime;

        float percent = CalcTraveledPercent();

        CalcCameraPositionAndRotation(percent);


        if (percent >= 100)
        {
            Reset();

            foreach (IBulletCamListener listener in listeners)
            {
                listener.OnBulletCamEnd(bullet, hit);
            }
        }
    }


    private float CalcTraveledPercent()
    {
        float a = (hit.point - bulletSpawnPosition).sqrMagnitude;
        float b = (bullet.transform.position - bulletSpawnPosition).sqrMagnitude;

        float percent = (100f / a) * b;

        return percent;
    }

    private void CalcCameraPositionAndRotation(float percent)
    {
        bullet.transform.LookAt(hit.transform);

        float angleStep = (360f / 100f) * percent - 90f;
        float radius = 10f;


        float cos = Mathf.Cos(angleStep * Mathf.Deg2Rad) * radius;
        float sin = Mathf.Sin(angleStep * Mathf.Deg2Rad) * radius;

        Vector3 offset = new Vector3(sin, 0, cos);

        transform.position = bullet.transform.TransformPoint(offset);
        transform.LookAt(bullet.transform);
    }
}
