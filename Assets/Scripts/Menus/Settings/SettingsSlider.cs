using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slider : MonoBehaviour {
    
    // globals
    [SerializeField]
    private Settings settingsManager;
    
    [SerializeField]
    private GameObject label;
    
    [SerializeField]
    private UnityEngine.UI.Slider slider;

    void Start() {
        if (GameObject.Find("SettingsManager") != null)
            settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
        else
            Debug.Log("Unable to find SettingsManager in current Scene");
    }

    public void updateText() {
        var textComponent = label.GetComponent<TextMeshProUGUI>();
        if (!textComponent) {
            Debug.Log("Unable to get the TextMeshPro component of the label");
            return;
        }

        label.GetComponent<TextMeshProUGUI>().SetText(Mathf.RoundToInt(slider.value).ToString());
    }

    public void updateSetting() {
        if (settingsManager != null) {
            if (gameObject.name == "TargetFpsSlider") {
                settingsManager.setTargetFps(Mathf.RoundToInt(slider.value));
            }
            else if (gameObject.name == "AudioLevelSlider") {
                settingsManager.setAudioLevel(Mathf.RoundToInt(slider.value));
            }
        }
    }
}
