using System;
using System.Collections;
using System.Collections.Generic;
using IniParser;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Settings : MonoBehaviour {
    
    // globals
    [SerializeField]
    private bool showFps;
    
    [SerializeField]
    private int targetFps;
    
    [SerializeField]
    private bool playAudio;
    
    [SerializeField]
    private float audioLevel;
    
    [SerializeField]
    private bool autoStartRounds;
    
    private GameObject fpsCounter;
    
    public SettingsHandler settingsHandler;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        
        Application.targetFrameRate = 60;
        autoStartRounds = false;
        targetFps = 60;
        fpsCounter = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        fpsCounter.SetActive(false);
        settingsHandler = new SettingsHandler();
    }

    public void setShowFps(bool val) {
        showFps = val;
        Debug.Log("Show Fps: " + showFps);

        if (showFps)
            fpsCounter.SetActive(true);
        else
            fpsCounter.SetActive(false);
    }

    public void setTargetFps(int val) {
        targetFps = val;
        Debug.Log("Set Target FPS: " + targetFps);
        Application.targetFrameRate = val;
    }

    public void setPlayAudio(bool val) {
        playAudio = val;
        Debug.Log("Set Play Audio: " + playAudio);
    }

    public void setAudioLevel(float val) {
        audioLevel = val;
        Debug.Log("Set Audio Level: " + audioLevel);
    }

    public void setAutoStartRounds(bool val) {
        autoStartRounds = val;
        Debug.Log("Auto Start Rounds: " + autoStartRounds);
    }

    public bool getShowFps() {
        return showFps;
    }

    public int getTargetFps() {
        return targetFps;
    }

    public bool getPlayAudio() {
        return playAudio;
    }

    public int getAudioLevel() {
        return (int)audioLevel;
    }
    
    public bool getAutoStartRounds() {
        return autoStartRounds;
    }

    public void saveSettings() {
        settingsHandler.writeSettings(getShowFps(), getPlayAudio(), getTargetFps(), getAudioLevel());
    }
}
