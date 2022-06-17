using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GetThing : MonoBehaviour
{
    public enum EGetType
    {
        Immediately,
        Has,

        Count
    }

    // 스폰된 지점 알기 위한 변수
    public int index;

    public EGetType itemType;

    // public abstract void Use();
}
