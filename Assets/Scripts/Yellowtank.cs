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
        Inactive
    }
    public YellowTankState curState; 

    private List currentPath; // List of nodes representing the path
    public Transform yellowSphere; // The target (YellowSphere)
    public Transform gate3prefab;
    public GameObject[] waypointList;
    public float stopDistance = 1f; // Distance the tank stops before waypoiny
    public float stopDistancesphere = 5f; // Distance the tank stops near the sphere

    public float rotationSpeed = 2.0f;

    private NavMeshAgent nav;
    private int currentWaypointIndex = 0;

    // Start is called before the first frame update
    void Start() //getting their positon of sphere and yellowtank
{       nav = GetComponent<NavMeshAgent>();
        nav.speed = 5.0f; // Adjust speed as needed
        nav.angularSpeed = 120.0f; // Adjust angular speed for smoother turning
        nav.acceleration = 10.0f; // Adjust acceleration for smoother movement
        
    
 
    // transform.position = transform.position;
    // curState = YellowTankState.Inactive;
    curState = YellowTankState.Active;

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
        }
    }
    public void ActiveState(bool isActivated)
{
    if (isActivated)
    {
        curState = YellowTankState.Active;
    }
    else
    {
        curState = YellowTankState.Inactive;
    }
}

    // Method to activate or deactivate the tank

    void UpdateInactiveState()
    {
        nav.isStopped = true;
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
            // If the tank is within the stop distance of the current waypoint, move to the next one
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
            Debug.Log("Reached yellow");
        }
        else //keep following spere
        {
            nav.SetDestination(sphereposition);
            nav.isStopped = false;
            Debug.Log("Moving towards the YellowSphere.");
        }

    }
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

