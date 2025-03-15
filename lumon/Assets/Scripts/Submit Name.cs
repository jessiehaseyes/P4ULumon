using TMPro;
using UnityEngine;

public class SubmitName : MonoBehaviour
{
    public TextMeshProUGUI inputfield;
    public string playerfirstName = "";

    // Update is called once per frame
    void Update()
    {
        Debug.Log("The current name is:" + inputfield.text);
    }

    public void PlayerFirstNamer()
    {
        playerfirstName = inputfield.text;
        Debug.Log("The player name is:" + playerfirstName);
    }
}
