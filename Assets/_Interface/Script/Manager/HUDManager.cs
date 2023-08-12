//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class UICategory
{
    public string categoryName;
    public GameObject[] uiElements;
}

public class HUDManager : MonoBehaviour
{
    public bool isVisible = true;

    public UICategory[] uiCategories; // array of UI categories

    private void Start()
    {
        ToggleUI();
    }

    public void ToggleUI()
    {
        isVisible = !isVisible;

        foreach (var category in uiCategories)
        {
            foreach (var ui in category.uiElements)
            {
                ui.SetActive(isVisible);
            }
        }
    }

    public void ToggleUICategory(int index)
    {
        if (index >= 0 && index < uiCategories.Length)
        {
            foreach (var ui in uiCategories[index].uiElements)
            {
                ui.SetActive(!ui.activeSelf);
            }
        }
    }
}