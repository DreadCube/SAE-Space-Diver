using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private void Update()
    {
        transform.rotation = Quaternion.identity;
        transform.LookAt(target.transform);
    }
}
