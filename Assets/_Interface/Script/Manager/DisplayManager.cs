//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ResolutionOption {
    public string label;
    public Vector2Int resolution;
    public Sprite sprite;
}

public enum ScreenMode {
    Fullscreen,
    Windowed,
    BorderlessWindowed
}

[System.Serializable]
public class ScreenOptions {

    public string label;
    public ScreenMode screenMode;

}

public class DisplayManager : MonoBehaviour {
    
    public Dropdown resolutionDropdown;
    public List<ResolutionOption> resolutionOptions;
    
    public List<ScreenOptions> screenOptions;
    public Dropdown screenOptionsDropdown;

    void Start() {
        foreach (ResolutionOption option in resolutionOptions) {
            // Create a new option with the sprite next to the resolution
            resolutionDropdown.options.Add(new Dropdown.OptionData(option.label, option.sprite));
        }

        // Set the initial value of the resolution dropdown to the current resolution
        resolutionDropdown.value = GetCurrentResolutionOptionIndex();

        // Register the OnDropdownValueChanged method to be called when the resolution dropdown value changes
        resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownValueChanged);
        
        // Create options for the screen mode menu       
        foreach (ScreenOptions option in screenOptions){
            screenOptionsDropdown.options.Add(new Dropdown.OptionData(option.label));
        }
        
        // Set the initial value of the screen options dropdown
        screenOptionsDropdown.onValueChanged.AddListener(OnScreenOptionsDropdownValueChanged);
        screenOptionsDropdown.value = 0;
    }

    void OnResolutionDropdownValueChanged(int value) {
        // Set the resolution to the selected resolution
        Vector2Int resolution = resolutionOptions[value].resolution;
        Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);

        // Update the resolution dropdown caption text to show the current resolution
        resolutionDropdown.captionText.text = resolutionOptions[value].label;
    }

    void OnScreenOptionsDropdownValueChanged(int value) {
        // Toggle the selected screen mode
        ScreenMode selectedMode = screenOptions[value].screenMode;
        switch(selectedMode) {
            case ScreenMode.Fullscreen:
                SetFullscreen(true);
                break;
            case ScreenMode.Windowed:
                SetWindowed();
                break;
            case ScreenMode.BorderlessWindowed:
                SetBorderlessWindowed();
                break;
            default:
                SetFullscreen(true);
                break;
        }    
    }

    private int GetCurrentResolutionOptionIndex() {
        Resolution currentResolution = Screen.currentResolution;
        for (int i = 0; i < resolutionOptions.Count; i++) {
            Vector2Int optionResolution = resolutionOptions[i].resolution;
            if (optionResolution.x == currentResolution.width && optionResolution.y == currentResolution.height) {
                return i;
            }
        }
        return resolutionOptions.Count - 1;
    }

    private void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
    }

    private void SetWindowed() {
        SetFullscreen(false);
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    private void SetBorderlessWindowed() {
        SetFullscreen(false);
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }

}