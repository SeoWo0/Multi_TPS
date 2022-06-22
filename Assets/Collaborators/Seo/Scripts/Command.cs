using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    protected PlayerInput playerInput;
    
    protected Vector3 screenCenterPos = new Vector2(Screen.width / 2f, Screen.height / 2f);

    public abstract void Execute();         // override 해서 사용할 함수
}
