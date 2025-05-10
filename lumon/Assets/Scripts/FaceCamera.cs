using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    
    void Start()
    {
        // Get reference to the main camera
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        // Make the text face the camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        
        // Alternative method that maintains the up direction
        // transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
        //                 mainCamera.transform.rotation * Vector3.up);
    }
}
