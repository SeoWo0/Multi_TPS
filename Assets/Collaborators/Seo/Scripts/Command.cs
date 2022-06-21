using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public PlayerInput playerInput;

    public abstract void Execute();         // override 해서 사용할 함수
}
