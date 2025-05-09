using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform playerTransform;
    
    [Header("Damping Settings")]
    [SerializeField] private float rotationDamping = 5f;
    [SerializeField] private float rotationFactor = 0.5f; // Camera rotates by this factor of player rotation
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    
    private Quaternion lastPlayerRotation;
    private Quaternion targetCameraRotation;
    
    private void Start()
    {
        if (playerTransform != null)
        {
            lastPlayerRotation = playerTransform.rotation;
            targetCameraRotation = transform.rotation;
        }
    }
    
    private void LateUpdate()
    {
        if (playerTransform == null)
            return;
            
        // Follow position exactly
        transform.position = playerTransform.position + offset;
        
        // Check if player has rotated
        if (Quaternion.Angle(lastPlayerRotation, playerTransform.rotation) > 0.1f)
        {
            // Calculate rotation difference
            Quaternion rotationDifference = playerTransform.rotation * Quaternion.Inverse(lastPlayerRotation);
            
            // Convert to angle-axis representation
            float angle;
            Vector3 axis;
            rotationDifference.ToAngleAxis(out angle, out axis);
            
            // Scale the angle by our factor (0.5 = half rotation)
            angle *= rotationFactor;
            
            // Create new rotation with scaled angle
            Quaternion scaledRotation = Quaternion.AngleAxis(angle, axis);
            
            // Apply this scaled rotation to our current target
            targetCameraRotation = scaledRotation * targetCameraRotation;
            
            // Update last player rotation
            lastPlayerRotation = playerTransform.rotation;
        }
        
        // Apply damping to smoothly move to target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCameraRotation, 
                                            rotationDamping * Time.deltaTime);
    }
    
    public void SetPlayer(Transform player)
    {
        playerTransform = player;
        if (playerTransform != null)
            lastPlayerRotation = playerTransform.rotation;
    }
}
