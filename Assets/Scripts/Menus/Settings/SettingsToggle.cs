using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour {
    
    // globals
    [SerializeField]
    private Settings settingsManager;
    
    [SerializeField]
    private UnityEngine.UI.Toggle baseToggle;
    
    public bool isAudio;
    public bool isFPS;
    
    void Awake() {
        if (baseToggle == null)
            baseToggle = gameObject.GetComponent<UnityEngine.UI.Toggle>();
        
        if (GameObject.Find("SettingsManager"))
            settingsManager = GameObject.Find("SettingsManager").GetComponent<Settings>();
        else
            Debug.Log("Unable to find SettingsManager in current Scene");
        
        if (isFPS)
            baseToggle.isOn = settingsManager.getShowFps();
        if (isAudio)
            baseToggle.isOn = settingsManager.getPlayAudio();
    }

    // checkbox has been toggled
    public void Toggled(bool toggled) {
        if (settingsManager != null) {
            if (gameObject.name == "ShowFpsToggle")
                settingsManager.setShowFps(baseToggle.isOn);
            else if (gameObject.name == "PlayAudioToggle")
                settingsManager.setPlayAudio(baseToggle.isOn);
        }
    }
}
