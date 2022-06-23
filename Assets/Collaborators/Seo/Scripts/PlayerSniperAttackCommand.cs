using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSniperAttackCommand : Command
{
    private PlayerMove m_player;
    private SniperGun m_sniper;

    public PlayerSniperAttackCommand(PlayerMove player, SniperGun gun)
    {
        m_player = player;
        m_sniper = gun;
    }

    public override void Execute()
    {
        //m_player.photonView.RPC(nameof(SniperFire), RpcTarget.All);
        SniperFire();
    }

    [PunRPC]
    public void SniperFire()
    {
        // 플레이어 attackPos 에서 앞으로 쭉 쏘기
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPos);

        if (Physics.Raycast(ray, out RaycastHit _hit, m_sniper.maxRange))
        {
            _hit.transform.TryGetComponent(out IDamagable _target);

            if (_target == null) return;

            _target.TakeDamage(m_sniper.damage);
        }

        m_sniper.Use();
    }
}
