using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] Transform cam = null;

    void Update()
    {
        var ray = new Ray(cam.position, cam.forward);
        Debug.DrawRay(ray.origin, ray.direction + ray.origin);
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(Physics.Raycast(ray, out RaycastHit info, 5f, 1 << 12))
            {
                transform.position = info.point;
                transform.up = info.normal;
            }
        }
    }
}
