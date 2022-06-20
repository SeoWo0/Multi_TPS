using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPun ,IDamagable, IPunObservable
{
    Rigidbody rigid;
    Animator animator;
    PlayerInput m_input;
    GroundChecker groundChecker;

    private float moveSpeed = 10f;
    private float jumpPower = 10f;
    
    private int m_Hp = 1;
    
    public void TakeDamage(int damage)
    {
        //todo: 총에 맞았을때
    }

    public void Die()
    {
        //todo: 죽었을때
    }
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<SphereGroundChecker>();
        m_input = GetComponent<PlayerInput>();
    }


    private void Update()
    {
        if (!photonView.IsMine)
            return;
        
        Move();
        
        //rpc 많이쓰면 안됨
        photonView.RPC("Jump", RpcTarget.All);
        
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
            animator.SetBool("Walk", true);
        animator.SetFloat("xDir", m_input.HInput);
        animator.SetFloat("yDir", m_input.VInput);

    }

    [PunRPC]
    private void Jump()
    {
        if (!photonView.IsMine)
            return;

        animator.SetBool("Jump", !groundChecker.IsGrounded());
        if (!groundChecker.IsGrounded())
            return;

        if (m_input.JumpInput)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Item"))
        {
            // TODO : Item Use
            Destroy(other.gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        //서버에 데이터를 보내줌 -> 데이터 변경
        if(stream.IsWriting)
        {
            stream.SendNext(moveSpeed);
            stream.SendNext(m_Hp);
        }

        else
        {
            moveSpeed = (float)stream.ReceiveNext();
            m_Hp = (int)stream.ReceiveNext();
        }
    }
}
