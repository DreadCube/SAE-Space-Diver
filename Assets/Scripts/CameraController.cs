using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        // Set the rotation of camera to default
        transform.rotation = Quaternion.identity;

        // Look at our target
        transform.LookAt(target.transform);
    }
}
