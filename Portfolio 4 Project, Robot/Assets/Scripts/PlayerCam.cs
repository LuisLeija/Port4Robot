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
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senseX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senseY;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90);
        //cam orient
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        //transform.Rotate(mouseY, 0, 0);
    }
}
