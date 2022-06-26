using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using ExitGames.Client.Photon;

[System.Serializable]
public struct tPlayerCommand
{
    public KeyCode playerJump;
    public KeyCode playerShoot;
    public KeyCode playerZoom;
}

public class PlayerInput : MonoBehaviour
{
    //public static PlayerInput instance
    //{
    //    get { return s_Instance; }
    //}
    //private static PlayerInput s_Instance;

    [HideInInspector] public bool playerControllerInputBlocked;

    [SerializeField]
    public tPlayerCommand m_command;

    // 플레이어 움직임 입력
    private bool  m_walk;
    private bool  m_Jump;
    private float m_vInput;
    private float m_hInput;
    private int ownerId;

    private float m_mouseX;
    private float m_mouseY;
    private bool m_mouseRight;
    private bool m_mouseLeft;
    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
    }

    public bool JumpInput
    {
        get
        {
            // Jump 이며 Block이 아닐때
            return m_Jump && !playerControllerInputBlocked;
        }
    }

    // 위,아래 움직임 반환
    public float VInput
    {
        get
        {
            if (playerControllerInputBlocked)
                return 0;

            return m_vInput;
        }
    }

    //좌,우 움직임 반환
    public float HInput
    {
        get
        {
            if (playerControllerInputBlocked)
                return 0;

            return m_hInput;
        }
    }

    //마우스 움직임 반환    
    public float MouseX
    {
        get
        {
            if (playerControllerInputBlocked)
                return 0;
            
            return m_mouseX;
        }
    }

    public float MouseY
    {
        get
        {
            if (playerControllerInputBlocked)
                return 0;
            
            return m_mouseY;
        }
    }

    public bool MouseRight
    {
        get
        {
            return m_mouseRight && !playerControllerInputBlocked;
        }
    }

    public bool MouseLeft
    {
        get
        {
            return m_mouseLeft && !playerControllerInputBlocked;
        }
    }

    private void Awake()
    {
        //if (s_Instance == null)
        //{
        //    s_Instance = this;
        //} 
        //else if(s_Instance != this)
        //{
        //    Debug.Log("Input은 1개만 있어야함");
        //    Destroy(gameObject);
        //}

        // Player Input Block -> 입력을 안받음
        playerControllerInputBlocked = false;

    }

    private void Update()
    {
        if (Input.GetButtonDown("Chat"))
            playerControllerInputBlocked = true;

        else if (Input.GetButtonDown("Submit"))
            playerControllerInputBlocked = false;

        m_Jump = Input.GetKeyDown(m_command.playerJump);

        //키 입력시 값 변환
        m_hInput = Input.GetAxisRaw("Horizontal");
        m_vInput = Input.GetAxisRaw("Vertical");

        m_mouseX = Input.GetAxis("Mouse X");
        m_mouseY = Input.GetAxis("Mouse Y");

        m_mouseLeft = Input.GetKeyDown(m_command.playerShoot);
        
        if(Input.GetKeyDown(m_command.playerZoom)) 
        {
            m_mouseRight = !m_mouseRight;
        }
        
        
        
    }
}
