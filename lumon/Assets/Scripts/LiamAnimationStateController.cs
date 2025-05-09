using System;
using UnityEngine;
using System.IO.Ports;
using System.Collections;

public class LiamAnimationStateController : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator animator;
    private int _isWalkingHash;  
    private int _isRunningHash;

    [Header("Movement Settings")]
    [SerializeField] private float smoothTime = 0.05f;
    [SerializeField] private float speed = 5f;

    [Header("Arduino Serial Settings")]
    public bool useArduino = true;
    public string comPort = "COM5";
    public int baudRate = 9600;

    private Vector2 _input;
    private Vector3 _moveDirection;
    private float _currentVelocity;
    private CharacterController _characterController;

    // Arduino control variables
    private SerialPort serialPort;
    private bool isLeft = false;
    private bool isRight = false;
    private bool isUp = false;
    private bool isDown = false;
    private bool isRunning = false;
    private bool isStill = true;
    private string serialInput = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");

        if (useArduino)
        {
            InitializeSerialPort();
        }
    }

    private void InitializeSerialPort()
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
            useArduino = false; // Disable Arduino control if we can't open the port
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
        
        // Check if input contains RUN command
        if (input.Contains("RUN"))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleAnimation();
        HandleMovement();
        HandleRotation();
    }

    private void HandleInput()
    {
        if (useArduino && !Input.anyKey)
        {
            // Use Arduino input if enabled and no keyboard input detected
            float horizontal = 0f;
            float vertical = 0f;
            
            if (isLeft) horizontal = -1f;
            if (isRight) horizontal = 1f;
            if (isUp) vertical = 1f;
            if (isDown) vertical = -1f;
            
            _input = new Vector2(horizontal, vertical).normalized;
        }
        else
        {
            // Use keyboard input
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            _input = new Vector2(horizontal, vertical).normalized;
        }
        
        _moveDirection = new Vector3(_input.x, 0f, _input.y);
    }

    private void HandleAnimation()
    {
        bool currentlyRunning = animator.GetBool(_isRunningHash);
        bool currentlyWalking = animator.GetBool(_isWalkingHash);
        
        // Check if we are moving (either from keyboard or Arduino)
        bool isMoving = _input.sqrMagnitude > 0;
        
        // Check if run is pressed (either keyboard shift or Arduino run command)
        bool runPressed = Input.GetKey(KeyCode.LeftShift) || (useArduino && isRunning);
        
        // Update walking animation
        if (!currentlyWalking && isMoving)
        {
            animator.SetBool(_isWalkingHash, true);
        }
        
        if (currentlyWalking && !isMoving)
        {
            animator.SetBool(_isWalkingHash, false);
        }
        
        // Update running animation
        if (!currentlyRunning && (isMoving && runPressed))
        {
            animator.SetBool(_isRunningHash, true);
        }
        
        if (currentlyRunning && (!isMoving || !runPressed))
        {
            animator.SetBool(_isRunningHash, false);
        }
    }

    private void HandleMovement()
    {
        if (_input.sqrMagnitude == 0) return;
        
        // Determine if we should run (from keyboard or Arduino)
        bool shouldRun = Input.GetKey(KeyCode.LeftShift) || (useArduino && isRunning);
        float currentSpeed = shouldRun ? speed * 2 : speed;
        
        Vector3 movement = _moveDirection * currentSpeed * Time.deltaTime;
        
        if (_characterController != null)
        {
            _characterController.Move(movement);
        }
        else
        {
            transform.position += movement;
        }
    }

    private void HandleRotation()
    {
        if (_input.sqrMagnitude == 0) return;
        
        var targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
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
}