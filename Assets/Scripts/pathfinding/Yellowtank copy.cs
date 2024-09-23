// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class yellowtank2 : MonoBehaviour
// {
//     public enum YellowTankState
//     {
//         Active,
//         Inactive
//     }
//     public YellowTankState curState; 

//     private List currentPath; // List of nodes representing the path
//     public Transform yellowSphere; // The target (YellowSphere)
//     public float stopDistance = 1f; // Distance at which the tank stops near the sphere
//     // public float moveSpeed = 5f; // Speed of the tank
//     private int targetNodeIndex = 0;
//     public float rotationSpeed = 2.0f;

//     private Rigidbody rb;
//     private NavMeshAgent nav; 

//     // Start is called before the first frame update
//     void Start() //getting their positon of sphere and yellowtank
// {
//     rb = GetComponent<Rigidbody>();
//     nav = GetComponent<NavMeshAgent>(); // Initialize the NavMeshAgent
//     nav.enabled = true;
//     // navAgent.speed = moveSpeed;

//     // transform.position = transform.position;
//     curState = YellowTankState.Active;

//     // Find the YellowSphere by tag
//     GameObject yellowSphereObj = GameObject.FindGameObjectWithTag("YellowSphere");
//     if (yellowSphereObj != null)
//     {
//         yellowSphere = yellowSphereObj.transform;
//     }
//     else
//     {
//         Debug.LogError("YellowSphere not found! Please ensure there is a GameObject tagged 'YellowSphere'.");
//     }

// }


//     // Update is called once per frame
//     void Update()
//     {
//         switch (curState)
//         {
//             case YellowTankState.Inactive:
//                 UpdateInactiveState();
//                 break;
//             case YellowTankState.Active:
//                 UpdateActiveState();
//                 break;
//         }
//     }
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

//     // Method to activate or deactivate the tank

//     void UpdateInactiveState()
//     {
//         nav.isStopped = true; // stop
//         // GetComponent<Rigidbody>().velocity = Vector3.zero; //to stop
//     }

//     void UpdateActiveState()
//     {
//         MoveAlongPath();
//     }

//    void MoveAlongPath()
//     {
//         // Check if the Yellow Sphere isn assigned
//         if (yellowSphere == null)
//         {
//             Debug.LogError("YellowSphere is not assigned.");
//             return;
//         }

//         // If there's no path or we've reached the end of the current path, find a new path to the Yellow Sphere
//         if (currentPath == null || targetNodeIndex >= currentPath.Length)
//         {
//             Vector3 startPos = transform.position;
//             Vector3 targetPos = yellowSphere.position;

//             // Get start and goal nodes from GridManager
//             Node startNode = GridManager.instance.NodeFromWorldPoint(startPos);
//             Node goalNode = GridManager.instance.NodeFromWorldPoint(targetPos);
            
//             if (startNode == null || goalNode == null) {
//             Debug.LogError("Start or Goal Node is null.");
//             return;
//             }
//             else {
//                     Debug.Log("Start Node: " + startNode);
//                     Debug.Log("Goal Node: " + goalNode);
//                 }

            
//             Debug.Log("Calling FindPath with Start: " + startNode + ", Goal: " + goalNode);

//             // compute multiple paths and store them i want to pick the forced one
//             // Find path using AStar

//             // ArrayList path = AStar.FindPath(startNode, goalNode);
            

           
//         }

//         // Proceed with movement along the path
//         Node targetNode = currentPath.Get(targetNodeIndex);
//         Vector3 targetPosition = targetNode.position;

//         // Set the destination for the NavMeshAgent to the next node in the path
//         nav.SetDestination(targetPosition);

//         // Check if the tank has reached the current node
//         if (Vector3.Distance(transform.position, targetPosition) < 2f)
//         {
//             targetNodeIndex++;
//         }

//         // Stop if within the stop distance to the Yellow Sphere
//         if (Vector3.Distance(transform.position, yellowSphere.position) <= stopDistance)
//         {
//             nav.isStopped = true; // Stop the NavMeshAgent
//             Debug.Log("Reached the Yellow Sphere.");
//         }
//     }

//  // Optional: Visualize the path in the Scene view
//       void OnDrawGizmos()
//     {
//         if (currentPath == null)
//             return;

//         if (currentPath.Length > 0)
//         {
//             Gizmos.color = Color.yellow;
//             for (int i = targetNodeIndex; i < currentPath.Length - 1; i++)
//             {
//                 Node currentNode = currentPath.Get(i);
//                 Node nextNode = currentPath.Get(i + 1);
//                 if (currentNode != null && nextNode != null)
//                 {
//                     Gizmos.DrawLine(currentNode.position + Vector3.up * 0.05f, nextNode.position + Vector3.up * 0.05f);
//                 }
//             }
//         }
//     }
// }
