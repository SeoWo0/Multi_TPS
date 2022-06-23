using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGunAttackCommand : Command
{   
    private ShotGun m_shotgun;
    private PlayerMove m_player;

    public RaycastHit savedHitPoint;

    public PlayerGunAttackCommand(PlayerMove player, ShotGun gun)
    {
        m_player = player;
        m_shotgun = gun;
    }    

    public override void Execute()          // 플레이어가 기본 총으로 공격하였을 때
    {
        ShotGunFire();
    }

    [PunRPC]
    public void ShotGunFire()                 // 플레이어 기본 총 공격 함수
    {       
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, m_shotgun.maxRange))
        {
            hit.transform.TryGetComponent(out IDamagable _target);

            if(_target == null)
                return;
            _target.TakeDamage(m_shotgun.damage);
        }

        m_shotgun.Use();  
    }
}
