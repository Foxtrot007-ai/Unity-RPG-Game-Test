using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor.Animations;
using UnityEngine;

public class InputManager : MonoBehaviour
{ 

    PlayerControls playerControls;
    AnimatorManager animatorManager;
    public ApplyDamage applyDamageScript;
    
    public Vector2 movementInput;
    public Vector2 cameraInput;

    public float cameraInputX;
    public float cameraInputY;

    public bool strongAttackUsed = false;
    public bool wirlAttackUsed = false;
    public bool blockUsed = false;
    public bool usingMoveStarted = false;
    public int animationState = 0;
    public bool stateBLock = false;
    public bool stateLoco = false;

    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    private void Awake()
    {
        Cursor.visible = false;
        usingMoveStarted = false;
        animationState = 0;
        animatorManager = GetComponent<AnimatorManager>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.StrongAttack.performed += i => strongAttackUsed = true;
            playerControls.PlayerMovement.StrongAttack.canceled += i => strongAttackUsed = false;
            playerControls.PlayerMovement.WirlAttack.performed += i => wirlAttackUsed = true;
            playerControls.PlayerMovement.WirlAttack.canceled += i => wirlAttackUsed = false;
            playerControls.PlayerMovement.Block.performed += i => blockUsed = true;
            playerControls.PlayerMovement.Block.canceled += i => blockUsed = false;
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   
    }

    public void HandleAllInputs()
    {
        if (!usingMoveStarted)
        {
            animationState = 0;
            if (strongAttackUsed)
            {
                animationState = 1;
            }
            if (wirlAttackUsed)
            {
                animationState = 2;
            }
            if (blockUsed)
            {
                animationState = 3;
            }

            if (animationState != 0)
            {
                usingMoveStarted = true;
            }
        }
        else
        {
            if (animatorManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
            {
                usingMoveStarted = false;
            }
        }
      

        HandleMovementInput();
    }

   
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        animatorManager.UpdateAnimatorValues(0, moveAmount, animationState, usingMoveStarted);
    }
}
