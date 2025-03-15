using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;

public class Collect : MonoBehaviour
{

    void Start()
    {
        
    }

    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (other.gameObject.CompareTag("Cube"))
        {
            Debug.Log("hi");

            Destroy(other.gameObject);
            //access gamemanager and remove from the cube list
            
        }
    }
}
