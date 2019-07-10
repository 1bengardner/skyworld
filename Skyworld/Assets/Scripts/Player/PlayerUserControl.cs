using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (PlayerMovement))]
public class PlayerUserControl : MonoBehaviour
{
    private PlayerMovement m_Character;
    private bool m_Run;
    private bool m_Jump;
    private bool m_HoldJump;
    private bool m_RunLock;
    public delegate void RunLockAction();
    public event RunLockAction OnRunLock;

    private void Awake()
    {
        m_Character = GetComponent<PlayerMovement>();
    }

    void Start()
    {
        if (PlayerPrefs.GetInt("Run Lock") == 1 && !m_RunLock)
        {
            ToggleRunLock();
        }
    }

    void ToggleRunLock()
    {
        m_RunLock = !m_RunLock;
        if (OnRunLock != null)
        {
            OnRunLock();
        }
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.toggleKeysEnabled && CrossPlatformInputManager.GetButtonDown("Run Lock"))
        {
            ToggleRunLock();
            PlayerPrefs.SetInt("Run Lock", m_RunLock ? 1 : 0);
        }
        m_Run = m_RunLock ? !CrossPlatformInputManager.GetButton("Run") : CrossPlatformInputManager.GetButton("Run");

        // Read the jump input in Update so button presses aren't missed.
        if (!m_Jump && !m_HoldJump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
        m_HoldJump = CrossPlatformInputManager.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        // Read the inputs.
        bool crouch = Input.GetButton("Crouch");
        float h = m_Run ? CrossPlatformInputManager.GetAxis("Horizontal") : CrossPlatformInputManager.GetAxis("Horizontal") / 2;
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        if (GameManager.Instance != null && GameManager.Instance.paused)
        {
            m_Character.Move(0f, 0f, false, false, false);
        }
        else
        {
            // Pass all parameters to the character control script.
            m_Character.Move(h, v, crouch, m_Jump, m_HoldJump);
        }

        m_Jump = false;
    }
}
