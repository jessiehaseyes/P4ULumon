using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class EnemyDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
         
            SceneManager.LoadScene("EndGame");
        }
    }
}
