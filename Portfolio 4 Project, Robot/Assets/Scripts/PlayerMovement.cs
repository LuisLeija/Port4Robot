using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMBAT STATS")]
    [SerializeField] int health;
    [SerializeField] int damage;

    public float timeBetweenAttacks;
    public GameObject projectile;
    public Transform shootLocation;

    [Header("MOVEMENT")]
    public float movespeed;
    public Transform orientation;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;

    Rigidbody rb;
    [SerializeField] Transform robot;
    [SerializeField] Transform head;
    public float xAngle;

    float nextAttackTime = 0;
    [SerializeField] float attackCD = .1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time > nextAttackTime)
            {
                Fire();
                nextAttackTime = Time.time + attackCD;
            }
        }


    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
             print("youdied");
            float timer = Time.deltaTime;
            timer = 20f;
            SceneManager.LoadScene("Menu");
           
        }
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Acceleration);
        //robot.forward = -orientation.forward;
        transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * 180);
        
        //xAngle += verticalInput * Time.deltaTime * 180;
        //if (Mathf.Abs(xAngle)<90)
        //{
        //    head.transform.rotation = Quaternion.Euler(xAngle, 0, 0);

        //    //head.Rotate(transform.right,xAngle, Space.World);
        //}
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > movespeed)
        {
            Vector3 limitedVel = flatVel.normalized * movespeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Fire()
    {
        Rigidbody rb = Instantiate(projectile, shootLocation.transform.position, head.rotation).GetComponent<Rigidbody>();
    }
}
