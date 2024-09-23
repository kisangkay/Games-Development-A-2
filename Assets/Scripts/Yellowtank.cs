using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class yellowtank : MonoBehaviour
{
    public enum YellowTankState
    {
        Active,
        Inactive,
        Dead
    }
    public YellowTankState curState; 

    public Transform yellowSphere; // The target (YellowSphere)
    public Transform gate3prefab;
    public GameObject[] waypointList;
    public float movespeed = 10f;
    public float stopDistance = 1f; // Distance the tank stops before waypoiny
    public float stopDistancesphere = 5f; // Distance the tank stops near the sphere

    public float rotationSpeed = 2.0f;

    private NavMeshAgent nav;
    private int currentWaypointIndex = 0;
    protected bool bDead;
    public int health = 100;
    public GameObject explosionPrefab;
    public GameObject yellowkeyprefab;

    // Start is called before the first frame update
    void Start() //getting their positon of sphere and yellowtank
{       nav = GetComponent<NavMeshAgent>();
        nav.speed =movespeed;
        nav.angularSpeed = 120.0f; // Adjust angular speed for smoother turning
        nav.acceleration = 10.0f; // Adjust acceleration for smoother movement
        bDead = false;
        
    
 
    // transform.position = transform.position;
    // curState = YellowTankState.Inactive;
    curState = YellowTankState.Inactive;

    // Find the YellowSphere by tag
    GameObject yellowSphereObj = GameObject.FindGameObjectWithTag("YellowSphere");
    if (yellowSphereObj != null)
    {
        yellowSphere = yellowSphereObj.transform;
    }
    else
    {
        Debug.LogError("YellowSphere not found! Please ensure there is a GameObject tagged 'YellowSphere'.");
    }


}


    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case YellowTankState.Inactive:
                UpdateInactiveState();
                break;
            case YellowTankState.Active:
                UpdateActiveState();
                break;
            case YellowTankState.Dead:
                UpdateDeadState();
                break;
        }
    }
//     public void ActiveState(bool isActivated)
// {
//     if (isActivated)
//     {
//         curState = YellowTankState.Active;
//     }
//     else
//     {
//         curState = YellowTankState.Inactive;
//     }
// }

    // Method to activate or deactivate the tank

    void UpdateInactiveState()
    {
        nav.isStopped = true;
    }
    void UpdateDeadState()
    {
        if (!bDead)
        {
            bDead = true;
            Die();
        }
    }

    void UpdateActiveState()
    {
    movetootherpoints();

    }
    void movetootherpoints(){
    // nav.SetDestination(waypointList[0].transform.position);
    Vector3 startPos = transform.position;
    // Vector3 targetPos = yellowSphere.position;

    if (currentWaypointIndex < waypointList.Length)//ti first follow waypoints
    {
        Vector3 targetPos = waypointList[currentWaypointIndex].transform.position;
        nav.stoppingDistance = stopDistance;
        Debug.Log(nav.stoppingDistance);
        if (Vector3.Distance(startPos, targetPos) <= stopDistance)
        {
            currentWaypointIndex++;
            }
        else
        {
            nav.SetDestination(targetPos); //else go to next waypoint
            nav.isStopped = false;
        }
        }
    else //no next point now go to sphere
    {
        Vector3 sphereposition = yellowSphere.position;//new target
        Vector3 currentpos = transform.position;
        nav.stoppingDistance = stopDistancesphere;
        Debug.Log(nav.stoppingDistance);
        
        if (Vector3.Distance(currentpos, sphereposition) <= stopDistancesphere)
        {
            nav.isStopped = true;
        }
        else //keep following spere
        {
            nav.SetDestination(sphereposition);
            nav.isStopped = false;
        }

    }
    }
    

    public void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log("Hit Yellow for "+ damage+"health left "+health);
        if (health <= 0)
        {
            curState = YellowTankState.Dead;
        }
    }
    void Die()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }
        gameObject.SetActive(false);
        Instantiate(yellowkeyprefab, transform.position, transform.rotation);
    }

      void OnDrawGizmos()
    {
         if (waypointList != null && waypointList.Length > 0)
    {
        Gizmos.color = Color.yellow;
        
        // Draw lines between waypoints
        for (int i = 0; i < waypointList.Length - 1; i++)
        {
            if (waypointList[i] != null && waypointList[i + 1] != null)
            {
                Gizmos.DrawLine(waypointList[i].transform.position + Vector3.up * 0.5f, 
                                waypointList[i + 1].transform.position + Vector3.up * 0.5f);
            }
        }
}
    }
    }

