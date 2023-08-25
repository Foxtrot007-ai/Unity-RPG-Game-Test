using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;
    int state;
    int can;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        state = Animator.StringToHash("State");
        can = Animator.StringToHash("CanUseMove");
    }
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, int specificButton, bool usingMoveStarted)
    {
        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
        animator.SetInteger(state, specificButton);
        animator.SetBool(can, usingMoveStarted);
    }
}
