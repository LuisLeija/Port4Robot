using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float senseX, senseY;
    public Transform orientation;
    float xRotation, yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //mouse input
        float mouseX = Input.GetAxisRaw("TurretY") * Time.deltaTime * senseX;
        float mouseY = Input.GetAxisRaw("TurretX") * Time.deltaTime * senseY;
        yRotation += mouseX;
        xRotation -= mouseY;


        //finally got him turning the right way, this was needed
        Quaternion qy = Quaternion.AngleAxis(mouseX, Vector3.up);
        transform.Rotate(Vector3.up, mouseX * -1, Space.World);
        transform.Rotate(transform.forward, mouseY * -1, Space.World);
    }
}
