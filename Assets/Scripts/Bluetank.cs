using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Bluetank : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Bluetankstate
    {
        Active,
        Inactive,
        Dead
    }
    public Transform ballposition;
    private GameObject bluesphereball;
    public Bluetankstate curState;

    public Transform bluesphere;
    public Transform bluetile;
    public GameObject[] waypointList;
    public float movespeed = 10f;
    public float stopDistance = 1f; // Distance the tank stops before waypoiny
    public float stopDistancesphere = 8f; // Distance the tank stops near the sphere
    public float zerodistance = 1f; // Distance the tank stops near the sphere

    public float rotationSpeed = 2.0f;

    private NavMeshAgent nav;
    private int currentWaypointIndex = 0;
    private int currentWaypointIndex_from_end = 3;
    protected bool bDead;
    public int health = 100;
    public GameObject explosionPrefab;
    public GameObject bluekeyprefab;
    
    

    // Start is called before the first frame update
    void Start() //getting their positon of sphere and yellowtank
    {
        bDead = false;
        nav = GetComponent<NavMeshAgent>();
        nav.speed = movespeed;
        nav.angularSpeed = 120.0f; // Adjust angular speed for smoother turning
        nav.acceleration = 10.0f; // Adjust acceleration for smoother movement
        Die();



        // transform.position = transform.position;
        // curState = Bluetankstate.Inactive;
        curState = Bluetankstate.Inactive;

        // Find the YellowSphere by tag
        GameObject blueSphereObj = GameObject.FindGameObjectWithTag("bluesphere");
        bluesphere = blueSphereObj.transform;
        GameObject bluetileobj = GameObject.FindGameObjectWithTag("BlueTiletoexplode");
        bluetile = bluetileobj.transform; //get its postion

        currentWaypointIndex_from_end = waypointList.Length - 1; //a reverse of our first array

    }


    // Update is called once per frame
    void Update()
    {
        switch (curState)
        {
            case Bluetankstate.Inactive:
                UpdateInactiveState();
                break;
            case Bluetankstate.Active:
                UpdateActiveState();
                break;
                case Bluetankstate.Dead:
                UpdateDeadState();
                break;
        }
    }
    void OnTriggerEnter(Collider other) //TO pickup the ball if triggerred
    {
        // Check if the object has the "Pickup" tag and no object is currently held
        if (other.CompareTag("bluesphere") && bluesphereball == null)
        {
            PickupObject(other.gameObject);
        }
    }
    //     public void ActiveState(bool isActivated)
    // {
    //     if (isActivated)
    //     {
    //         curState = Bluetankstate.Active;
    //     }
    //     else
    //     {
    //         curState = Bluetankstate.Inactive;
    //     }
    // }

    // Method to activate or deactivate the tank

    void UpdateInactiveState()
    {
        nav.isStopped = true;
    }

    void UpdateActiveState()
    {
        movetoblueballandexplode();

    }
    void movetoblueballandexplode()
    {
        // nav.SetDestination(waypointList[0].transform.position);
        Vector3 startPos = transform.position;

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
            Vector3 sphereposition = bluesphere.position;//new target
            Vector3 currentpos = transform.position;
            nav.stoppingDistance = stopDistancesphere;
            Debug.Log(nav.stoppingDistance);

            if (Vector3.Distance(currentpos, sphereposition) <= stopDistancesphere)
            {
                //onTrigger will run then auto pick up the blue sphere
                // nav.isStopped = true; //then pickup the sphere and go back on reverse waypoints
                gobacktobluetile();


            }
            else //keep following spere
            {
                nav.SetDestination(sphereposition);
                nav.isStopped = false;
            }
            //after collecting sphere we navigate back to starting tile which i have as bluetile in the class variables
            // But NOW WE GO REVERSE 
            void gobacktobluetile()
            {
                nav.isStopped = false;
                Vector3 position_after_pickup = transform.position;//altest postion

                if (currentWaypointIndex_from_end > 0)//ti first follow waypoints
                {
                    Vector3 targetPos = waypointList[currentWaypointIndex_from_end].transform.position;
                    nav.stoppingDistance = stopDistance;

                    if (Vector3.Distance(position_after_pickup, targetPos) <= stopDistance)
                    {
                        // If the tank is within the stop distance of the current waypoint, move to the next one
                        currentWaypointIndex_from_end--;
                    }
                    else
                    {
                        nav.SetDestination(targetPos); //else go to next waypoint
                        // nav.isStopped = false;
                    }
                }
                else
                {
                    //After finishing waypoints now we go to starting tile again

                    Vector3 bluetileposition = bluetile.position;//new target
                    Vector3 currentpostn = transform.position;
                    nav.stoppingDistance = zerodistance;//recycle the same distance as used for stopping next to a sphere

                    if (Vector3.Distance(currentpostn, bluetileposition) <= zerodistance) //chose to use 0 to stand on the tile itself
                    {
                        curState = Bluetankstate.Dead;
                        //stop and now exploding
                    }
                    else //keep following bluetile
                    {
                        nav.SetDestination(bluetileposition);
                        // nav.isStopped = false;
                    }
                
            }
            }


        }
    }
    protected void UpdateDeadState() 
    {
        // Show the dead animation with some physics effects
        if (!bDead) 
        {
            health = 0;
            bDead = true;
            nav.enabled = false;
            Die();
        }
    }

    void PickupObject(GameObject obj) //pick up the ball on the position i define on top 
    {
        bluesphereball = obj;
        obj.transform.position = ballposition.position;
        obj.transform.parent = ballposition;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics while holding
        }
    }
 
    void Die()
    {
        if (explosionPrefab)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
        }

       gameObject.SetActive(false);
    Instantiate(bluekeyprefab, transform.position, transform.rotation);
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
