using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunAttackCommand : Command
{   
    public override void Execute()          // 플레이어가 기본 총으로 공격하였을 때
    {
        PlayerGunAttack();
    }

    public void PlayerGunAttack()                 // 플레이어 기본 총 공격 함수
    {
        // if( 마우스 좌클릭을 눌렀을때 : InputManager의 특정 bool 값 && 총을 들고 있을 때 : PlayerController의 특정 bool 값)
        {
            // 플레이어 attackPos 에서 앞으로 쭉 쏘기

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
