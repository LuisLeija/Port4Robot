using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet_M : MonoBehaviour
{
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {        PlayerMovement player = collision.transform.root.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TakeDamage(5);
        }
        Destroy(gameObject);
    }
}
