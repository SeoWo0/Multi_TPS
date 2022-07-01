using UnityEngine;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

public class PlayerGunAttackCommand : Command
{
    public PlayerGunAttackCommand(PlayerMove player, Gun gun) : base(player, gun)
    {
    }
    
    public override void Execute(Vector3 targetPos)          // 플레이어가 기본 총으로 공격하였을 때
    {
        ShotGunFire(targetPos);
    }

    public void ShotGunFire(Vector3 targetPos)                 // 플레이어 기본 총 공격 함수
    {
        gun.Use();

        var _position = gun.muzzlePos.position;
            
        Vector3 _aimDir = (targetPos - _position).normalized;

        for (int i = 0; i < gun.shotPos.Length; ++i)
        {
            //Quaternion.LookRotation(_aimDir, Vector3.up)
            var _projectile = Object.Instantiate(gun.bullet, gun.shotPos[i].position, gun.shotPos[i].rotation);
            _projectile.SetShooterInfo(player.photonView, gun.damage);
        }
    }
}
