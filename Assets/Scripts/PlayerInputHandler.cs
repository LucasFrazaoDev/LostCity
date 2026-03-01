using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool IsRunning { get; private set; }

    private PlayerControls m_controls;

    private void Awake()
    {
        m_controls = new PlayerControls();
    }

    private void OnEnable()
    {
        m_controls.Enable();

        // Movement
        m_controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        m_controls.Player.Move.canceled += _ => MoveInput = Vector2.zero;

        // Look (mouse ou analog)
        m_controls.Player.LookCamera.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        m_controls.Player.LookCamera.canceled += _ => LookInput = Vector2.zero;

        // Jump
        m_controls.Player.Jump.performed += _ => JumpPressed = true;

        // Run
        m_controls.Player.Run.performed += _ => IsRunning = true;
        m_controls.Player.Run.canceled += _ => IsRunning = false;
    }

    private void Update()
    {
        // Reset do pulo por frame
        // Isso permite que o sistema de jump assist (buffer e coyote) funcione corretamente
        JumpPressed = false;
    }

    private void OnDisable()
    {
        m_controls.Disable();
    }
}
