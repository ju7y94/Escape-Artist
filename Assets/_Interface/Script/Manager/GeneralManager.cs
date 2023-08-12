//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralManager : MonoBehaviour
{
    public void LoadScene (string s)
    {   
        SceneManager.LoadScene(s);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
