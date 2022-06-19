using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public Transform attackPos;             // 캐릭터의 공격 위치

    public abstract void Execute();         // override 해서 사용할 함수
}
