using UnityEngine;
using TMPro;
using System.Collections;

public class ExitOpen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private float countdownDuration = 30f;
    [SerializeField] private string prefixText = "Find the exit in ";
    [SerializeField] private bool hideOnComplete = true;

    private void Start()
    {
        // Make sure we have a TextMeshPro component assigned
        if (countdownText == null)
        {
            countdownText = GetComponent<TextMeshProUGUI>();
            
            if (countdownText == null)
            {
                Debug.LogError("No TextMeshProUGUI component found on this GameObject!");
                return;
            }
        }

        // Start the countdown coroutine
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        float timeRemaining = countdownDuration;
        
        while (timeRemaining > 0)
        {
            // Update the text with the current time remaining
            int seconds = Mathf.CeilToInt(timeRemaining);
            countdownText.text = prefixText + seconds.ToString() + " seconds";
            
            // Wait for the next frame
            yield return null;
            
            // Reduce the time remaining by the time elapsed since last frame
            timeRemaining -= Time.deltaTime;
        }
        
        // Show the final "0 seconds" message
        countdownText.text = prefixText + "0 seconds";
        
        // Wait a brief moment to ensure the "0 seconds" message is visible
        yield return new WaitForSeconds(0.5f);
        
        // Hide the countdown text if requested
        if (hideOnComplete)
        {
            countdownText.gameObject.SetActive(false);
        }
    }
}
