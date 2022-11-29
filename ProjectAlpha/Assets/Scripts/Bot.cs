using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bot : MonoBehaviour
{
    
    public NavMeshAgent agent;

    Transform player;
    int damage = 2;

    public LayerMask whatIsGround, whatIsPlayer;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;


    //Attacking
    public float timeBetweenAttacks = 0.5f;
    bool alreadyAttacked;
    public GameObject projectile;
    public Transform gunPoint;


    //States
    public float sightRange = 85f;
    public float attackRange = 20f;
    public bool playerInSightRange, playerInAttackRange;


    void Start(){
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update(){
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling(){
        if (!walkPointSet){
            SearchWalkPoint();
        }

        if (walkPointSet){
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f){
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint(){
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)){
            walkPointSet = true;
        }
    }

    private void ChasePlayer(){
        agent.destination = player.position;
    }

    private void AttackPlayer(){
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked){
            ///Attack code here
            RaycastHit hit;
            if(Physics.Raycast(this.gameObject.transform.position, this.gameObject.transform.forward, out hit, 900f)){
                PlayerMovement.curHealth -= damage;
                if(PlayerMovement.curHealth <= 0){
                    Debug.Log("Death!");
                    //Destroy(hit.transform.gameObject);
                }
            }
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack(){
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
