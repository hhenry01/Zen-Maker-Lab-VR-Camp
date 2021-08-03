using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Projectile (must be a GameObject with Rigidbody)
    public GameObject Projectile;
    public float ProjectileSpeed = 40f;
    public float despawn = 0.2f; // How long until the projectile automatically disappears

    // Combat info
    public float Health = 10;
    public Transform Barrel; // Or whatever the projectile shoots out of


    // Navigation info
    [SerializeField] private NavMeshAgent agent; // Pathfinding
    public Transform player; // The player to target
    public LayerMask Ground, Player;

    // Patrol
    private Vector3 walkPoint;
    public float walkPointRange = 5f;
    private bool walkPointSet;

    // Attack
    public float timeBetweenAttacks = 1f;
    private bool attacked;

    public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackrange;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        // Check for the player within a radius and act accordingly
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, Player);
        playerInAttackrange = Physics.CheckSphere(transform.position, attackRange, Player);
        if (!playerInSightRange && !playerInAttackrange) Patrol();
        if (playerInSightRange && !playerInAttackrange) Chase();
        if (playerInSightRange && playerInAttackrange) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) FindWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // If the random position is too close, retry
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void FindWalkPoint()
    {
        float z = Random.Range(-walkPointRange, walkPointRange);
        float x = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        // Check that the walk point is on solid ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
            walkPointSet = true;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        agent.SetDestination(transform.position); // Halt the enemy
        transform.LookAt(player);

        if (!attacked)
        {
            GameObject spawnProjectile = Instantiate(Projectile, Barrel.position, Barrel.rotation);
            spawnProjectile.GetComponent<Rigidbody>().velocity = ProjectileSpeed * transform.forward;
            attacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Calls function ResetAttack() after delay
            Destroy(spawnProjectile, despawn);
        }
    }

    private void ResetAttack()
    {
        attacked = false;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Enemy took damage: " + damage);
        Health -= damage;
        if (Health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.1f);
            Debug.Log("Enemy killed");
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}