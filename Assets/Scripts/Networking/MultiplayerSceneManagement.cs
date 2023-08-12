using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSceneManagement : MonoBehaviour
{
    public void LoadNetScene()
    {
        SceneManager.LoadScene("NetCodeScene");
    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}