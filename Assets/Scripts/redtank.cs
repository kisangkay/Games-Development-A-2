using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redtank : MonoBehaviour
{
    public enum redtankstate
    {
        Patrol,
        Inactive,
        Attack,
        Dead
    }

    public redtankstate curState; // Current state of the NPC
    public float moveSpeed = 12.0f; // Speed of the tank
    public float rotSpeed = 2.0f; // Tank rotation speed
    public GameObject turret; // Turret object
    public float turretRotationSpeed = 10.0f; // Speed at which the turret rotates
    protected Transform playerTransform; // Reference to the player's transform
    protected Vector3 destPos; // Destination position for patrolling
    protected GameObject[] pointList; // List of patrol points
    protected bool bDead; // Flag to check if the NPC is dead
    public int health = 100; // NPC health
    public GameObject explosion; // Explosion effect
    public GameObject smokeTrail; // Smoke trail effect
    public float ChaseRange = 15f; // Range to start chasing the player
    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public float fireRate = 1.0f; // Rate of fire (time between shots)
    private float nextFireTime = 0.0f; // Time when the NPC can fire next
    public GameObject bulletSpawnPoint; // Bullet spawn point
    public GameObject explosionPrefab;
    public LayerMask lineOfSightMask; // LayerMask to define what blocks line of sight

    void Start()
    {

        curState = redtankstate.Inactive;
        // curState = redtankstate.Patrol;
        bDead = false;


        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer)
        {
            playerTransform = objPlayer.transform;
        }
        else
        {
            Debug.LogError("Player not found. Please ensure there is a GameObject tagged 'Player'.");
        }
    }

    void Update()
    {
        if (health <= 0 && !bDead)
        {
            curState = redtankstate.Dead;
        }

        switch (curState)
        {
            case redtankstate.Inactive:
                UpdateInactiveState();
                break;
            case redtankstate.Patrol:
                UpdatePatrolState();
                break;
            case redtankstate.Attack:
                UpdateAttackState();
                break;
            case redtankstate.Dead:
                UpdateDeadState();
                break;
        }
    }
    public void ActiveState(bool isActivated)
{
    if (isActivated)
    {
        curState = redtankstate.Patrol;
    }
    else
    {
        curState = redtankstate.Inactive;
    }
}

     void UpdateInactiveState()
    {
            GetComponent<Rigidbody>().velocity = Vector3.zero; //to stop
        
    }

    void UpdateAttackState()
    {
        // Stop moving
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Fire bullets at regular intervals if the player is visible
        if (CanSeePlayer())
        {
            RotateTurret(playerTransform.position);
            if (Time.time > nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
        else
        {
            curState = redtankstate.Patrol; // Switch to Patrol if player not visible
        }
    }

    void Fire()
    {
        if (bulletPrefab && bulletSpawnPoint)
        {
            Vector3 spawnPosition = bulletSpawnPoint.transform.position + bulletSpawnPoint.transform.forward * 2;
            Quaternion spawnRotation = bulletSpawnPoint.transform.rotation;

            Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        }
    }

    void UpdatePatrolState()
    {
        // Check for visibility of the player
        if (CanSeePlayer())
        {
            curState = redtankstate.Attack; // Switch to Attack if player is seen
            return;
        }

        // Find next patrol point if current one is reached
        if (Vector3.Distance(transform.position, destPos) <= 2.0f)
        {
            FindNextPoint();
        }

        // Rotate towards the destination point and move forward
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed));
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * moveSpeed);
    }

    void UpdateDeadState()
    {
        if (!bDead)
        {
            bDead = true;
            Explode();
        }
    }

    void FindNextPoint()
    {
        int rndIndex = Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;
    }

    void Explode()
    {
        float rndX = Random.Range(8.0f, 12.0f);
        float rndZ = Random.Range(8.0f, 12.0f);
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10.0f, transform.position - new Vector3(rndX, 2.0f, rndZ), 45.0f, 40.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 10.0f, rndZ));
        }

        if (smokeTrail)
        {
            GameObject clone = Instantiate(smokeTrail, transform.position, transform.rotation);
            clone.transform.parent = transform;
        }
        Invoke("CreateFinalExplosionEffect", 1.4f);
        Destroy(gameObject, 1.5f);
    }

    void CreateFinalExplosionEffect()
    {
        if (explosion)
            Instantiate(explosion, transform.position, transform.rotation);
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        gameObject.SetActive(false);
    }

    void RotateTurret(Vector3 targetPosition)
    {
        if (turret == null)
        {
            Debug.LogError("Turret is not assigned!");
            return;
        }

        Vector3 direction = targetPosition - turret.transform.position;
        Quaternion turretRotation = Quaternion.LookRotation(direction);
        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, turretRotation, Time.deltaTime * turretRotationSpeed);
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;

        // Check if there's a clear line of sight
        if (!Physics.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, lineOfSightMask))
        {
            return true;
        }

        return false;
    }
}