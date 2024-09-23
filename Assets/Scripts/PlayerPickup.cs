using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPosition; // Position where the object will be held
    private GameObject heldObject; // Currently held object
    public float throwForce = 10f; // Force applied when throwing

    // Optional: UI or visual feedback can be added here

    void Update()
    {
        // Handle throwing the object
        if (Input.GetMouseButtonDown(0) && heldObject != null)
        {
            ThrowObject();
        }

        // Optional: You can add code here to rotate the held object or add other interactions
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object has the "Pickup" tag and no object is currently held
        if (other.CompareTag("RedSphere") || other.CompareTag("YellowSphere") && heldObject == null)
        {
            PickupObject(other.gameObject);
        }
    }

    void PickupObject(GameObject obj)
{
    heldObject = obj;
    obj.transform.position = holdPosition.position;
    obj.transform.parent = holdPosition;

    Rigidbody rb = obj.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; // Disable physics while holding
    }
}


    void ThrowObject()
    {
        if (heldObject == null)
            return;

        // Detach the object from the hold position
        heldObject.transform.parent = null;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Re-enable physics
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        // Optional: Reset object's rotation or apply additional effects

        heldObject = null;

        // Optional: Add visual feedback, e.g., reset object color or hide UI
    }
}