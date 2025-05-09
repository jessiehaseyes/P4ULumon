using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    // References
    private JJGameManager gameManager;
    
    // Visual representation of door state
    public GameObject closedDoorVisual;
    public GameObject openDoorVisual;
    
    // Optional components for effects
    public AudioSource doorSound;
    
    // Collider that enables player to pass through when open
    public Collider exitCollider;
    
    void Start()
    {
        // Find the game manager
        gameManager = FindObjectOfType<JJGameManager>();
        
        // Ensure door starts closed
        CloseDoor();
    }
    
    public void OpenDoor()
    {
        // Change appearance of the door
        if (closedDoorVisual != null)
            closedDoorVisual.SetActive(false);
            
        if (openDoorVisual != null)
            openDoorVisual.SetActive(true);
        
        // Enable the exit collider
        if (exitCollider != null)
            exitCollider.enabled = true;
            
        // Play sound if available
        if (doorSound != null)
            doorSound.Play();
    }
    
    public void CloseDoor()
    {
        // Reverse the open state
        if (closedDoorVisual != null)
            closedDoorVisual.SetActive(true);
            
        if (openDoorVisual != null)
            openDoorVisual.SetActive(false);
        
        // Disable the exit collider
        if (exitCollider != null)
            exitCollider.enabled = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // Check if player entered the exit while it's open
        if (other.CompareTag("Player"))
        {
            gameManager.PlayerReachedExit();
        }
    }
}
