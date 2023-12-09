using TMPro;
using UnityEngine;

public class Slider : MonoBehaviour {
    
    [SerializeField] private Settings settingsManager;
    [SerializeField] private GameObject label;
    [SerializeField] private UnityEngine.UI.Slider slider;
    
    public bool isAudio;
    public bool isFPS;

    void Start() {
        if (GameObject.Find("SettingsManager"))
            settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
        else
            Debug.Log("Unable to find SettingsManager in current Scene");
        
        if (isAudio)
            slider.value = settingsManager.getAudioLevel();
        
        if (isFPS)
            slider.value = settingsManager.getTargetFps();
    }

    // update label text
    public void updateText() {
        var textComponent = label.GetComponent<TextMeshProUGUI>();
        if (!textComponent) {
            Debug.Log("Unable to get the TextMeshPro component of the label");
            return;
        }

        label.GetComponent<TextMeshProUGUI>().SetText(Mathf.RoundToInt(slider.value).ToString());
    }

    // update settings
    public void updateSetting() {
        if (settingsManager) {
            if (gameObject.name == "TargetFpsSlider")
                settingsManager.setTargetFps(Mathf.RoundToInt(slider.value));
            if (gameObject.name == "AudioLevelSlider")
                settingsManager.setAudioLevel(Mathf.RoundToInt(slider.value));
        }
    }
}
