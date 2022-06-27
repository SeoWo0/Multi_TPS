using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.UtilityScripts;
using UnityEngine.Animations.Rigging;

public class PlayerMove : MonoBehaviourPun ,IDamagable, IPunObservable
{
    private Rigidbody m_rigid;
    private Animator m_animator;
    private PlayerInput m_input;
    private GroundChecker m_groundChecker;
    private Collider m_col;
    private Command m_attackCommand;
    private RagdollChanger m_ragdollChanger;
    private Item m_currentItem;
    private CameraMovement m_cameraMovement;
    private float m_jumpPower = 7f;
    private float m_extraGravity = -15f;
    private bool m_isDead;
    private bool m_isZoom;
    public bool IsDead => m_isDead; 

    [Header("Animation Rigging")]
    public Transform riggingTarget;

    [SerializeField] private Image aimImage;
    [SerializeField] private Image zoomImage;
    [SerializeField] private LayerMask attackTargetLayer;

    [Header("Player Info Setting")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private int m_Hp = 1;

    [Header("Weapon Info")] public Gun[] guns;
    [SerializeField] private Transform weaponHolder;

    public UnityAction onDeadEvent;
    public UnityAction<int> onScoreEvent;

    //FallAnimation
    //private bool isFall = false;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }

        set
        {
            moveSpeed = value;
        }
    }

    public int Hp
    {
        get
        {
            return m_Hp;
        }

        set
        {
            m_Hp = value;
        }
    }
    
    public void TakeDamage(int damage, string attackerName, int attackerNumber)
    {
        if (m_isDead) return;
        
        m_Hp -= damage;
        print("Hit!!");

        if (m_Hp <= 0)
        {
            if (photonView.IsMine)
            {
                Chat.instance.KillLog($"/c log {PhotonNetwork.LocalPlayer.NickName} 님이 {attackerName} 님에 의해 죽음");
            }
            else
            {
                Chat.instance.KillLog($"/c log {photonView.Owner.NickName} 님이 {attackerName} 님에 의해 죽음");
            }

            //Die();
            photonView.RPC(nameof(Die), RpcTarget.All, attackerNumber);
        }
    }

    [PunRPC]
    public void Die(int killerNumber)
    {
        m_ragdollChanger.ChangeRagdoll();
        
        m_isDead = true;
        enabled = false;
        onDeadEvent?.Invoke();
        onScoreEvent?.Invoke(killerNumber);
    }

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
        m_groundChecker = GetComponent<SphereGroundChecker>();
        m_input = GetComponent<PlayerInput>();

        m_col = GetComponent<Collider>();

        GameManager.Instance.onGameComplete += OnGameComplete;

