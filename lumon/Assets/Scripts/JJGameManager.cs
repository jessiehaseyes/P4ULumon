using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
public class JJGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float exitOpenDuration = 30f; // Duration the exit stays open in seconds
    public int requiredItems = 4; // Number of items needed to open exit (4 different colored balls)
    
    [Header("References")]
    public GameObject exitDoor;
    public GameObject countdownTimerObject;
    public GameObject Door;
    public GameObject Exit;
    
    // Track which balls have been collected
    private bool greenBallCollected = false;
    private bool redBallCollected = false;
    private bool blueBallCollected = false;
    private bool yellowBallCollected = false;
    
    // Ball prefabs for respawning
    public GameObject greenBallPrefab;
    public GameObject redBallPrefab;
    public GameObject blueBallPrefab;
    public GameObject yellowBallPrefab;
    
    // Original spawn positions
    private Vector3 greenBallSpawnPos;
    private Vector3 redBallSpawnPos;
    private Vector3 blueBallSpawnPos;
    private Vector3 yellowBallSpawnPos;
    
    // Track the actual ball instances
    private GameObject greenBallInstance;
    private GameObject redBallInstance;
    private GameObject blueBallInstance;
    private GameObject yellowBallInstance;
    
    private bool isExitOpen = false;
    private Coroutine exitTimerCoroutine;
    private TextMeshProUGUI timerText;

    void Start()
    {
        // Make sure exit is closed at start
        CloseExit();
        
        // Store initial ball positions and references
        InitializeBalls();
        
        // Initialize timer text and hide it
        if (countdownTimerObject != null)
        {
            timerText = countdownTimerObject.GetComponent<TextMeshProUGUI>();
            countdownTimerObject.SetActive(false);
        }
    }
    
    void InitializeBalls()
    {
        // Find the balls in the scene and store their positions
        if (greenBallPrefab != null)
        {
            greenBallInstance = GameObject.FindGameObjectWithTag("GreenBall");
            if (greenBallInstance != null)
                greenBallSpawnPos = greenBallInstance.transform.position;
        }
        
        if (redBallPrefab != null)
        {
            redBallInstance = GameObject.FindGameObjectWithTag("RedBall");
            if (redBallInstance != null)
                redBallSpawnPos = redBallInstance.transform.position;
        }
        
        if (blueBallPrefab != null)
        {
            blueBallInstance = GameObject.FindGameObjectWithTag("BlueBall");
            if (blueBallInstance != null)
                blueBallSpawnPos = blueBallInstance.transform.position;
        }
        
        if (yellowBallPrefab != null)
        {
            yellowBallInstance = GameObject.FindGameObjectWithTag("YellowBall");
            if (yellowBallInstance != null)
                yellowBallSpawnPos = yellowBallInstance.transform.position;
        }
    }
    
    public void BallCollected(string ballTag)
    {
        // Mark the appropriate ball as collected
        switch (ballTag)
        {
            case "GreenBall":
                greenBallCollected = true;
                Debug.Log("Green ball collected!");
                break;
            case "RedBall":
                redBallCollected = true;
                Debug.Log("Red ball collected!");
                break;
            case "BlueBall":
                blueBallCollected = true;
                Debug.Log("Blue ball collected!");
                break;
            case "YellowBall":
                yellowBallCollected = true;
                Debug.Log("Yellow ball collected!");
                break;
        }
        
        // Check if all balls are collected
        CheckAllBallsCollected();
    }
    
    void CheckAllBallsCollected()
    {
        if (greenBallCollected && redBallCollected && blueBallCollected && yellowBallCollected)
        {
            Debug.Log("All balls collected! Opening exit...");
            OpenExit();
            countdownTimerObject.SetActive(true);
            Door.SetActive(false);
            Exit.SetActive(true);
        }
    }
    
    void OpenExit()
    {
        if (!isExitOpen)
        {
            isExitOpen = true;
            
            // Activate exit door
            exitDoor.GetComponent<ExitDoor>().OpenDoor();
            
            // Start timer to close exit
            exitTimerCoroutine = StartCoroutine(ExitTimer());
            
            // Show the countdown timer
            if (countdownTimerObject != null)
            {
                countdownTimerObject.SetActive(true);
                
            }
            
            Debug.Log("Exit opened! You have " + exitOpenDuration + " seconds to reach it!");
        }
    }
    
    void CloseExit()
    {
        if (isExitOpen)
        {
            isExitOpen = false;
            
            // Deactivate exit
            exitDoor.GetComponent<ExitDoor>().CloseDoor();
            
            // Hide the countdown timer
            if (countdownTimerObject != null)
            {
                countdownTimerObject.SetActive(false);
             
            }
            
            Debug.Log("Exit closed! Collect all balls again.");
        }
    }
    
    IEnumerator ExitTimer()
    {
        float timeRemaining = exitOpenDuration;
        
        while (timeRemaining > 0)
        {
            // Update the timer text
            if (timerText != null)
            {
                int seconds = Mathf.FloorToInt(timeRemaining);
                timerText.text = seconds.ToString();
            }
            
            yield return new WaitForSeconds(0.1f); // Update more frequently for smoother countdown
            timeRemaining -= 0.1f;
        }
        
        // Time's up - close exit and reset items
        CloseExit();
        ResetBalls();
    }
    
    void ResetBalls()
    {
        // Reset collection flags
        greenBallCollected = false;
        redBallCollected = false;
        blueBallCollected = false;
        yellowBallCollected = false;
        
        // Respawn all balls
        RespawnBall("GreenBall", greenBallPrefab, greenBallSpawnPos);
        RespawnBall("RedBall", redBallPrefab, redBallSpawnPos);
        RespawnBall("BlueBall", blueBallPrefab, blueBallSpawnPos);
        RespawnBall("YellowBall", yellowBallPrefab, yellowBallSpawnPos);
        
        Debug.Log("All balls have been reset. Start collecting again!");
    }
    
    void RespawnBall(string ballTag, GameObject ballPrefab, Vector3 spawnPosition)
    {
        if (ballPrefab != null)
        {
            GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            newBall.tag = ballTag;
            
            // Store reference to the new instance
            switch (ballTag)
            {
                case "GreenBall":
                    greenBallInstance = newBall;
                    break;
                case "RedBall":
                    redBallInstance = newBall;
                    break;
                case "BlueBall":
                    blueBallInstance = newBall;
                    break;
                case "YellowBall":
                    yellowBallInstance = newBall;
                    break;
            }
        }
    }
    
    public void PlayerReachedExit()
    {
        // Stop the exit timer
        if (exitTimerCoroutine != null)
        {
            StopCoroutine(exitTimerCoroutine);
        }
        
        // Hide the countdown timer
        if (countdownTimerObject != null)
        {
            countdownTimerObject.SetActive(false);
        }
        
        // Player wins!
        Debug.Log("Congratulations! You've won the game!");
        
        // Here you could load a win screen or next level
    }
    
    void OnMessageArrived(string msg)
    {
        Debug.Log("Message arrived: " + msg);
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
