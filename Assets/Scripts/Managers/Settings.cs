using System.Collections;
using UnityEngine;

public class Settings : MonoBehaviour {
    
    // globals
    [SerializeField] private bool showFps;
    [SerializeField] private int targetFps;
    [SerializeField] private bool playAudio;
    [SerializeField] private float audioLevel;
    [SerializeField] private bool autoStartRounds;
    
    private GameObject fpsCounter;
    public Camera mainCamera;
    public Canvas fpsCounterCanvas;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        
        Application.targetFrameRate = 60;
        autoStartRounds = false;
        targetFps = 60;
        fpsCounter = gameObject.transform.GetChild(0).GetChild(0).gameObject;
        fpsCounter.SetActive(false);
    }

    // Shows the values of the frams persecond by checking the bool value of val
    public void setShowFps(bool val) {
        showFps = val;
        Debug.Log("Show Fps: " + showFps);

        if (showFps)
            fpsCounter.SetActive(true);
        else
            fpsCounter.SetActive(false);
    }
    
    // Sets the target frams per seconds in the setting screen
    public void setTargetFps(int val) {
        targetFps = val;
        Debug.Log("Set Target FPS: " + targetFps);
        Application.targetFrameRate = val;
        
    }

    // Sets the play audio value by setting the playAudio val to a bool value 
    public void setPlayAudio(bool val) {
        playAudio = val;
        Debug.Log("Set Play Audio: " + playAudio);
    }
    // This method set the audio level to a float value
    public void setAudioLevel(float val) {
        audioLevel = val;
        Debug.Log("Set Audio Level: " + audioLevel);
    }
    // Sets the autoStartValue to a true or false value
    public void setAutoStartRounds(bool val) {
        autoStartRounds = val;
        Debug.Log("Auto Start Rounds: " + autoStartRounds);
    }
    // This method return a bool value for showFps
    public bool getShowFps() {
        return showFps;
    }
    // return an integer for the Fps 
    public int getTargetFps() {
        return targetFps;
    }
    // return a bool value that was set for playAudio
    public bool getPlayAudio() {
        return playAudio;
    }
    // return an int for the audio level
    public int getAudioLevel() {
        return (int)audioLevel;
    }
    // return a bool value for autoSTartRounds
    public bool getAutoStartRounds() {
        return autoStartRounds;
    }
    // This method saves the setting after the user made changes
    public void saveSettings() {
        initPrefs();
        
        PlayerPrefs.SetInt("ShowFps", getShowFps() ? 1 : 0);
        PlayerPrefs.SetInt("PlayAudio", getPlayAudio() ? 1 : 0);
        PlayerPrefs.SetInt("TargetFps", getTargetFps());
        PlayerPrefs.SetFloat("AudioLevel", getAudioLevel());
    }
    // This method loads those new changes
    public void loadSettings() {
        initPrefs();
        
        setShowFps(PlayerPrefs.GetInt("ShowFps") != 0);
        setPlayAudio(PlayerPrefs.GetInt("PlayAudio") != 0);
        
        var fps = PlayerPrefs.GetInt("TargetFps");
        setTargetFps(fps is >= 5 and <= 60 ? fps : 60);
        
        var level = PlayerPrefs.GetFloat("AudioLevel");
        setAudioLevel(level is >= 0f and <= 100f ? level : 100);
    }

    private void initPrefs() {
        if (!PlayerPrefs.HasKey("ShowFps"))
            PlayerPrefs.SetInt("ShowFps", 0);
        if (!PlayerPrefs.HasKey("PlayAudio"))
            PlayerPrefs.SetInt("PlayAudio", 0);
        if (!PlayerPrefs.HasKey("TargetFps"))
            PlayerPrefs.SetInt("TargetFps", 60);
        if (!PlayerPrefs.HasKey("AudioLevel"))
            PlayerPrefs.SetFloat("AudioLevel", 100f);
    }
    // Gets a new Camera
    public void getNewCamera() {
        StartCoroutine(GetCamera());
    }

    private IEnumerator GetCamera() {
        while (!mainCamera) {
            mainCamera = Camera.main;
            fpsCounterCanvas.worldCamera = mainCamera;
            fpsCounterCanvas.sortingLayerName = "UI_Top";
            fpsCounterCanvas.sortingOrder = 10;
            break;
        }
        
        Debug.Log("Settings : Camera found");
        yield return null;
    }
}
