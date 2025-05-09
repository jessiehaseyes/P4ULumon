using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightTrigger : MonoBehaviour
{
    public GameObject lights;
 
    
    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided with knife");
            lights.SetActive(true);
      
        }
    }
}
