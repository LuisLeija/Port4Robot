using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_M : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //attacking stuff
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;

    //states
    public float sightRange, attackRange;
    public bool playerInSight, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
    }

    private void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSight && !playerInAttackRange) Patroling();
        if (playerInSight && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSight) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {


        agent.SetDestination(transform.position);
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        Quaternion potato = Quaternion.LookRotation(dir);
        transform.rotation = potato;

        if (!alreadyAttacked)
        {
            
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        //if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    PlayerMovement player = collision.transform.root.GetComponent<PlayerMovement>();
    //    if (player != null)
    //    {
    //        player.TakeDamage(20);
    //    }
    //    Destroy(gameObject);
    //}
    //private void OnCollisionEnter(Collision collision)
    //{
    //    PlayerMovement player = collision.transform.root.GetComponent<PlayerMovement>();
    //    if (player != null)
    //    {
    //        player.TakeDamage(20);
    //    }
    //}
}
