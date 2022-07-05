using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected PlayerMove player;
    protected Gun gun;

    // TODO : player가 필요할 지 모르겠음 일단 보류
    protected Command(PlayerMove player, Gun gun)
    {
        this.player = player;
        this.gun = gun;
    }
    
    public abstract void Execute(Vector3 targetPos);         // override 해서 사용할 함수
}
