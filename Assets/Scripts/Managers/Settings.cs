using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 60;
    }

    public void setShowFps(bool val) {
        showFps = val;
        Debug.Log("Show Fps: " + showFps);
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

    public bool getShowFps() {
        return showFps;
    }

    public int getTargetFps() {
        return targetFps;
    }

    public bool getPlayAudio() {
        return playAudio;
    }

    public float getAudioLevel() {
        return audioLevel;
    }
}
