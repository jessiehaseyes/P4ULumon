using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public SubmitName firstnamer;

    public LastSubmitName lastnamer;

    public TextMeshProUGUI titleText;

    public TextMeshProUGUI firstText;

    public TextMeshProUGUI lastText;

    public TMP_InputField firstinputField;

    public TMP_InputField lastinputField;

    public Button submitButton;

    public Button startButton;

    public string welcomeText = "Welcome <player>, Praise Kier!";

    public void Start()
    {
        startButton.gameObject.SetActive(false);
    }    
        public void StartQuest()
    {
        welcomeText = welcomeText.Replace("<player>", firstnamer.playerfirstName + " " + lastnamer.playerlastName + ".");
        titleText.text = welcomeText;
        firstText.gameObject.SetActive(false);
        lastText.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        firstinputField.gameObject.SetActive(false);
        lastinputField.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }
}
