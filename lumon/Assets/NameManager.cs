using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance { get; private set; }
    public string playerName;
    public string playerLastName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        // Singleton pattern to preserve object between scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
