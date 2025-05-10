using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class WinGame : MonoBehaviour
{
    public SerialController serialController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided");
            SceneManager.LoadScene("Win");
            serialController.SendSerialMessage("reset");
        }
    }
    }

