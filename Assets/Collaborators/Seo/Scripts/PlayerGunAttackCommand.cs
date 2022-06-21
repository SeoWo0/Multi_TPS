using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttackCommand : Command
{   
    private ShotGun m_shotgun;

    public PlayerGunAttackCommand(ShotGun gun)
    {
        m_shotgun = gun;
    }    

    public override void Execute()          // 플레이어가 기본 총으로 공격하였을 때
    {
        PlayerGunAttack();
    }

    public void PlayerGunAttack()                 // 플레이어 기본 총 공격 함수
    {
        {
            // 플레이어 attackPos 에서 앞으로 쭉 쏘기 
            m_shotgun.Use();

            // if( 카메라 에임의 RayCast가 상대 플레이어를 찍지 못했을 때)
            {

            }
            // if( 카메라 에임의 Raycast가 상대 플레이어을 찍었을 때)
            {
                // 상대방 HP -1
            }
        }
        
    }
}
