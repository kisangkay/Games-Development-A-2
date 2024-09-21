using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellowtank : MonoBehaviour
{
    public enum Yellowtankstate
    {
        Active,
        Inactive
    }
    public Yellowtankstate curState; 


    // Start is called before the first frame update
    void Start()
    {
          curState = Yellowtankstate.Inactive;
        
    }

    // Update is called once per frame
    void Update()
    {
          switch (curState)
        {
            case Yellowtankstate.Inactive:
                UpdateInactiveState();
                break;
            case Yellowtankstate.Active:
                UpdateActiveState();
                break;
        }
        
    }

        public void ActiveState(bool isActivated)
{
    if (isActivated)
    {
        curState = Yellowtankstate.Active;
    }
    else
    {
        curState = Yellowtankstate.Inactive;
    }
}


void UpdateInactiveState()
    {
            GetComponent<Rigidbody>().velocity = Vector3.zero; //to stop
        
    }
    void UpdateActiveState()
    {
            // GetComponent<Rigidbody>().velocity = Vector3.zero; //to stop
        
    }


}
