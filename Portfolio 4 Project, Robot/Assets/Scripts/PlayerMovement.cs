using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("COMBAT STATS")]
    [SerializeField] public float health;
    public float currHealth;
    [SerializeField] int damage;
    [SerializeField] float attackCD = .1f;
    public GameObject projectile;
    public Transform shootLocation;

    [Header("MOVEMENT")]
    public float movespeed;
    public Transform orientation;
    float horizontalInput, verticalInput;
    Vector3 moveDirection;
    private Animator anim;

    Rigidbody rb;
    [SerializeField] Transform robot;
    [SerializeField] Transform head;
    public float xAngle;
    public Transform animRob;

    float nextAttackTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        PlatformController.singleton.Init("COM5", 115200);
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        float heave = PlatformController.singleton.MapRange(animRob.position.y,0.12f, 0.3f, -5, 5);
        PlatformController.singleton.floatValues[2] = heave;
        
        MyInput();
        SpeedControl();
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Joystick1Button5))
        {
            //PlatformController.singleton.floatValues[0] = Random.Range(-1f, 1f);
            if (Time.time > nextAttackTime)
            {
                Fire();
                SoundManager.PlaySound("SingleShotMachinegun");
                PlatformController.singleton.floatValues[1] = -8;
                nextAttackTime = Time.time + attackCD;
            }
            PlatformController.singleton.floatValues[1] *= 0.95f;

        }
        else
        {
            PlatformController.singleton.floatValues[1] = 0;
        }
    }

    public void TakeDamage(int amount)
    {
        currHealth -= amount;
        if (currHealth <= 0)
        {
            print("youdied");
            float timer = Time.deltaTime;
            timer = 20f;
            SceneManager.LoadScene("Menu");
        }
    }

    private bool MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput > .19 || verticalInput > .19)
        {
            MovePlayer();
            return true;
        }
        else if (horizontalInput < -.19 || verticalInput < -.19)
        {
            MovePlayer();
            return true;
        }
        else
        {
            anim.SetBool("IsMoving?", false);
            return false;
        }
    }

    private void MovePlayer()
    {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * movespeed * 10f, ForceMode.Acceleration);
        transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * 180);
        anim.SetBool("IsMoving?", true);

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
