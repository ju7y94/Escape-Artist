//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;

public class ChangeVersion : MonoBehaviour
{
    [Header ("Version Text Array :")]
    [SerializeField] private Text[] UIVersion;
        
    [Header ("Developer Options :")]
    [SerializeField] private GameObject[] DeveloperBuild;
    [SerializeField] private bool isDevBuild;
    
    [Header ("Version Text Field :")]
    [SerializeField] private string VersionNumber;
    [SerializeField] private string GameName;
    
    // Update is called once per frame
    void Start()
    {
        VersionUpdate();
        ToggleDev();
    }
    void VersionUpdate()
    {
        for (int i = 0; i < UIVersion.Length; i++)
        {
            UIVersion[i].text = GameName + " " + VersionNumber;
        }
    }
    void ToggleDev()
    {
        if (isDevBuild)
        {
            for (int i = 0; i < DeveloperBuild.Length; i++)
            {
                DeveloperBuild[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < DeveloperBuild.Length; i++)
            {
                DeveloperBuild[i].SetActive(false);
            }
        }
    }
}