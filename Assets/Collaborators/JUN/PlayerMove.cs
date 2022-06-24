using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPun ,IDamagable
{
    Rigidbody rigid;
    Animator animator;
    PlayerInput m_input;
    GroundChecker groundChecker;
    Collider col;
    
    PlayerGunAttackCommand playerGunAttackCommand;
    PlayerSniperAttackCommand sniperAttack;
    [SerializeField] private LayerMask attackTargetLayer;
    RagdollChanger ragdollChanger;

    Item currentItem;
    
    [SerializeField]
    private float moveSpeed = 4f;
    private float jumpPower = 7f;

    [SerializeField] private Transform weaponHolder;

    private float m_extraGravity = -15f;
    
    [SerializeField]
    private int m_Hp = 1;

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

    [PunRPC]
    public void TakeDamage(int damage)
    {
        m_Hp -= damage;
        print("Hit!!");

        if (m_Hp <= 0)
        {
            Die();
            //photonView.RPC(nameof(Die), RpcTarget.All);
        }
    }

    [PunRPC]
    public void Die()
    {
        ragdollChanger.ChangeRagdoll();
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        groundChecker = GetComponent<SphereGroundChecker>();
        m_input = GetComponent<PlayerInput>();
        col = GetComponent<Collider>();
        ragdollChanger = GetComponent<RagdollChanger>();
    }


    private void Update()
    {
        if(!photonView.IsMine)
            return;

        if (m_input.MouseLeft)
        {
            Vector3 _screenCenterPos = new Vector3(Screen.width / 2f, Screen.height / 2f);
            Ray _ray = Camera.main.ScreenPointToRay(_screenCenterPos);

            if (Physics.Raycast(_ray, out RaycastHit _hit, attackTargetLayer))
            {
                photonView.RPC(nameof(Attack), RpcTarget.All, _hit.point);
            }
            //Attack();
        }

        //Fall Animation
        //animator.SetBool("IsGround", groundChecker.IsGrounded());
        //if(groundChecker.IsGrounded() == true)
        //{
        //    isFall = false;
        //}

        if (m_input.JumpInput && groundChecker.IsGrounded())
            Jump();

        //Fall Animation
        //if (rigid.velocity.y < -0.1f && !groundChecker.IsGrounded())
        //{
        //    if(isFall == false)
        //    {
        //        animator.SetTrigger("Fall");
        //        isFall = true;
        //    }

        //}
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
            animator.SetBool("Walk", false);
        else
            animator.SetBool("Walk", groundChecker.IsGrounded());
        
        animator.SetFloat("xDir", m_input.HInput);
        animator.SetFloat("yDir", m_input.VInput);
    }
    
    // [PunRPC]
    private void Jump()
    {
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        animator.SetTrigger("jumping");
    }

    [PunRPC]
    private void Attack(Vector3 targetPos)
    {
        if (!currentItem)
        {
            animator.SetBool("HasGun", false);
            return;
        }

        switch (currentItem.gunType)
        {
            case Item.EGunType.ShotGun:
                playerGunAttackCommand.Execute(targetPos);
                break;
            case Item.EGunType.Sniper:
                sniperAttack.Execute(targetPos);
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (currentItem && other.transform.GetComponent<Item>().itemType == Item.EItemType.Weapon)
            {
                return;
            }
            
            currentItem = other.transform.GetComponent<Item>();

            switch (currentItem.itemType)
            {
                case Item.EItemType.Weapon:
                    WeaponSpawnManager.Instance.CheckListRemove(currentItem.index);
                    break;

                case Item.EItemType.Buff:
                    ItemSpawnManager.Instance.CheckListRemove(currentItem.index);
                    break;
            }

            if (currentItem.useType == Item.EUseType.Immediately)
            {
                currentItem.Use();
            }

            else
            {
                switch (currentItem.gunType)
                {
                    case Item.EGunType.ShotGun:
                        playerGunAttackCommand = new PlayerGunAttackCommand(this, currentItem as ShotGun);
                        break;

                    case Item.EGunType.Sniper:
                        sniperAttack = new PlayerSniperAttackCommand(this, currentItem as SniperGun);
                        break;
                    
                    case Item.EGunType.None:
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                currentItem.transform.SetParent(weaponHolder);
                currentItem.transform.SetPositionAndRotation(weaponHolder.transform.position, weaponHolder.transform.rotation);
                animator.SetBool("HasGun", true);
            }
        }
    }
}
