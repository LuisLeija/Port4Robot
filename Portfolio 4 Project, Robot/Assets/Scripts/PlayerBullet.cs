using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.right * 50, ForceMode.VelocityChange);
        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("hit"+ collision.gameObject.name);
        Enemy_M melee = collision.transform.root.GetComponent<Enemy_M>();
        Enemy_R ranged = collision.transform.root.GetComponent<Enemy_R>();

        if (melee != null)
        {
            melee.TakeDamage(20);
        }
        else if (ranged != null)
        {
            ranged.TakeDamage(20);
        }
        Destroy(gameObject);
    }
}
