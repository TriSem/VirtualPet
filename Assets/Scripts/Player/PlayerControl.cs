using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float turningSpeed = 20f;
    [SerializeField] Camera playerCamera = default;
    [SerializeField] Transform cursor = default;

    Vector3 velocity = Vector3.zero;
    float currentCameraPitch;
    CharacterController characterController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        TurnCharacter();
        MoveCharacter();
        cursor.transform.position = playerCamera.transform.forward + playerCamera.transform.position;
    }

    void TurnCharacter()
    {
        var scaledSpeed = turningSpeed * Time.deltaTime;
        var pitchDelta = -Input.GetAxis("Mouse Y") * scaledSpeed;
        var yawDelta = Input.GetAxis("Mouse X") * scaledSpeed;
        currentCameraPitch += pitchDelta;
        currentCameraPitch = Mathf.Clamp(currentCameraPitch, -80f, 80f);
        playerCamera.transform.localEulerAngles = Vector3.right * currentCameraPitch;
        transform.localEulerAngles += Vector3.up * yawDelta;

    }

    void MoveCharacter()
    {
        float oldY = velocity.y;
        
        var x = Input.GetAxisRaw("Horizontal") * walkSpeed * transform.right;
        var z = Input.GetAxisRaw("Vertical") * walkSpeed * transform.forward;
        velocity = x + z;

        if (characterController.isGrounded && velocity.y < 0)
            velocity.y = 0;
        else
            velocity.y = oldY + (-9.8f) * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
}
