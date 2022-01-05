using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{    
    private CharacterController controller;
    private PlayerInputActions playerInputActions;
    private PlayerInput playerInput;

    [SerializeField]
    private float jumpSpeed = 5f;
    private float ySpeed;
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private Transform cam;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();

    }
    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && ySpeed < 0)
        {
            ySpeed = -2f;
        }
    }

    private void FixedUpdate()
    {        
        DoMove();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Jump.performed += DoJump;
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public void DoMove()
    {

        ySpeed += Physics.gravity.y * Time.deltaTime;
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        Vector3 direction = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        float magnitude = Mathf.Clamp01(direction.magnitude) * speed;


        if (direction.magnitude > 0.1f)
        {
            //To smooth the rotarion of the player animation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        Vector3 finalDirection = direction * magnitude;
        finalDirection.y = ySpeed;
        controller.Move(finalDirection * Time.deltaTime);


    }

    public void DoJump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
        {
            if (context.performed)
            {
                ySpeed = jumpSpeed;
            }
        }
        
    }

}
