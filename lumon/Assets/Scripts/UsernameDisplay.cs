using UnityEngine;
using TMPro;

public class UsernameDisplay : MonoBehaviour
{
    private Camera mainCamera;
    public TextMeshPro playerNameText;

    void Awake()
    {
        mainCamera = Camera.main;
    }
    void Start()
    {
        
        if (playerNameText == null)
        {
            playerNameText = GetComponent<TextMeshPro>();
        }
        
        // Get both names from GameManager and display them together
        if (NameManager.Instance != null && playerNameText != null)
        {
            string fullName = NameManager.Instance.playerName + " " + NameManager.Instance.playerLastName;
            playerNameText.text = fullName;
        }
    }
    
    void LateUpdate()  // Changed from Update to LateUpdate for smoother camera following
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null) return;
        }
        
        // Billboard effect - always face camera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
                         

    }
}