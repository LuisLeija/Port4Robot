using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * 50, ForceMode.VelocityChange);
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement player = collision.transform.root.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.TakeDamage(20);
        }
        Destroy(gameObject);
    }
}
