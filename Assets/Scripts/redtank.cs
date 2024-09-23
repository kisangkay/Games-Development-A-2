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

    public redtankstate curState;
    public float moveSpeed = 12.0f; 
    public float rotSpeed = 2.0f; 
    public GameObject turret; 
    public float turretRotationSpeed = 10.0f;
    protected Transform playerTransform; 
    protected Vector3 destPos; 
    protected GameObject[] pointList; 
    protected bool bDead; 
    public int health = 100; 
    public GameObject bulletPrefab; 
    public float fireRate = 1.0f; 
    private float nextFireTime = 0.0f;
    public GameObject bulletSpawnPoint; 
    public GameObject explosionPrefab;
    public LayerMask lineOfSightMask;
    public GameObject redkeyprefab;

    void Start()
    {

        curState = redtankstate.Inactive;
        // curState = redtankstate.Patrol;
        pointList = GameObject.FindGameObjectsWithTag("Waypoints");
        FindNextPoint();
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
//     public void ActiveState(bool isActivated)
// {
//     if (isActivated)
//     {
//         curState = redtankstate.Patrol;
//     }
//     else
//     {
//         curState = redtankstate.Inactive;
//     }
// }

     void UpdateInactiveState()
    {
            GetComponent<Rigidbody>().velocity = Vector3.zero; //to stop
        
    }

    void UpdateAttackState()
    {
        // Stop moving
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Fire bullets at regular intervals if the player is visible
        if (raycasttoplayer())
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
        // visibility of the player
        if (raycasttoplayer())
        {
            curState = redtankstate.Attack; 
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
            Die();
        }
    }

    void FindNextPoint()
    {
        int rndIndex = Random.Range(0, pointList.Length);
        destPos = pointList[rndIndex].transform.position;
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            curState = redtankstate.Dead;
        }
    }

    void Die()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

        gameObject.SetActive(false);
        Instantiate(redkeyprefab, transform.position, transform.rotation);
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

    // bool raycasttoplayer()
    // {
    //     Vector3 directionToPlayer = playerTransform.position - transform.position;

    //     // Check if there's a clear line of sight
    //     if (!Physics.Raycast(transform.position, directionToPlayer.normalized, directionToPlayer.magnitude, lineOfSightMask))
    //     {
    //         return true;
    //     }

    //     return false;
    // }
    bool raycasttoplayer()//looked up raycasthit to filter what raycast is hitting as raycast alone isnt enough to check
{
    Vector3 directionToPlayer = playerTransform.position - transform.position;
    RaycastHit hitInfo;
    if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hitInfo, directionToPlayer.magnitude, lineOfSightMask))
    {
        //ignore these 2
        if (hitInfo.collider.CompareTag("YellowTank") || hitInfo.collider.CompareTag("EnemyTank"))
        {
            return true;
        }
        return false;
    }
    return true;
}


    
}