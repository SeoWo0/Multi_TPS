using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSniperAttackCommand : Command
{
    public PlayerSniperAttackCommand(PlayerMove player, Gun gun) : base(player, gun)
    {
    }

    public override void Execute(Vector3 targetPos)
    {
        SniperFire(targetPos);
    }

    public void SniperFire(Vector3 targetPos)
    {
        gun.Use();

        var _position = gun.muzzlePos.position;
            
        Vector3 _aimDir = (targetPos - _position).normalized;

        Object.Instantiate(gun.bullet, _position, Quaternion.LookRotation(_aimDir, Vector3.up));
    }
}
