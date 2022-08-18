using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float movespeed;
    public Transform orientation;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;
    [SerializeField] Transform robot;
    [SerializeField] Transform head;
    public float xAngle;

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
    }

    private void FixedUpdate()
    {
        MovePlayer();
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
        xAngle += verticalInput * Time.deltaTime * 180;
        if (Mathf.Abs(xAngle)<90)
        {
        head.transform.rotation = Quaternion.Euler(xAngle, 0, 0);

            //head.Rotate(transform.right,xAngle, Space.World);
        }
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
}
