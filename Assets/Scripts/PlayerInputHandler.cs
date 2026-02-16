using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpPressed { get; private set; }

    private PlayerControls m_controls;

    private void Awake()
    {
        m_controls = new PlayerControls();
    }

    private void OnEnable()
    {
        m_controls.Enable();

        // MOVIMENTO
        m_controls.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        m_controls.Player.Move.canceled += _ => MoveInput = Vector2.zero;

        // LOOK (mouse ou analog)
        m_controls.Player.LookCamera.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        m_controls.Player.LookCamera.canceled += _ => LookInput = Vector2.zero;

        // PULO
        m_controls.Player.Jump.performed += _ => JumpPressed = true;
    }

    private void Update()
    {
        // Reset do pulo por frame
        JumpPressed = false;
    }

    private void OnDisable()
    {
        m_controls.Disable();
    }
}
