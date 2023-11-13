using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Rigidbody targetRigidbody;

    private float forwardOffset = 50f;

    private void Awake()
    {
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        /**
         * Our Camera position is based on the position of the target.
         * We use a default Offset and the sqrtMagnitute of the target as an
         * additional "velocity" offset.
         */
        float sqrtMagnitude = targetRigidbody.velocity.sqrMagnitude;
        float velocityOffset = Mathf.Clamp(sqrtMagnitude / 500, 1f, 20f);
        transform.position = target.position - (target.forward * (forwardOffset + velocityOffset));

        // To prevent up and down jigglering we force Y position to zero.
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        // We always keep the default rotation of the camera
        transform.rotation = Quaternion.identity;


        // Look to the target
        transform.LookAt(target.transform);
    }
}