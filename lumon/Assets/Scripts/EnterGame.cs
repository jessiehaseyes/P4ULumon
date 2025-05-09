using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterGame : MonoBehaviour
{
    public void ChangeScene(string newScene)
    {
        Debug.Log("loading scene");
        Debug.Log("Changing to " + newScene);
        SceneManager.LoadScene("ll");
    }
}
