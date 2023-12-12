using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float intialPlayerSpeed = 4f;
    [SerializeField]
    private float maximumPlayerSpeed = 30f;
    [SerializeField]
    private float PlayerSpeedIncreaseRate = 0.1f;
    [SerializeField]
    private float JumpHeight = 1.0f;
    [SerializeField]
    private float initialGravityValue = -9.81f;
    [SerializeField]
    private LayerMask groundLayer;

    private float playerSpeed;
    private float gravity;
    private Vector3 movementDirection = Vector3.forward;

    private PlayerInput playerInput;
    private InputAction turnAction;
    private InputAction jumpAction;
    private InputAction slideAction;

    private CharacterController controller;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        turnAction = playerInput.actions["Turn"];
        jumpAction = playerInput.actions["Jump"];
        slideAction = playerInput.actions["Slide"];

        gravity = initialGravityValue;
    }

    private void OnEnable()
    {
        turnAction.performed += PlayerTurn;
        slideAction.performed += PlayerSlide;
        jumpAction.performed += PlayerJump;
    }

    private void OnDisable()
    {
        turnAction.performed -= PlayerTurn;
        slideAction.performed -= PlayerSlide;
        jumpAction.performed -= PlayerJump;
    }

    private void Start()
    {
        playerSpeed = initialGravityValue;
        gravity = initialGravityValue;
    }

    private void PlayerTurn(InputAction.CallbackContext context)
    {

    }

    private void PlayerSlide(InputAction.CallbackContext context)
    {

    }

    private void PlayerJump(InputAction.CallbackContext context)
    {

    }

    private void Update()
    {
        controller.Move(transform.forward * playerSpeed * Time.deltaTime);
    }

    private bool IsGrounded(float length)
    {
        Vector3 raycastOriginFirst = transform.position;
        raycastOriginFirst.y -= controller.height / 2f;
        raycastOriginFirst.y += .1f;

        Vector3 raycastOriginSecond = raycastOriginFirst;
        raycastOriginFirst -= transform.forward * .2f;
        raycastOriginSecond += transform.forward * .2f;

        Debug.DrawLine(raycastOriginFirst, Vector3.down, Color.green, 2f);
        Debug.DrawLine(raycastOriginSecond, Vector3.down, Color.red, 2f);


        if(Physics.Raycast(raycastOriginFirst, Vector3.down, out RaycastHit hit, length, groundLayer) 
            || Physics.Raycast(raycastOriginFirst, Vector3.down, out RaycastHit hit2, length, groundLayer)){
            return true;
        }
        else
        {
            return false;
        }
    }
}
