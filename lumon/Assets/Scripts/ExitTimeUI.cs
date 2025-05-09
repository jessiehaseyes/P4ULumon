using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExitTimeUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject timerPanel;         // The panel containing the timer UI
    public TextMeshProUGUI timerText;     // Text to display the countdown
    public Image timerFillImage;          // Optional: fill image that depletes as time runs out

    [Header("Timer Settings")]
    public Color startColor = Color.green;
    public Color midColor = Color.yellow;
    public Color endColor = Color.red;
    public float warningThreshold = 10f;  // When to change color to yellow
    public float criticalThreshold = 5f;  // When to change color to red
    
    private bool isTimerActive = false;
    private float currentTime = 0f;
    private float maxTime = 0f;

    private void Start()
    {
        // Make sure the timer is hidden at start
        HideTimer();
    }

    private void Update()
    {
        if (isTimerActive)
        {
            // Update the countdown
            currentTime -= Time.deltaTime;
            
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                HideTimer();
            }
            
            // Update UI
            UpdateTimerDisplay();
        }
    }
    
    public void StartTimer(float duration)
    {
        maxTime = duration;
        currentTime = duration;
        isTimerActive = true;
        
        // Show and initialize the timer UI
        timerPanel.SetActive(true);
        UpdateTimerDisplay();
    }
    
    public void StopTimer()
    {
        isTimerActive = false;
        HideTimer();
    }
    
    private void HideTimer()
    {
        isTimerActive = false;
        timerPanel.SetActive(false);
    }
    
    private void UpdateTimerDisplay()
    {
        // Update the text
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = seconds.ToString();
        
        // Update the fill amount if we have a fill image
        if (timerFillImage != null)
        {
            timerFillImage.fillAmount = currentTime / maxTime;
        }
        
        // Change color based on remaining time
        if (currentTime <= criticalThreshold)
        {
            timerText.color = endColor;
            if (timerFillImage != null)
                timerFillImage.color = endColor;
        }
        else if (currentTime <= warningThreshold)
        {
            timerText.color = midColor;
            if (timerFillImage != null)
                timerFillImage.color = midColor;
        }
        else
        {
            timerText.color = startColor;
            if (timerFillImage != null)
                timerFillImage.color = startColor;
        }
    }
}
