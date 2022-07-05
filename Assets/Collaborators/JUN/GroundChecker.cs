using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundChecker : MonoBehaviour
{
    [SerializeField]
    protected Transform feetPos;

    [SerializeField]
    protected float detectSize;

    public abstract bool IsGrounded();
}
