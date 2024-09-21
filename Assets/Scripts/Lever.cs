using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Remove this!!
    public GameObject tank;

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
    }

    public void ToggleLever()
    {
        isActivated = !isActivated;
        
        if (isActivated)
        {
            leverColour = Color.green;
            // You can add code here to trigger your AI - Turn on


        } else
        {
            leverColour = Color.red;
            // You can add code here to trigger your AI - Turn off


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
