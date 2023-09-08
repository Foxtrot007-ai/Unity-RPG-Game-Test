using System.Collections;
using System.Collections.Generic;
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
    public bool teleportUsed = false;
    public bool usingMoveStarted = false;
    public int animationState = 0;
    public bool stateBLock = false;
    public bool stateLoco = false;

    public int bossDefeated = 0;

    public GameObject StrongAttackHitbox;
    public GameObject WirlAttackHitbox;
    public GameObject WirlEffectPoint;
    public GameObject electricSlash;
    public GameObject iceSlash;
    public Transform savedEffectPoint;

    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    private void Awake()
    {
        bossDefeated = PlayerPrefs.GetInt("BossDefeated", 0);
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
            playerControls.PlayerMovement.UseTeleport.performed += i => teleportUsed = true;
            playerControls.PlayerMovement.UseTeleport.canceled += i => teleportUsed = false;
            
           
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
            
            if(animationState == 1)
            {
                int lvl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().level;
                int crit = Random.Range(1, 100);
                int dmg = lvl * 15;
                if(crit > 70)
                {
                    dmg *= 3;
                }

                StrongAttackHitbox.GetComponent<ApplyDamage>().LookAtTarget(transform);
                StrongAttackHitbox.GetComponent<ApplyDamage>().UseIt(1f, dmg);
            }

            if (animationState == 2)
            {
                int lvl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().level;
                int dmg = lvl * 20;
                WirlAttackHitbox.GetComponent<ApplyDamage>().UseIt(1f, dmg);
                savedEffectPoint = WirlEffectPoint.transform;
                Invoke("MakeWirlEffect", 0.7f);
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

    public void MakeWirlEffect()
    {
        GameObject temp;
        if (bossDefeated == 1)
        {
            temp = Instantiate(iceSlash, savedEffectPoint.position, savedEffectPoint.rotation);
        }
        else
        {
            temp = Instantiate(electricSlash, savedEffectPoint.position, savedEffectPoint.rotation);
        }
        temp.transform.localScale = new Vector3(6, 6, 6);
        Destroy(temp, 0.3f);
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
