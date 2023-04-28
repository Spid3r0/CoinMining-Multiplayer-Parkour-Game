using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncingPlatform : MonoBehaviour
{
    [SerializeField] float bounceForce;
    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody objectRB = collision.rigidbody;

        objectRB.AddExplosionForce(bounceForce, collision.contacts[0].point, 5);
    }
}
