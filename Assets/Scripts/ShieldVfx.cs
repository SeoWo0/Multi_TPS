using System;
using UnityEngine;
using Photon.Pun;
using Managers;

public class ShieldVfx : MonoBehaviour
{
    public float retentionTime;
    public ParticleSystem deactivateVfx;

    private PhotonView m_pv;

    private void Awake()
    {
        m_pv = GetComponent<PhotonView>();
    }
    
    private void OnEnable()
    {
        Destroy(gameObject, retentionTime);
    }

    private void OnDestroy()
    {
        PlayerMove _player = transform.root.gameObject.GetComponent<PlayerMove>();

        if (!_player) return;
        _player.ToggleShield(false);
        
        if (GameManager.Instance.isGameCompleted) return;
        Instantiate(deactivateVfx, transform.position, Quaternion.Euler(-90, 0, 0));
    }
}
