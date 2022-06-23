using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGunAttackCommand : Command
{   
    private ShotGun m_shotgun;
    private PlayerMove m_player;

    public PlayerGunAttackCommand(PlayerMove player, ShotGun gun)
    {
        m_player = player;
        m_shotgun = gun;
    }    

    public override void Execute()          // 플레이어가 기본 총으로 공격하였을 때
    {
        ShotGunFire();
    }

    public void ShotGunFire()                 // 플레이어 기본 총 공격 함수
    {
        //m_shotgun.Use();

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPos);

        if (Physics.Raycast(ray, out RaycastHit _hit, m_shotgun.hitLayer))
        {
            Vector3 _aimDir = (_hit.point - m_shotgun.muzzlePos.position).normalized;

            //Vector3 _shotPos = (Vector2)m_shotgun.muzzlePos.position - Random.insideUnitCircle;
            Object.Instantiate(m_shotgun.bullet, m_shotgun.muzzlePos.position, Quaternion.LookRotation(_aimDir, Vector3.up));
        }
    }
}
