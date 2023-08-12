using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitApp : MonoBehaviour
{
    [SerializeField] Button quitButton;
    void Awake()
    {
        quitButton.onClick.AddListener(()=> {
            Application.Quit();
        });
    }

}
