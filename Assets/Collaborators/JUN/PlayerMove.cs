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

    Item currentItem;

    [SerializeField]
    private float moveSpeed = 10f;
    private float jumpPower = 10f;
    
    [SerializeField]
    private int m_Hp = 1;

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

    public void TakeDamage(int damage)
    {
        //todo: 총에 맞았을때
    }

    public void Die()
    {
        //todo: 죽었을때
    }
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        groundChecker = GetComponent<SphereGroundChecker>();
        m_input = GetComponent<PlayerInput>();
        col = GetComponent<Collider>();
    }


    private void Update()
    {
        if (!photonView.IsMine)
            return;


        if (m_input.JumpInput && groundChecker.IsGrounded())
            Jump();

        if (animator.velocity.y > 0 && !groundChecker.IsGrounded())
            animator.SetTrigger("Fall");
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        Move();
    }

    private void Move()
    {
        //Vector3 m_Velocity= new Vector3(-m_input.HInput, 0, -m_input.VInput) * moveSpeed;
        Vector3 m_dir = transform.right * m_input.HInput + transform.forward * m_input.VInput;
        rigid.velocity = new Vector3(m_dir.x * moveSpeed, rigid.velocity.y, m_dir.z * moveSpeed);
        
        //animator setting
        if (Mathf.Approximately(m_dir.x, 0) && Mathf.Approximately(m_dir.z, 0))
            animator.SetBool("Walk", false);
        else
            animator.SetBool("Walk", groundChecker.IsGrounded());
        animator.SetFloat("xDir", m_input.HInput);
        animator.SetFloat("yDir", m_input.VInput);

    }

    private void Jump()
    {
        rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        animator.SetTrigger("jumping");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
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
                currentItem.Use();
            }
        }
    }
}
