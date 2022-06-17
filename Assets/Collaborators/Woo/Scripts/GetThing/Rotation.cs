using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private void Update()
    {
        ItemRotate();
    }


    private void ItemRotate()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
