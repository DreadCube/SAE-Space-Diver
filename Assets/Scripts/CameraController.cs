using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private Rigidbody targetRigidbody;

    private float forwardOffset = 50f;
    private float upOffset = 8f;


    private void Awake()
    {
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        if (!targetRigidbody)
        {
            return;
        }

        /**
         * Our Camera position is based on the position of the target.
         * We use a default forward offfset and the sqrtMagnitute of the target as an
         * additional "velocity" offset.
         */
        float sqrtMagnitude = targetRigidbody.velocity.sqrMagnitude;
        float velocityOffset = Mathf.Clamp(sqrtMagnitude / 500, 1f, 20f);
        transform.position = target.position - (target.forward * (forwardOffset + velocityOffset));

        // Setting the up Offset
        transform.position = new Vector3(transform.position.x, upOffset, transform.position.z);

        // Look at the target
        transform.LookAt(target);
    }
}