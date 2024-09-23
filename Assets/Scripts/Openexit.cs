using UnityEngine;
public class OpenExit : MonoBehaviour
{
    private float offAngle = 0;
    private float onAngle = 90;
    private bool isActivated = false;
    private Quaternion targetRotation;
    private float currentAngle;
    private float toggleSpeed = .5f;
    // Start is called before the first frame update
    void Start()
    {
        currentAngle = offAngle;
        transform.rotation = Quaternion.Euler(offAngle, 0, 0);
    }
    public void ToggleExit()
    {
        isActivated = true;
        currentAngle = onAngle;
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
