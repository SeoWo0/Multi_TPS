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
}

public class PlayerInput : MonoBehaviour,IPunObservable
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
    public float mouseX
    {
        get
        {
            return m_mouseX;
        }
    }

    public float mouseY
    {
        get
        {
            return m_mouseY;
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

        m_Jump = Input.GetKeyDown(m_command.playerJump);

        //키 입력시 값 변환
        m_hInput = Input.GetAxis("Horizontal");
        m_vInput = Input.GetAxis("Vertical");

        m_mouseX = Input.GetAxis("Mouse X");
        m_mouseY = Input.GetAxis("Mouse Y");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
