using UnityEngine;

public class BetweenScenes : MonoBehaviour {
    // called immediately
    private void Awake () {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("Background Music");
        if (musicObj.Length > 1) { //If true 2 tracks are playing on top of each other
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
