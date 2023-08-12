    // Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AudioCategory
{
    public string name;
    public AudioSource[] audioSources;
    public Slider slider;
    public bool isMuted = false;
    [HideInInspector] public float[] previousVolumes;
    public Button muteButton;

    public void OnMuteButtonClicked()
    {
        if (isMuted) // If the category is currently muted, unmute it
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume = previousVolumes[i];
            }
            slider.value = previousVolumes[0];
            isMuted = false;
        }
        else // If the category is currently unmuted, mute it
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                previousVolumes[i] = audioSources[i].volume;
                audioSources[i].volume = 0;
            }
            slider.value = 0;
            isMuted = true;
        }
    }
}

public class AudioManager : MonoBehaviour
{
    public AudioCategory[] audioCategories;
    public Slider totalVolumeSlider;
    public Button muteButton;

    private bool isMuted = false;

    void Start()
    {
        // Initialize previousVolumes, set the volume sliders to their initial values.
        // Set isMuted to false for all audio categories.
        foreach (AudioCategory audioCategory in audioCategories)
        {
            audioCategory.previousVolumes = new float[audioCategory.audioSources.Length];

            for (int i = 0; i < audioCategory.audioSources.Length; i++)
            {
                audioCategory.slider.value = audioCategory.audioSources[i].volume;
                audioCategory.previousVolumes[i] = audioCategory.audioSources[i].volume;
            }

            audioCategory.isMuted = false;

            // Add a listener to the mute button for this audio category
            audioCategory.muteButton.onClick.AddListener(MuteAudio);
        }
    }

    public void AdjustVolume(AudioCategory audioCategory)
    {
        // Set the volume of each audio source in the category to the value of the corresponding volume slider.
        foreach (AudioSource audioSource in audioCategory.audioSources)
        {
            audioSource.volume = audioCategory.slider.value;
        }

        // If audio is muted, unmute it when the volume is adjusted.
        if (isMuted && audioCategory.slider.value > 0)
        {
            isMuted = false;
            muteButton.interactable = true;
        }
    }

    public void AdjustTotalVolume()
    {
        // Set the master volume of the game to the value of the total volume slider.
        AudioListener.volume = totalVolumeSlider.value;

        // If audio is muted, unmute it when the volume is adjusted.
        if (isMuted && AudioListener.volume > 0)
        {
            isMuted = false;
            muteButton.interactable = true;
        }
    }

    public void MuteAudio()
    {
        // Mute or unmute all categories and the master volume when the mute button is clicked
        // and track the mute state of all categories.
        isMuted = !isMuted;

        foreach (AudioCategory audioCategory in audioCategories)
        {
            audioCategory.isMuted = isMuted;

            if (isMuted)
            {
                // Mute audio sources and record their previous volumes.
                audioCategory.previousVolumes = new float[audioCategory.audioSources.Length];

                for (int i = 0; i < audioCategory.audioSources.Length; i++)
                {
                    audioCategory.previousVolumes[i] = audioCategory.audioSources[i].volume;
                    audioCategory.audioSources[i].volume = 0;
                }

                audioCategory.slider.value = 0;
            }
            else
            {
                // Un-mute audio sources and set their volumes to previous values.
                for (int i = 0; i < audioCategory.audioSources.Length; i++)
                {
                    audioCategory.audioSources[i].volume = audioCategory.previousVolumes[i];
                }

                audioCategory.slider.value = audioCategory.previousVolumes[0];
            }
        }

        // Set the master volume of the game to the value of the total volume slider.
        AudioListener.volume = isMuted ? 0 : totalVolumeSlider.value;
        totalVolumeSlider.value = AudioListener.volume;

        // Set the interactable state of the mute button based on the mute state.
        muteButton.interactable = !isMuted;
    }
}