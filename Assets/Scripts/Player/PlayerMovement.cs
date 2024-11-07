using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovments : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private Transform mainCamera;

    private Vector3 direction;
    private CharacterController characterController;
    private GameManager gameManager;

    private float rotationSpeed;
    private float rotationTime = 0.1f;
    private float gravity = 30f;
    private float jumpSpeed = 24f;
    private float vecticalMovement = 0f;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        BuildSurfaceMovement();
        BuildVerticalMovement();

        characterController.Move(direction);
    }

    private void BuildSurfaceMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (gameManager.IsGamepadPresent())
        {
            horizontal = Input.GetAxisRaw("Horizontal Gamepad");
            vertical = Input.GetAxisRaw("Vertical Gamepad");
        }

        direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 1.0f) direction = direction.normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, rotationTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            float tempSpeed = speed;
            if (!characterController.isGrounded) tempSpeed /= 2;

            Vector3 directionWithCamera = (Quaternion.Euler(0f, angle, 0f) * Vector3.forward).normalized;
            float originalMovementMagnitude = direction.magnitude;
            direction.x = directionWithCamera.x * tempSpeed * originalMovementMagnitude * Time.deltaTime;
            direction.z = directionWithCamera.z * tempSpeed * originalMovementMagnitude * Time.deltaTime;
        }
        else
        {
            direction = Vector3.zero;
        }
    }

    private void BuildVerticalMovement()
    {
        if (!characterController.isGrounded)
            vecticalMovement -= gravity * Time.deltaTime;

        bool hasJump = Input.GetButtonDown("Jump");
        if (gameManager.IsGamepadPresent())
            hasJump = Input.GetButtonDown("Jump Gamepad");

        if (hasJump)
        {
            if (characterController.isGrounded)
                vecticalMovement = jumpSpeed;
        }

        direction.y = vecticalMovement * Time.deltaTime;
    }
}