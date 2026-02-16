using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private PlayerInputHandler m_input;

    [Header("Movement")]
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_rotationSpeed = 10f;

    [Header("Jump")]
    [Tooltip("Altura do pulo em metros")]
    [SerializeField] private float m_jumpHeight = 1.5f;

    [Header("Jump Assist")]
    [SerializeField] private float m_jumpBufferTime = 0.15f;
    [SerializeField] private float m_coyoteTime = 0.15f;

    private float m_jumpBufferCounter;
    private float m_coyoteCounter;


    [Header("Gravity")]
    [SerializeField]
    private float Gravity = -25f;           // mais forte que real
    [SerializeField]
    private float FallMultiplier = 2f;     // cair mais rápido
    [SerializeField]
    private float GroundedGravity = -2f;   // cola no chão

    private CharacterController m_controller;
    private Vector3 m_velocity;

    private void Awake()
    {
        m_controller = GetComponent<CharacterController>();
        m_input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJumpAndGravity();
    }

    private void HandleMovement()
    {
        Vector2 inputDir = m_input.MoveInput;

        if (inputDir.sqrMagnitude < 0.01f)
            return;

        Vector3 camForward = m_cameraTransform.forward;
        Vector3 camRight = m_cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.y + camRight * inputDir.x;

        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, m_rotationSpeed * Time.deltaTime);

        m_controller.Move(moveDir * m_moveSpeed * Time.deltaTime);
    }

    private void HandleJumpAndGravity()
    {
        bool grounded = m_controller.isGrounded;

        // Atualiza coyote time
        if (grounded)
            m_coyoteCounter = m_coyoteTime;
        else
            m_coyoteCounter -= Time.deltaTime;

        // Atualiza jump buffer
        if (m_input.JumpPressed)
            m_jumpBufferCounter = m_jumpBufferTime;
        else
            m_jumpBufferCounter -= Time.deltaTime;

        // Cola no chão
        if (grounded && m_velocity.y < 0)
            m_velocity.y = GroundedGravity;

        // Pulo com assistência
        if (m_jumpBufferCounter > 0 && m_coyoteCounter > 0)
        {
            m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * Gravity);
            m_jumpBufferCounter = 0f;
        }

        // Gravidade
        if (m_velocity.y < 0)
            m_velocity.y += Gravity * FallMultiplier * Time.deltaTime;
        else
            m_velocity.y += Gravity * Time.deltaTime;

        m_controller.Move(m_velocity * Time.deltaTime);
    }

}
