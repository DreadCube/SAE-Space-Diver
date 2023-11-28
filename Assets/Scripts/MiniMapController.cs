using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private float upOffset = 800f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void LateUpdate()
    {
        if (!target)
        {
            return;
        }
        transform.position = target.position + (Vector3.up * upOffset);
    }
}
