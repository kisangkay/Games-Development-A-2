using UnityEngine;
/// <summary>
/// This Code is complete and doesn't need to be modified for Assignment 2
/// </summary>
public class DrawBridge : MonoBehaviour
{
    private float offAngle = -90;
    private float onAngle = 0;
    private bool isActivated = false;
    private Quaternion startRotation;
    private Quaternion targetRotation;
    private float currentAngle;
    private float toggleSpeed = .5f;
    // Start is called before the first frame update
    void Start()
    {
        currentAngle = offAngle;
        transform.rotation = Quaternion.Euler(offAngle, 0, 0);
    }

    public void ToggleBridgeOn()
    {
        isActivated = true;
        currentAngle = onAngle;
    }

    public void ToggleBridgeOff()
    {
        isActivated = false;
        currentAngle = offAngle;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (isActivated)
        {
            targetRotation = Quaternion.Euler(currentAngle, 0, 0);
        }
        else
        {
            targetRotation = Quaternion.Euler(currentAngle, 0, 0);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * toggleSpeed);
    }
}
