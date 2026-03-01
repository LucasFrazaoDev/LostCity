using Unity.Cinemachine;
using UnityEngine;

public class CameraSensitivityMultiplier : MonoBehaviour
{
    [Header("Sensitivity Multiplier")]
    [Range(1f, 5f)]
    [SerializeField] private float m_sensitivityMultiplier = 3f;

    private CinemachineInputAxisController m_axisController;

    private float m_baseX = 1f;
    private float m_baseY = -1f;

    void Awake()
    {
        m_axisController = GetComponent<CinemachineInputAxisController>();
        ApplySensitivity();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

#if UNITY_EDITOR
    // Update sensitivity in editor when values change
    void OnValidate()
    {
        if (m_axisController == null)
            m_axisController = GetComponent<CinemachineInputAxisController>();

        ApplySensitivity();
    }
#endif

    void ApplySensitivity()
    {
        if (m_axisController == null) return;

        // X Axis (usually index 0)
        var axisX = m_axisController.Controllers[0];
        axisX.Input.Gain = m_baseX * m_sensitivityMultiplier;
        m_axisController.Controllers[0] = axisX;

        // Y Axis (usually index 1)
        var axisY = m_axisController.Controllers[1];
        axisY.Input.Gain = m_baseY * m_sensitivityMultiplier;
        m_axisController.Controllers[1] = axisY;
    }
}
