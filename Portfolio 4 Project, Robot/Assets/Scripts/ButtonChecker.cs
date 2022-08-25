using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChecker : MonoBehaviour
{
    KeyCode[] buttons;

    void Start()
    {
        
    }

    void Update()
    {
        for (int i = 0; i < 20; i++)
        {
            KeyCode k = (KeyCode)System.Enum.Parse(typeof(KeyCode), "JoystickButton" + i);
            if (Input.GetKeyDown(k))
            {
                print("Joystick Button #" + i + " was pressed");
            }
        }
    }
}
