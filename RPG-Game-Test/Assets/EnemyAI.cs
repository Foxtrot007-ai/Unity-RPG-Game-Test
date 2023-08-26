using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //animation
    /*
     * animationState -> State
     * 0            Waiting
     * 1            Running
     * 2            Attackig
     * 
    */
    public int animationState = 0; 
    public Animator animator;
    int horizontal;
    int vertical;
    int attack;

    //stats
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange;
    public bool playerInAttackRange;

    //waiting
    public GameObject spawnPoint;
    public bool returnedToSpawn;
    
    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    private void Awake()
    {
        returnedToSpawn = true;
        player = GameObject.Find("Character").transform;
        agent = GetComponent<NavMeshAgent>();

        animator.GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
        attack = Animator.StringToHash("Attack");
    }

    private void Update()
    {
        Vector3 distanceToSpawnPoint = transform.position - spawnPoint.transform.position;
        if (distanceToSpawnPoint.magnitude < 1f)
        {
            returnedToSpawn = true;
        }
        else
        {
            returnedToSpawn = false;
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            AttackPlayer();
        }else if (playerInSightRange)
        {
            ChasePlayer();
        }else if (!returnedToSpawn)
        {
            ReturningToSpawn();
        }
        else
        {
            Waiting();
        }

        SetAnimation();
    }

    private void ReturningToSpawn()
    {
        animationState = 1;
        agent.SetDestination(spawnPoint.transform.position);
        Vector3 distanceToSpawnPoint = transform.position - spawnPoint.transform.position;
        if(distanceToSpawnPoint.magnitude < 1f)
        {
            returnedToSpawn = true;
        }
    }

    private void ChasePlayer()
    {
        animationState = 1;
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {

        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            //attacking
            Debug.Log("Attack player");
            animationState = 2;
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        else
        {
            animationState = 0;
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
    private void Waiting()
    {
        animationState = 0;
        Debug.Log("Waiting");
    }

    private void SetAnimation()
    {
        if(animationState == 1)
        {
            animator.SetFloat(horizontal, 0f, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat(horizontal, 0f, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, 0f, 0.1f, Time.deltaTime);
        }
       
        if(animationState == 2)
        {
            animator.SetBool(attack, true);
        }
        else
        {
            animator.SetBool(attack, false);
        }
        
    }
}
