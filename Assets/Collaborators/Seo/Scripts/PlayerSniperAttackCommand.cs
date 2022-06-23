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
        SniperFire();
    }

    public void SniperFire()
    {
        //m_sniper.Use();

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPos);

        if (Physics.Raycast(ray, out RaycastHit _hit, m_sniper.hitLayer))
        {
            Vector3 _aimDir = (_hit.point - m_sniper.muzzlePos.position).normalized;

            Object.Instantiate(m_sniper.bullet, m_sniper.muzzlePos.position, Quaternion.LookRotation(_aimDir, Vector3.up));
        }
    }
}
