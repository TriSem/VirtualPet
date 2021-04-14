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
    [SerializeField] float commandCooldown = 1f;

    AudioSource audioSource = null;
    Vector3 velocity = default;
    Vector3 defaultNeckPosition = default;
    CharacterController characterController;
    float nextCommandAvailable = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        defaultNeckPosition = neck.transform.localPosition;
    }

    void Update()
    {
        ProcessMovement();
        ProcessCommandInput();
    }

    void ProcessMovement()
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
            neck.localPosition = defaultNeckPosition;

        ProcessCommandInput();
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

    void ProcessCommandInput()
    {
        float time = Time.time;
        if (time >= nextCommandAvailable)
        {
            foreach (var mapping in commands)
            {
                if (Input.GetKeyDown(mapping.Key))
                {
                    commandHub.PushSignal(mapping.CommandName);
                    nextCommandAvailable = time + commandCooldown;
                    audioSource.clip = mapping.AudioClip;
                    audioSource.Play();
                    break;
                }
            }
        }
    }

    void Crouch()
    {
        var newNeckPosition = neck.transform.localPosition;
        newNeckPosition -= newNeckPosition * 2f * Time.deltaTime;
        if (newNeckPosition.sqrMagnitude < (defaultNeckPosition * 0.3f).sqrMagnitude)
            newNeckPosition = defaultNeckPosition * 0.3f;
        neck.transform.localPosition = newNeckPosition;
    }

    [Serializable]
    struct CommandMapping
    {
        [SerializeField] KeyCode key;
        [SerializeField] string commandName;
        [SerializeField] AudioClip audioClip;

        public KeyCode Key => key;
        public string CommandName => commandName;
        public AudioClip AudioClip => audioClip;
    }
}
