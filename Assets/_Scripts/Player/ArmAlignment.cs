using Cinemachine;
using UnityEngine;

public class ArmAlignment : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam = null;

    void LateUpdate()
    {
        AlignWithCamera();
    }

    void AlignWithCamera()
    {
        if (cam != null)
        {
            var euler = transform.localRotation.eulerAngles;
            euler.x = cam.State.RawOrientation.eulerAngles.x;
            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
