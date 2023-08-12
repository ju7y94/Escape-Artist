//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;

public class FakeLight : MonoBehaviour
{
    [SerializeField] GameObject gameobject_DarkLight; 
    [SerializeField] GameObject gameobject_BrightLight; 
    [SerializeField] private string string_Togglelight;
    [SerializeField] private bool bool_isLightToggled = true;
    void Update()
    {
        if (Input.GetKeyDown(string_Togglelight))
        {
            bool_isLightToggled = !bool_isLightToggled;
            Togglelight();
        }
    }

    void Togglelight()
    {
        if (bool_isLightToggled)
        {
            gameobject_DarkLight.SetActive(true);
            gameobject_BrightLight.SetActive(false);
        }
        else 
        {
            gameobject_DarkLight.SetActive(false);
            gameobject_BrightLight.SetActive(true);
        }
    }
}
