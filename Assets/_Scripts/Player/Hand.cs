using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] float armLength = 0.63f;
    [SerializeField] Transform cam = null;

    void Update()
    {
        var ray = new Ray(cam.position, cam.forward);
        Debug.DrawRay(ray.origin, ray.direction + ray.origin);
        if(Input.GetKey(KeyCode.Mouse0))
        {
            if(Physics.Raycast(ray, out RaycastHit info, armLength, 1 << 12))
            {
                transform.position = info.point;
                transform.up = info.normal;
            }
        }
    }
}
