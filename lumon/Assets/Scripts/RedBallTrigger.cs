using UnityEngine;

public class RedBallTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("redball triggered");
          
        }
    }
}

