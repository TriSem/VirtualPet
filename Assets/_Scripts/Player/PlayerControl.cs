using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] Transform neck = default;
    [SerializeField] Transform playerCamera = default;
    [SerializeField] List<CommandMapping> commands = null;
    [SerializeField] CommandHub commandHub = null;

    Vector3 velocity = default;
    Vector3 neckPosition = default;
    CharacterController characterController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        neckPosition = neck.transform.localPosition;
    }

    void Update()
    {
        ProcessInput();
    }

    void ProcessInput()
    {
        var lookDirection = playerCamera.forward;
        lookDirection.y = 0;
        transform.forward = lookDirection;
        MoveCharacter();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Crouch();
        }
        else
            neck.localPosition = neckPosition;

        foreach(var mapping in commands)
        {
            if (Input.GetKeyDown(mapping.Key))
                commandHub.PushSignal(mapping.CommandName);
        }

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

    void Crouch()
    {
        neck.transform.localPosition = neckPosition * 0.3f;
    }

    [Serializable]
    struct CommandMapping
    {
        [SerializeField] KeyCode key;
        [SerializeField] string commandName;

        public KeyCode Key => key;
        public string CommandName => commandName;
    }

}
