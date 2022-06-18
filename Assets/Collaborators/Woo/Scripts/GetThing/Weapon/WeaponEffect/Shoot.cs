using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : Item
{
    public override void Use()
    {
        // TODO : 쏘기
        Destroy(gameObject);
    }
}
