using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public Collider attackCollider;
    public Transform player;
    public Transform boundsChecker, wallChecker;
    public LayerMask whatIsGround, whatIsWall, whatIsPlayer;
    public float health;
    public AudioSource sfx;
    public AudioSource aiStateSounds;
    public AudioClip deathSfx, attackSfx, takeDamage, patrolSound, chaseSound;
    bool IsDead;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public Vector3 bounds;

    //Attacking
    public float timeBetweenAttacks;
    float attackTimer;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, withinLevel, hitWall, seenPlayer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        aiStateSounds.loop = true;
    }

    void Update(){
        attackTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(IsDead) {return;}

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        withinLevel = Physics.CheckBox(boundsChecker.position, bounds, Quaternion.identity, whatIsGround);
        hitWall = Physics.CheckBox(boundsChecker.position, bounds, Quaternion.identity, whatIsWall);
        
        if (Physics.Raycast(wallChecker.position, wallChecker.forward, 1f, whatIsWall))
        {
            Debug.Log("is in proximity with A Wall");
            walkPointSet = false;
            SearchWalkPoint();
        }

        Debug.DrawRay(wallChecker.position, wallChecker.forward, Color.green);

        if (!playerInSightRange && !playerInAttackRange && !seenPlayer) Patroling();
        if (playerInSightRange && !playerInAttackRange) seenPlayer = true;
        if(seenPlayer) ChasePlayer();
        if (playerInAttackRange && playerInSightRange){ 
            if(attackTimer>=timeBetweenAttacks && !IsDead)
            {
                AttackPlayer();
                attackTimer = 0;
            }
        }

        if(!withinLevel) SearchWalkPoint();
        if(hitWall) SearchWalkPoint();
    }

    private void Patroling()
    {
        animator.SetBool("inSight", false);
        if (!walkPointSet) SearchWalkPoint();
        aiStateSounds.clip = patrolSound;

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 0.5f)
            walkPointSet = false;
    }

    public void TakeDamage(float damage)
    {
        if(IsDead) {return;}
        if(health > 0)
        {
            health -= damage;
            animator.SetTrigger("TakeHit");
            sfx.PlayOneShot(takeDamage);
            seenPlayer = true;
        }
        else
            Die();
    }

    void Die()
    {
        if (!IsDead)
        {
            sfx.PlayOneShot(deathSfx);
            //Logic for the Death function
            animator.SetTrigger("Die");
            animator.SetBool("Dead", true);
            IsDead = true;
        }   
    }

    public void FinalDeath()
    {
        gameObject.SetActive(false);
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        hitWall = false;

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        animator.SetBool("inSight", true);
        aiStateSounds.clip = chaseSound;
        transform.LookAt(player);

        agent.SetDestination(player.position);

        if (!playerInSightRange) seenPlayer = false;
    }

    private void AttackPlayer()
    {
        animator.SetTrigger("attack");
        animator.SetBool("inSight", false);
        seenPlayer = false;

        agent.SetDestination(transform.position);

        transform.LookAt(player);
        if(!player.gameObject.GetComponent<ThirdPersonController>().isDead)
            Invoke("Attack", 1.15f);
    }

    void Attack()
    {
        Debug.Log("Attack!!!!!!");
        attackCollider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            sfx.PlayOneShot(attackSfx);
            other.GetComponent<PlayerStats>().TakeDamage(10f);
        }
        else
        {
            hitWall = true;
        }
    }

    public void ResetAttack()
    {
        attackCollider.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawCube(boundsChecker.position, new Vector3(bounds.x, bounds.y, bounds.z));
    }
}
