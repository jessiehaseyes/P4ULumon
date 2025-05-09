using System;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject prefabCup;
    public Vector3 speed;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newCup = Instantiate<GameObject>(prefabCup);

            newCup.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.z);
                
            speed = new Vector3(0, 0, -30);
            
            // Get the Rigidbody component and set its velocity
            Rigidbody rb = newCup.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = speed;
            }
            else
            {
                Debug.LogError("No Rigidbody component found on the prefab!");
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Note: You should destroy the gameObject, not the collider
        Destroy(other.gameObject);
    }
}