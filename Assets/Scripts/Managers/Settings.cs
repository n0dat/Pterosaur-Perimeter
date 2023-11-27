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
    public SettingsHandler settingsHandler;
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
        initPrefs();
        
        PlayerPrefs.SetInt("ShowFps", getShowFps() ? 1 : 0);
        PlayerPrefs.SetInt("PlayAudio", getPlayAudio() ? 1 : 0);
        PlayerPrefs.SetInt("TargetFps", getTargetFps());
        PlayerPrefs.SetFloat("AudioLevel", getAudioLevel());
    }

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
