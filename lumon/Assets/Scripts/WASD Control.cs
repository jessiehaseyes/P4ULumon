using System;
using UnityEngine;
using System.IO.Ports;
using System.Collections;

public class WASDControl : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public string comPort = "COM5";
    public int baudRate = 9600;
    
    private SerialPort serialPort;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isUp = false;
    private bool isDown = false;
    private bool isStill = true;
    private string serialInput = "";
    
    void Start()
    {
        // Configure and open the serial port
        serialPort = new SerialPort(comPort, baudRate);
        serialPort.ReadTimeout = 20; // Increased timeout
        serialPort.WriteTimeout = 20;
        serialPort.NewLine = "\n"; // Explicitly setting newline character
        
        try
        {
            serialPort.Open();
            Debug.Log("Serial port opened successfully on " + comPort);
            StartCoroutine(ReadSerialData());
        }
        catch (Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
    }
    
    IEnumerator ReadSerialData()
    {
        while (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                // Only read when there's data available
                if (serialPort.BytesToRead > 0)
                {
                    // Read a line from the Arduino (which sends println messages)
                    serialInput = serialPort.ReadLine();
                    
                    // Trim any whitespace or newline characters
                    serialInput = serialInput.Trim();
                    
                    // Debug the received input
                    Debug.Log("Received: " + serialInput);
                    
                    // Process the input to set direction flags
                    ProcessSerialInput(serialInput);
                }
            }
            catch (TimeoutException)
            {
                // This is normal, just continue
            }
            catch (Exception e)
            {
                // Log other errors but don't stop the coroutine
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
            
            yield return new WaitForSeconds(0.01f); // Short wait instead of waiting for next frame
        }
    }
    
    void ProcessSerialInput(string input)
    {
        // Reset all direction flags
        isLeft = false;
        isRight = false;
        isUp = false;
        isDown = false;
        isStill = true;
        
        // Set the appropriate flag based on the Arduino message
        // Using case-insensitive comparison and trimming
        input = input.Trim().ToUpper();
        
        if (input.Contains("LEFT"))
        {
            isLeft = true;
            isStill = false;
        }
        else if (input.Contains("RIGHT"))
        {
            isRight = true;
            isStill = false;
        }
        else if (input.Contains("UP"))
        {
            isUp = true;
            isStill = false;
        }
        else if (input.Contains("DOWN"))
        {
            isDown = true;
            isStill = false;
        }
        else if (input.Contains("STILL"))
        {
            isStill = true;
        }
    }
    
    void Update()
    {
        // Get current position
        Vector3 position = transform.position;
        
        // Apply movement based on flags (from Arduino)
        if (isUp)
        {
            position += Vector3.forward * moveSpeed * Time.deltaTime;
        }
        
        if (isDown)
        {
            position += Vector3.back * moveSpeed * Time.deltaTime;
        }
        
        if (isLeft)
        {
            position += Vector3.left * moveSpeed * Time.deltaTime;
        }
        
        if (isRight)
        {
            position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        
        // Apply the position change
        transform.position = position;
    }
    
    void OnApplicationQuit()
    {
        // Close the serial port when the application quits
        CloseSerialPort();
    }
    
    void OnDestroy()
    {
        // Clean up the serial port when the component is destroyed
        CloseSerialPort();
    }
    
    void CloseSerialPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.Close();
                Debug.Log("Serial port closed");
            }
            catch (Exception e)
            {
                Debug.LogError("Error closing serial port: " + e.Message);
            }
        }
    }
    
    void OnMessageArrived(string msg)
    {
        Debug.Log(msg);
        
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    void OnConnectionEvent(bool success)
    {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }
}