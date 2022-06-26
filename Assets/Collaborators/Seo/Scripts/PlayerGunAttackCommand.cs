using UnityEngine;
using Photon.Pun.UtilityScripts;

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
        //m_shotgun.Use();

        var _position = gun.muzzlePos.position;
            
        Vector3 _aimDir = (targetPos - _position).normalized;

        //Vector3 _shotPos = (Vector2)m_shotgun.muzzlePos.position - Random.insideUnitCircle;
        var _projectile = Object.Instantiate(gun.bullet, _position, Quaternion.LookRotation(_aimDir, Vector3.up));
        _projectile.SetShooterInfo(player.photonView.Owner.NickName, player.photonView.Owner.GetPlayerNumber());
    }
}
