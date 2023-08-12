//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public Text[] textObjects;
    public TextStrings[] textStrings;

    private int currentLanguage = 0;

    private void Start()
    {
        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].text = textStrings[currentLanguage].textList[i];
        }
    }

    // Use this method to switch text language
    public void SwitchLanguage()
    {
        currentLanguage++;
        if (currentLanguage >= textStrings.Length)
        {
            currentLanguage = 0;
        }

        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].text = textStrings[currentLanguage].textList[i];
        }
    }
}

[System.Serializable]
public class TextStrings
{
    public string language;
    public List<string> textList = new List<string>();
}