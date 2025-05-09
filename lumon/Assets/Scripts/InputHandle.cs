using UnityEngine;

using UnityEngine;
using StarterAssets;
public class InputHandle : MonoBehaviour
{
  // Reference to the input system
    private StarterAssetsInputs starterAssetsInputs;
    
    // Previous state tracking (to detect changes from 1 to 0)
    private int prevFirstValue = 1;
    
    void Start()
    {
        // Find the StarterAssetsInputs component in the scene
        starterAssetsInputs = FindObjectOfType<StarterAssetsInputs>();
        
        if (starterAssetsInputs == null)
        {
            Debug.LogError("StarterAssetsInputs component not found in scene!");
        }
    }
    
    // This method should be called whenever you receive new serial data
    public void ProcessSerialInput(string input)
    {
        // Parse the input string (format: "1 1 1 1")
        string[] values = input.Trim().Split(' ');
        
        if (values.Length >= 4)
        {
            // Try to parse each value
            if (int.TryParse(values[0], out int firstValue) &&
                int.TryParse(values[1], out int secondValue) &&
                int.TryParse(values[2], out int thirdValue) &&
                int.TryParse(values[3], out int fourthValue))
            {
                // Detect falling edge (1 to 0) for S key
                if (prevFirstValue == 1 && firstValue == 0)
                {
                    // S key (back)
                    starterAssetsInputs.move = new Vector2(0, -1);
                    Debug.Log("S key pressed from serial");
                }
                else if (secondValue == 0)
                {
                    // W key (forward)
                    starterAssetsInputs.move = new Vector2(0, 1);
                    Debug.Log("W key pressed from serial");
                }
                else if (thirdValue == 0)
                {
                    // D key (right)
                    starterAssetsInputs.move = new Vector2(1, 0);
                    Debug.Log("D key pressed from serial");
                }
                else if (fourthValue == 0)
                {
                    // A key (left)
                    starterAssetsInputs.move = new Vector2(-1, 0);
                    Debug.Log("A key pressed from serial");
                }
                else
                {
                    // No keys pressed, stop movement
                    starterAssetsInputs.move = Vector2.zero;
                }
                
                // Save current state for next comparison
                prevFirstValue = firstValue;
            }
        }
    }
}
