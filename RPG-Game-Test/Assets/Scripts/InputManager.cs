using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
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

    public bool attacking = false;
    public bool canAttackAgain = true;

    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    private void Awake()
    {
        Cursor.visible = false;
        attacking = false;
        canAttackAgain = true;
        animatorManager = GetComponent<AnimatorManager>();
    }
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Attack.performed += i => attacking = true;
            playerControls.PlayerMovement.Attack.canceled += i => attacking = false;
            
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();   
    }

    public void HandleAllInputs()
    {
        
        if (!animatorManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            canAttackAgain = true;
            applyDamageScript.alreadyDamaged = false;
        }
        else
        {
            if (!applyDamageScript.alreadyDamaged)
            {
                applyDamageScript.alreadyDamaged = true;
                Invoke("StrongAttack", 1f);
            }
            canAttackAgain = false;
        }
        HandleMovementInput();
    }

    public void StrongAttack()
    {
        applyDamageScript.DamageIt();
    }
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        cameraInputX = cameraInput.x;
        cameraInputY = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, attacking && canAttackAgain, canAttackAgain);
    }
}
