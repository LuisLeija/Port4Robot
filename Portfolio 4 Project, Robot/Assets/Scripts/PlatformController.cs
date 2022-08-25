using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class PlatformController : MonoBehaviour
{
    public static PlatformController instance;

    public enum PlatformModes { Mode_8Bit, Mode_Float };
    public PlatformModes mode = PlatformModes.Mode_Float;

    public SliderControls sliderControls;

    private SerialPort serialPort;
    public string comPort;
    public int baudRate;
    bool initialized = false; // a bool to check if this controller has been initialized

    // Note:
    // The index order of axes that we will be using is [Sway, Surge, Heave, Pitch, Roll, Yaw]
    // The values for each axis is stored in one of the following two arrays depending on the mode

    public byte[] byteValues; // six byte values to be sent to the platform (in 8Bit Mode)
    public float[] floatValues; // six 32bit float valuesz

    private string startFrame = "!"; // '!' startFrame character (33) (to indicate the start of a message)
    private string endFrame = "#"; // '#' endFrame character (35) (to indicate the end of a message)

    private float nextSendTimestamp = 0;
    private float nextSendDelay = 0.02f; // 20 milliseconds by default

    public Transform ball;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (!initialized) { Init(comPort, baudRate); }
    }

    public void Init(string _com, int _baud)
    {
        if (initialized)
        {
            Debug.LogWarning(typeof(PlatformController).ToString() + ": is already initialized");
            return;
        }

        initialized = true;

        // Define and set some default values
        comPort = _com;
        baudRate = _baud;
        byteValues = new byte[] { 128, 128, 128, 128, 128, 128 };
        floatValues = new float[] { 0, 0, 0, 0, 0, 0 };

        // Create SerialPort instance (this does not open the connection)
        if (serialPort == null)
        {
            serialPort = new SerialPort(@"\\.\" + comPort); // special port formating to force Unity to recognize ports beyond COM9            
            serialPort.BaudRate = baudRate;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            //serialPort.ReadTimeout = 20; // miliseconds
        }

        // Attempt to open the SerialPort and log any errors
        try
        {
            serialPort.Open();
            Debug.Log("Initialize Serial Port: " + comPort);
        }
        catch (System.IO.IOException ex)
        {
            Debug.LogError("Error opening " + comPort + "\n" + ex.Message);
        }

        // Reset sliders, if in use
        if (sliderControls != null) { sliderControls.SetSliderParameters(this); }

        // Reset platform values
        HomePlatform();
    }

    // sway, surge, heave, pitch, roll, yaw
    void Update()
    {
        //can also do a ternary which is a short hand of doing an if else
        //for(int i = 0; i < 6; i++)
        //{
        //  byteValues[i] = (byte)(i % 2 == 0 ? 0 : 255)
        //}
        // the question mark is like an else so mod i to see if even, if so it gets set to 0 if not its set to 255

        //print(ball.position.y);

        //for (int i = 0; i < 6; i++)
        //{
        //    if (i % 2 == 0)
        //    {
        //        byteValues[i] = (byte)MapRange(ball.position.y, (float).5, 4, 0, 255);
        //    }
        //    else if (i % 2 != 0)
        //    {
        //        byteValues[i] = (byte)(255 - MapRange(ball.position.y, (float).5, 4, 0, 255));
        //    }
        //}

        // 

        //byte val = (byte)MapRange(ball.position.y, (float).5, 4, 0, 255);
        //for(int i = 0; i < 6; i++)
        //{
        //    byteValues[i] = (i % 2 == 1 ? val : (byte)(255 - val));
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    byteValues[0] = 0;
        //    byteValues[1] = 255;
        //    byteValues[2] = 0;
        //    byteValues[3] = 255;
        //    byteValues[4] = 0;
        //    byteValues[5] = 255;
        //}
        //else if (Input.GetKeyDown(KeyCode.S))
        //{
        //    byteValues[0] = 255;
        //    byteValues[1] = 0;
        //    byteValues[2] = 255;
        //    byteValues[3] = 0;
        //    byteValues[4] = 255;
        //    byteValues[5] = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    byteValues[0] = 0;
        //    byteValues[1] = 0;
        //    byteValues[2] = 0;
        //    byteValues[3] = 0;
        //    byteValues[4] = 0;
        //    byteValues[5] = 0;
        //}
        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    byteValues[0] = 255;
        //    byteValues[1] = 255;
        //    byteValues[2] = 255;
        //    byteValues[3] = 255;
        //    byteValues[4] = 255;
        //    byteValues[5] = 255;
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    byteValues[0] = 0;
        //    byteValues[1] = 255;
        //    byteValues[2] = 0;
        //    byteValues[3] = 0;
        //    byteValues[4] = 0;
        //    byteValues[5] = 0;
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    byteValues[0] = 255;
        //    byteValues[1] = 255;
        //    byteValues[2] = 255;
        //    byteValues[3] = 255;
        //    byteValues[4] = 255;
        //    byteValues[5] = 255;
        //}

        // The following lines are constantly sending out serial data, comment to implement manual control
        // Limit the send rate to the platform to about 20 milliseconds (50 fps)
        if (Time.time > nextSendTimestamp)
        {
            nextSendTimestamp = Time.time + nextSendDelay;
            if (sliderControls != null) { SendSerialFromSliders(); }
            else { SendSerial(); }
        }
    }

    public float MapRange(float val, float min, float max, float newMin, float newMax)
    {
        return Mathf.Clamp(((val - min) / (max - min) * (newMax - newMin) + newMin), newMin, newMax);
        // or Y = (X-A)/(B-A) * (D-C) + C
    }

    // The main functions to send the values to our platform
    // There are Three formats, one for 8bit int values, 16bit int values, and 32bit float values
    public void SendSerial()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            // 8 Bit Mode
            if (mode == PlatformModes.Mode_8Bit)
            {
                serialPort.Write(startFrame);
                serialPort.Write(byteValues, 0, byteValues.Length);
                serialPort.Write(endFrame);
            }            
            
            // 32 Bit Float Mode
            else if (mode == PlatformModes.Mode_Float)
            {
                serialPort.Write(startFrame);
                for (int i = 0; i < floatValues.Length; i++)
                {
                    byte[] myBytes = System.BitConverter.GetBytes(floatValues[i]);
                    serialPort.Write(myBytes, 0, myBytes.Length);
                }
                serialPort.Write(endFrame);
            }
        }
    }

    public void SendSerialFromSliders()
    {
        for (int i = 0; i < SliderControls.instance.sliders.Length; i++)
        {
            if (mode == PlatformModes.Mode_8Bit) byteValues[i] = (byte)SliderControls.instance.sliders[i].value;            
            else if(mode == PlatformModes.Mode_Float) floatValues[i] = SliderControls.instance.sliders[i].value;
        }
        SendSerial();
    }

    public void HomePlatform()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            // 8 bit int mode (a range from 0 to 255)
            if (mode == PlatformModes.Mode_8Bit)
            {
                for (int i = 0; i < byteValues.Length; i++)
                {
                    byteValues[i] = 128;
                }
            }            
            // 32 bit float mode (a range from 1.18e^ -38 to 3.40e^ 38)
            else if (mode == PlatformModes.Mode_Float)
            {
                for (int i = 0; i < floatValues.Length; i++)
                {
                    floatValues[i] = 0;
                }
            }

            SendSerial();
        }
    }

    // At shutdown, attempt to reset values and close ports   
    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            if (sliderControls != null) { sliderControls.ResetSliders(); }
            HomePlatform();
            serialPort.Close();
        }
    }

    // In many cases, a singleton implementation of this controller is very convenient
    // for maintaining persistence and easy access.

    private static PlatformController _singleton;
    public static PlatformController singleton
    {
        get
        {
            if (_singleton == null)
            {
                GameObject go = new GameObject("PlatformController");
                DontDestroyOnLoad(go);
                _singleton = go.AddComponent<PlatformController>();
            }

            return _singleton;
        }
    }
}
