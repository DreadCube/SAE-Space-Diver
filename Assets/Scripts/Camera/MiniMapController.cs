using UnityEngine;

/**
 * The MiniMapController represents the Camera that
 * will follow the Spaceship from the top
 */
public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private float upOffset = 600f;

    private void Start()
    {
        // Rotate the camera so it looks down to the ship
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void Update()
    {
        if (!target)
        {
            return;
        }
        transform.position = target.position + (Vector3.up * upOffset);
    }
}
