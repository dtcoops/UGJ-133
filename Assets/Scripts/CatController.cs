using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CatController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 20f;
    private float rotationSpeed = 10f;

    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;

    [Header("Sprint Settings")]
    public float sprintSpeedMultiplier = 1.8f;
    public float maxStamina = 100f;
    public float staminaDrainRate = 30f;
    public float staminaRegenRate = 15f;

    [Header("Audio")]
    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpSoundVolume = 1f;

    private Rigidbody rb;
    private Camera cam;

    private float coyoteTimer;
    private float jumpBufferTimer;
    private float stamina;
    private bool isSprinting;
    private bool isGrounded;

    public float StaminaPercent => stamina / maxStamina;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        stamina = maxStamina;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        UpdateCoyoteTimer();
        UpdateJumpBufferTimer();
        UpdateStamina();
        
        if (CanJump())
        {
            Jump();
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    #region Action Methods

    void Move()
    {
        // Get input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(h, 0f, v).normalized;

        Vector3 moveDirection = GetMoveDirection(input);
        UpdateVelocity(moveDirection);
        UpdateFacingRotation(moveDirection);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        PlayJumpSound();
    }

    void UpdateStamina()
    {
        bool wantsToSprint = Input.GetButton("Sprint") && stamina > 0f;

        if (wantsToSprint)
        {
            stamina -= staminaDrainRate * Time.deltaTime;
            isSprinting = stamina > 0f;
        }
        else
        {
            stamina += staminaRegenRate * Time.deltaTime;
            isSprinting = false;
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    void PlayJumpSound()
    {
        if (jumpSound != null)
        {
            AudioSource.PlayClipAtPoint(jumpSound, transform.position, jumpSoundVolume);
        }
    }

    #endregion

    #region Helper Functions

    Vector3 GetMoveDirection(Vector3 input)
    {
        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();
        Vector3 cameraRight = cam.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * input.z + cameraRight * input.x;
        return moveDirection;
    }

    void UpdateVelocity(Vector3 moveDirection)
    {   
        // Normal Speed or Sprint
        float currentSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;

        Vector3 targetVelocity = moveDirection * currentSpeed;
        Vector3 velChange = targetVelocity - new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        velChange = Vector3.ClampMagnitude(velChange, acceleration * Time.fixedDeltaTime);
        rb.AddForce(velChange, ForceMode.VelocityChange);
    }

    void UpdateFacingRotation(Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

    }

    bool CanJump()
    {
        return jumpBufferTimer > 0f && coyoteTimer > 0f;
    }

    void UpdateCoyoteTimer()
    {
        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        } else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    void UpdateJumpBufferTimer()
    {
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
        } else
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) 
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    #endregion
}
