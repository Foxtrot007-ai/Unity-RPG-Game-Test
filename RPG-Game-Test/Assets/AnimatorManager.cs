using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    int horizontal;
    int vertical;
    int attack;
    int canAttack;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        attack = Animator.StringToHash("IsAttacking");
        canAttack = Animator.StringToHash("CanAttackAgain");
    }
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool mouseClicked, bool can)
    {
        animator.SetFloat(horizontal, horizontalMovement, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, verticalMovement, 0.1f, Time.deltaTime);
        animator.SetBool(attack, mouseClicked);
        animator.SetBool(canAttack, can);
    }
}
