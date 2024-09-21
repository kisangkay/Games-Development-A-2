using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// This Code is complete and doesn't need to be modified for Assignment 2
/// </summary>
public class BigButton : MonoBehaviour
{
    public GameObject greenTank;
    public DrawBridge bridge;
    public GameObject buttonModel;

    private Color buttonColour = Color.red;
    private bool isActivated = false;
    private Vector3 offPos = Vector3.zero;
    private Vector3 onPos = new Vector3(0, -0.25f, 0);
    private Renderer buttonRenderer;
    // Start is called before the first frame update
    void Start()
    {
        buttonRenderer = buttonModel.GetComponent<Renderer>();
        buttonRenderer.material.color = buttonColour;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos;
        if (isActivated)
        {
            targetPos = onPos;
        }
        else
        {
            targetPos = offPos;
        }
        buttonModel.transform.localPosition = Vector3.Lerp(buttonModel.transform.localPosition, targetPos, Time.deltaTime * 2);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == greenTank)
        {
            isActivated = true;
            bridge.ToggleBridgeOn();
            buttonColour = Color.green;
            buttonRenderer.material.color = buttonColour;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == greenTank)
        {
            isActivated = false;
            bridge.ToggleBridgeOff();
            buttonColour = Color.red;
            buttonRenderer.material.color = buttonColour;
        }
    }
}