        m_col = GetComponent<Collider>();
        m_ragdollChanger = GetComponent<RagdollChanger>();
        m_cameraMovement = GetComponent<CameraMovement>();

    }

    private void Update()
    {
        if(!photonView.IsMine) return;

        Vector3 _screenCenterPos = new Vector3(Screen.width / 2f, Screen.height / 2f);
        Ray _ray = Camera.main.ScreenPointToRay(_screenCenterPos);

        if (Physics.Raycast(_ray, out RaycastHit _hit, attackTargetLayer))
        {
            riggingTarget.position = _hit.point;

            if (_hit.collider.tag == "Player")
            {
                aimImage.color = Color.red;
            }
            else
            {
                aimImage.color = Color.yellow;
            }
            
            if (m_input.MouseLeft)
            {
                photonView.RPC(nameof(Attack), RpcTarget.All, _hit.point);
            }
        }

        if(m_input.MouseRight)
        {
            Zoom();
        }

        m_animator.SetBool("IsGround", m_groundChecker.IsGrounded());
        if (m_input.JumpInput && m_groundChecker.IsGrounded())
            Jump();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        Move();
    }

    private void Move()
    {
        Vector3 _dir = transform.right * m_input.HInput + transform.forward * m_input.VInput;
        if (_dir.magnitude > 1)
        {
            _dir.Normalize();
        }

        transform.Translate(_dir * (moveSpeed * Time.deltaTime), Space.World);

        //rigid.velocity = new Vector3(_dir.x * moveSpeed, rigid.velocity.y, _dir.z * moveSpeed);

        //animator setting
        if (Mathf.Approximately(_dir.x, 0) && Mathf.Approximately(_dir.z, 0))
            m_animator.SetBool("Walk", false);
        else
            m_animator.SetBool("Walk", m_groundChecker.IsGrounded());
        
        m_animator.SetFloat("xDir", m_input.HInput);
        m_animator.SetFloat("yDir", m_input.VInput);
    }
    
    // [PunRPC]
    private void Jump()
    {
        m_rigid.AddForce(Vector3.up * m_jumpPower, ForceMode.Impulse);
        m_animator.SetTrigger("jumping");
    }

    [PunRPC]
    private void Attack(Vector3 targetPos)
    {
        if (!m_currentItem) return;
        
        m_attackCommand.Execute(targetPos);
        aimImage.gameObject.SetActive(false);

        m_currentItem = null;
        m_animator.SetBool("HasGun", false);
    }
    
    private void Zoom()
    {
        if (m_currentItem.gunType == Item.EGunType.Sniper)
        {

            if(!m_isZoom)
            {
                zoomImage.gameObject.SetActive(true);
                Camera.main.GetComponent<Camera>().fieldOfView = 15;
                m_isZoom = true;

                if(m_cameraMovement.rotXCamAxisSpeed < 100 || m_cameraMovement.rotYCamAxisSpeed < 100)
                    return;
                m_cameraMovement.rotXCamAxisSpeed -= 100;
                m_cameraMovement.rotYCamAxisSpeed -= 100;
            }

            else
            {
                zoomImage.gameObject.SetActive(false);
                Camera.main.GetComponent<Camera>().fieldOfView = 60;
                m_isZoom = false;

                if(m_cameraMovement.rotXCamAxisSpeed < 100 || m_cameraMovement.rotYCamAxisSpeed < 100)
                    return;
                m_cameraMovement.rotXCamAxisSpeed += 100;
                m_cameraMovement.rotYCamAxisSpeed += 100;
            }
        }   
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_Hp);
            stream.SendNext(moveSpeed);
        }
        else
        {
            m_Hp = (int) stream.ReceiveNext();
            moveSpeed = (float) stream.ReceiveNext();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if (other.CompareTag("Item"))
        {
            Item _item = other.transform.GetComponent<Item>();
            
            if (m_currentItem)
            {
                if (_item.useType == Item.EUseType.Immediately)
                {
                    _item.Use();
                }
                return;
            }

            m_currentItem = _item;

            if (m_currentItem.useType == Item.EUseType.Immediately)
            {
                m_currentItem.Use();
                m_currentItem = null;
            }

            else
            {
                if (m_currentItem.itemType != Item.EItemType.Weapon) return;

                string _gunName = Enum.GetName(typeof(Item.EGunType), m_currentItem.gunType);
                photonView.RPC(nameof(ActivateGun), RpcTarget.All, _gunName);
                
                m_animator.SetBool("HasGun", true);
            }
        }

        if (other.CompareTag("DeadZone"))
        {
            TakeDamage(100, "DEADZONE", -1);
            print("바닥충돌!");
        }
    }

    [PunRPC]
    public void ActivateGun(string gunType)
    {
        foreach (Gun _gun in guns)
        {
            if (Enum.GetName(typeof(Item.EGunType), _gun.gunType) != gunType) continue;

            m_currentItem = _gun;
            _gun.gameObject.SetActive(true);
            aimImage.gameObject.SetActive(true);

            if(m_currentItem.gunType == Item.EGunType.Sniper)
            {
                aimImage.gameObject.SetActive(false);
            }
            
            break;
        }
        
        switch (m_currentItem.gunType)
        {
            case Item.EGunType.ShotGun:
                m_attackCommand = new PlayerGunAttackCommand(this, m_currentItem as ShotGun);
                break;

            case Item.EGunType.Sniper:
                m_attackCommand = new PlayerSniperAttackCommand(this, m_currentItem as SniperGun);
                break;
                    
            case Item.EGunType.None:
                break;
                    
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnGameComplete()
    {
        m_input.playerControllerInputBlocked = true;
    }
}
