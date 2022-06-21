using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSniperAttackCommand : Command
{
    public PlayerSniperAttackCommand()
    {

    }

    public override void Execute()
    {
        SniperFire();
    }

    public void SniperFire()
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
