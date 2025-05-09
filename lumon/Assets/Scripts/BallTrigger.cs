using UnityEngine;
using System;
using UnityEditor;

public class BallTrigger : MonoBehaviour
{
    private JJGameManager gameManager;
    
    void Start()
    {
        // Find the game manager
        gameManager = FindObjectOfType<JJGameManager>();
        
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene!");
        }
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GreenBall") || 
            other.gameObject.CompareTag("RedBall") || 
            other.gameObject.CompareTag("BlueBall") || 
            other.gameObject.CompareTag("YellowBall"))
        {
            // Notify GameManager which ball was collected
            gameManager.BallCollected(other.gameObject.tag);
            
            // Destroy the ball as in your original script
            Debug.Log(other.gameObject.tag + " triggered");
            Destroy(other.gameObject);
        }
    }
}