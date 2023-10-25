using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Vector3 position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = transform.position + new Vector3(transform.rotation.x * Input.GetAxis("Vertical") * -1, 0, transform.rotation.z * Input.GetAxis("Horizontal"));

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up * 1);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up * -1);
        }
    }
}
