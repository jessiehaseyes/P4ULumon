using TMPro;
using UnityEngine;
using UnityEngine. SceneManagement;

public class SubmitName : MonoBehaviour
{
    public TMP_InputField firstNameInputField; 
    public TMP_InputField lastNameInputField;// Change to TMP_InputField instead of TextMeshProUGUI
    public string playerfirstName = "";
    
    public void PlayerFirstNamer()
    {
        // Save the name to the GameManager when submitted
        playerfirstName = firstNameInputField.text;
        NameManager.Instance.playerName = firstNameInputField.text;
        NameManager.Instance.playerLastName = lastNameInputField.text;
        Debug.Log("Player name saved: " + firstNameInputField.text + " " + lastNameInputField.text);
        
    }
}
