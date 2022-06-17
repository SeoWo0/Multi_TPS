using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public enum EItemType
    {
        Immediately,
        Has,

        Count
    }

    // 스폰된 지점 알기 위한 변수
    public int index;

    public EItemType itemType;

    // public abstract void Use();
}
