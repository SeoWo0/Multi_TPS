using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody bulletRigidBody;
    [SerializeField]
    private float bulletSpeed = 10f;

    private void Awake() {
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    private void Start() {
        bulletRigidBody.velocity = transform.forward * bulletSpeed * Time.deltaTime; 
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(gameObject);
    }
}
