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
    public float mouseSensitivity = 100f;
    private float verticalLookRotation = 0f;
     private Transform cameraTarget;
     private Rigidbody _rigidbody;
     
        public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    private bool isGrounded;
    private Vector3 velocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        // the CameraTarget by tag
        GameObject cameraTargetObj = GameObject.FindGameObjectWithTag("CameraTarget");
        cameraTarget = cameraTargetObj.transform;

        // Get Rigidbody component
        _rigidbody = GetComponent<Rigidbody>();

  


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
        isGrounded = CheckGround();

        
        if(health <= 0)
        {
            Die();
        }
        
    }
    bool CheckGround()
{
    // Perform a Raycast downwards from the player's position
    return Physics.Raycast(groundCheck.position, Vector3.down, groundDistance, groundMask);
}

    void UpdateMovement()
    {
         float moveX = Input.GetAxis("Horizontal");
    float moveZ = Input.GetAxis("Vertical");

    Vector3 move = transform.right * moveX + transform.forward * moveZ;

    // Handle sprinting
    bool isSprinting = Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Vertical") > 0);
    float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

    // Move the player
    Vector3 targetPosition = _rigidbody.position + move * currentSpeed * Time.deltaTime;
    _rigidbody.MovePosition(targetPosition);

    // Apply gravity
    _rigidbody.velocity += Vector3.up * gravity * Time.deltaTime;
        

    }

    void UpdateMouseLook()
    {
         if (cameraTarget == null)
            return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cameraTarget.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void UpdateSprint()
    {

    }

    void UpdateCrouch()
    {

    }

    void UpdateJump()
    {
         if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }

    void UpdateEnergy()
    {

    }

    void UpdateFOV()
    {

    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
        Debug.Log("Hit You for "+ damage+"health left "+health);
        if (health <= 0)
        {
            Die();
        }
    }
     void Die()
    {
        // Disable the player and to show gameover menu to scene menu
        gameObject.SetActive(false);
        // Load the leaderboard scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");


    }
}