using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Some helpful variables with pre-set values to use - this won't be all the variables you will need
    public float walkSpeed = 5f; // Walking backwards and crouching will be half walkSpeed
    public float sprintSpeed = 10f;
    public float sprintFOV = 75f;
    public float normalFOV = 60f;
    public float jumpHeight = 4f;
    public float gravity = -10f;
    public float maxEnergy = 100f;
    public float energySprintDrain = 20f; // per second
    public float energyRecoveryRate = 30f; // per second
    public float energyRecoveryDelay = 1f;

    public float health = 100f;
    

    void Start()
    {

    }

    void Update()
    {
        UpdateMovement();
        UpdateMouseLook();
        UpdateSprint();
        UpdateCrouch();
        UpdateJump();
        UpdateEnergy();
        UpdateFOV();

        
        if(health <= 0)
        {
            
        }
    }

    void UpdateMovement()
    {

    }

    void UpdateMouseLook()
    {

    }

    void UpdateSprint()
    {

    }

    void UpdateCrouch()
    {

    }

    void UpdateJump()
    {

    }

    void UpdateEnergy()
    {

    }

    void UpdateFOV()
    {

    }
}