//Script written by Leith Abdul-Hussain [001104598] (C) GameMasters Inc. 2023

using UnityEngine;
using UnityEngine.UI;

public class FontManager : MonoBehaviour {
    
    public Font[] fonts;
    public Text[] texts;
    public Slider textSizeSlider;

    void Start() {
        for (int i = 0; i < texts.Length; i++) {
            texts[i].font = fonts[i];
        }

        // Register the OnSliderValueChanged method to be called when the slider value changes
        textSizeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value) {
        for (int i = 0; i < texts.Length; i++) {
            texts[i].fontSize = Mathf.RoundToInt(value);
        }
    }
}