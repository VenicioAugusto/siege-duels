using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    private Rigidbody playerRb;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    [SerializeField] 
    private float jumpForce = 2f;

    [SerializeField]
    private float speedForce = 2f;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();        
    }

    private void Update()
    {
        SwitchActionMap();
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

    public void DoJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump " + context.phase);
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }      
    }

    public void DoMove()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        Vector3 direction =  new Vector3(inputVector.x, 0, inputVector.y).normalized;

        if(direction.magnitude >= 0.1f)
        {
            //To smooth the rotarion of the player animation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            playerRb.AddForce(direction * speedForce, ForceMode.Force);
        }        
    }

    public void SwitchActionMap()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("UI");
            playerInputActions.Player.Disable();
            playerInputActions.UI.Enable();
        }
        if (Keyboard.current.yKey.wasPressedThisFrame)
        {
            playerInput.SwitchCurrentActionMap("UI");
            playerInputActions.UI.Disable();
            playerInputActions.Player.Enable();
        }
    }
}
