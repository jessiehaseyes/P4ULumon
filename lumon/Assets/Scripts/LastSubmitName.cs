using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class LastSubmitName : MonoBehaviour
{
    public TextMeshProUGUI inputfield;
    public string playerlastName = "";


    // Update is called once per frame

    void Update()
    {
        Debug.Log("The current last name is:" + inputfield.text);
    }

    public void PlayerLastNamer()
    {
        playerlastName = inputfield.text;
        Debug.Log("The player last name is:" + playerlastName);
    }
}

