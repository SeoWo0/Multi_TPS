using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 20f;

    private Rigidbody m_rigidbody;
    private Animator m_animator;

    public float hp = 100f;

    private Vector3 m_networkPos;
    private Quaternion m_networkRot;

    // Input
    private float m_vInput = 0f;
    private float m_hInput = 0f;

    // Misssile
    public GameObject missilePrefab;
    public Transform shootPos;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void Start()
    {
        foreach (Renderer _renderer in GetComponentsInChildren<Renderer>())
        {
            _renderer.material.color = GameData.GetColor(photonView.Owner.GetPlayerNumber());
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetButtonDown("Fire1"))
        {
            photonView.RPC(nameof(Fire), RpcTarget.All);
        }

        if (Input.GetButtonDown("Fire2"))
        {
            // OnPhotonSerializeView는 계속 변수의 확인이 필요할 때 -> 폴링 같은 방식
            Heal();
            // RPC는 한번 호출될때 즉 자주 호출되는 것이 아닐 때 효율적
            //photonView.RPC(nameof(Heal), RpcTarget.All);
        }
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            m_rigidbody.position = Vector3.MoveTowards(m_rigidbody.position, m_networkPos, Time.fixedDeltaTime);
            m_rigidbody.rotation = Quaternion.RotateTowards(m_rigidbody.rotation, m_networkRot, 100f * Time.fixedDeltaTime);
            return;
        }

        Move();
    }

    private void Move()
    {
        m_vInput = Input.GetAxisRaw("Vertical");
        m_hInput = Input.GetAxisRaw("Horizontal");

        Vector3 _moveDir = new Vector3(m_hInput, 0f, m_vInput);

        if (_moveDir.sqrMagnitude > 1)
        {
            _moveDir.Normalize();
        }

        m_rigidbody.velocity = _moveDir * moveSpeed;

        m_animator.SetFloat("Speed", _moveDir.magnitude);

        if (_moveDir.sqrMagnitude == 0) return;
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(_moveDir), rotateSpeed * Time.deltaTime);

        //transform.Translate(new Vector3(0f, 0f, m_vInput) * (moveSpeed * Time.deltaTime), Space.Self);
        //transform.Rotate(new Vector3(0f, m_hInput, 0f) * (rotateSpeed * Time.deltaTime), Space.Self);
    }

    [PunRPC]
    private void Fire()
    {
        Instantiate(missilePrefab, shootPos.position, shootPos.rotation);
    }

    [PunRPC]
    public void Heal()
    {
        hp += 10;
    }

    public void Hit(float damage)
    {
        hp -= damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Debug.Log("OnPhotonSerializeView Called");
        if (stream.IsWriting)
        {
            stream.SendNext(hp);

            stream.SendNext(m_rigidbody.position);
            stream.SendNext(m_rigidbody.rotation);
            stream.SendNext(m_rigidbody.velocity);
        }
        else
        {
            hp = (float)stream.ReceiveNext();

            m_networkPos = (Vector3)stream.ReceiveNext();
            m_networkRot = (Quaternion)stream.ReceiveNext();
            m_rigidbody.velocity = (Vector3)stream.ReceiveNext();

            float _lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            m_networkPos += m_rigidbody.velocity * _lag;
        }
    }
}
