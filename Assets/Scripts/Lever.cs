using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public enum coloroftanks
    {
        Yellow,
        Red,
        Green,
        Blue
    }
    public redtank redtank;
    // public GameObject yellowtank;
    public yellowtank yellowtank;
    // public GameObject yellowtank;
    public GameObject bluetank;
    public GameObject greentank;
    public coloroftanks leverForTank;
    
    public GameObject interactionKey;
    public GameObject leverLight;
    public Transform leverAnchor;

    private bool isActivated = false;
    private float activationDistance = 2f;
    private float toggleAngle = 25f;
    private float toggleSpeed = 5f;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private Transform playerTransform;
    private Color leverColour = Color.red;
    private Renderer leverRenderer;

    

    void Start()
    {
        try
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        } catch
        {
            print("The lever requires a character to be tagged as Player to function");
            GetComponent<Lever>().enabled = false;
        }
        leverRenderer = leverLight.GetComponent<Renderer>();
        leverRenderer.material.color = leverColour;
        startRotation = transform.rotation;

        switch (leverForTank)
        {
            case coloroftanks.Red:
                leverColour = Color.red;
                break;
            case coloroftanks.Yellow:
                leverColour = Color.yellow; 
                break;
            // case coloroftanks.Green:
            //     leverColour = Color.green;
            //     break;
            // case coloroftanks.Blue:
            //     leverColour = Color.blue;
            //     break;
        }
    }

    public void ToggleLever()
    {
        isActivated = !isActivated;//setting to deactivate
        
        if (isActivated) //if lever pulled
        {
            leverColour = Color.green; //indic is active

            switch (leverForTank)
            {
                case coloroftanks.Red:
                    redtank.GetComponent<redtank>().curState = redtank.redtankstate.Patrol;
                    break;
                case coloroftanks.Yellow:
                    yellowtank.GetComponent<yellowtank>().curState = yellowtank.YellowTankState.Active;
                    break;
                // case coloroftanks.Green:
                //     greenTank.SetActive(true); // Assuming the green tank is deactivated when not in use
                //     break;
                // case coloroftanks.Blue:
                //     blueTank.SetActive(true); // Same for blue tank
                //     break;
            }

            // You can add code here to trigger your AI - Turn on
            // redtank.GetComponent<redtank>().curState = redtank.redtankstate.Patrol;
            



        } 
        else
        {
            leverColour = Color.red; // Change color to indicate it's deactivated

            switch (leverForTank)
            {
                case coloroftanks.Red:
                    redtank.GetComponent<redtank>().curState = redtank.redtankstate.Inactive;
                    break;
                case coloroftanks.Yellow:
                    yellowtank.GetComponent<yellowtank>().curState = yellowtank.YellowTankState.Inactive;
                    break;
                // case coloroftanks.Green:
                //     greenTank.SetActive(false);
                //     break;
                // case coloroftanks.Blue:
                //     blueTank.SetActive(false);
                //     break;
            }
            // You can add code here to trigger your AI - Turn off
            // redtank.GetComponent<redtank>().curState = redtank.redtankstate.Inactive;
            yellowtank.GetComponent<yellowtank>().curState = yellowtank.YellowTankState.Inactive;

        }

        leverRenderer.material.color = leverColour;
    }

    void Update()
    {
        float dist = Vector3.Distance(playerTransform.position, transform.position);
        if (dist < activationDistance)
        {
            interactionKey.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleLever();
            }
        } else
        {
            interactionKey.SetActive(false);
        }

        if (isActivated)
        {
            targetRotation = Quaternion.Euler(toggleAngle,0,0);
        }
        else
        {
            targetRotation = Quaternion.Euler(-toggleAngle, 0, 0);
        }

        leverAnchor.transform.localRotation = Quaternion.Lerp(leverAnchor.transform.localRotation, targetRotation, Time.deltaTime * toggleSpeed);
    }
}
