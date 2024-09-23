using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform carryposition;
    private GameObject carriedsphere;
    public float throwForce = 10f;


    // Optional: UI or visual feedback can be added here

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && carriedsphere != null)
        {
            Throwsphere();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedSphere") || other.CompareTag("YellowSphere") && carriedsphere == null)
        {
            Pickupsphere(other.gameObject);
        }

        if (other.CompareTag("Redkey") || other.CompareTag("Bluekey")|| other.CompareTag("Greenkey")
        || other.CompareTag("Yellowkey"))
        {
            Pickupkeys(other.gameObject);
        }
    }
     void Pickupkeys(GameObject obj)
{
    carriedsphere = obj;
    obj.transform.position = carryposition.position;
    obj.transform.parent = carryposition;

    Rigidbody rb = obj.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; // Disable physics while holding
    }
}

    void Pickupsphere(GameObject obj)
{
    carriedsphere = obj;
    obj.transform.position = carryposition.position;
    obj.transform.parent = carryposition;

    Rigidbody rb = obj.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; // Disable physics while holding
    }
}


    void Throwsphere()
    {
        if (carriedsphere == null)
            return;

        // Detach the object from the hold position
        carriedsphere.transform.parent = null;

        Rigidbody rb = carriedsphere.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Re-enable physics
            rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
        }

        carriedsphere = null;
    }
}