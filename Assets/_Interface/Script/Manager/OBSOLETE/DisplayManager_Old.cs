//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;

public class DisplayManager_Old : MonoBehaviour {
    
    public Dropdown resolutionDropdown;

    void Start() {
        // Populate the dropdown with available resolutions
        foreach (Resolution resolution in Screen.resolutions) {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.width + " x " + resolution.height));
        }

        // Set the initial value of the dropdown to the current resolution
        resolutionDropdown.value = Screen.resolutions.Length - 1;

        // Register the OnDropdownValueChanged method to be called when the dropdown value changes
        resolutionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void OnDropdownValueChanged(int value) {
        // Set the resolution to the selected resolution
        Resolution resolution = Screen.resolutions[value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        // Update the dropdown text to show the current resolution
        resolutionDropdown.captionText.text = resolution.width + " x " + resolution.height;
    }

    private void SetFullscreen(bool fullscreen) {
        Screen.fullScreen = fullscreen;
    }

    public void SetWindowed() {
        SetFullscreen(false);
        Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void SetWindowedBorderless() {
        SetFullscreen(false);
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
    }
}