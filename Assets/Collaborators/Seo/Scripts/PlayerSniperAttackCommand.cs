using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSniperAttackCommand : Command
{
    private PlayerMove m_player;
    private SniperGun m_sniper;

    private GameObject m_hitEffect;

    public PlayerSniperAttackCommand(PlayerMove player, SniperGun gun, GameObject hitEffect)
    {
        m_player = player;
        m_sniper = gun;
        m_hitEffect = hitEffect;
    }

    public override void Execute()
    {
        SniperFire();
    }
    
    public void SniperFire()
    {
        //m_sniper.Use();
        
        // 플레이어 attackPos 에서 앞으로 쭉 쏘기
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPos);

        if (Physics.Raycast(ray, out RaycastHit _hit, m_sniper.maxRange))
        {
            GameObject.Instantiate(m_hitEffect, _hit.transform.position, Quaternion.identity);
            _hit.transform.TryGetComponent(out IDamagable _target);

            if (_target == null) return;

            _target.TakeDamage(m_sniper.damage);
        }
    }
}
