using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    
    // globals
    [SerializeField] private Level currentLevel;
    [SerializeField] private Text disasterNotificationText;
    [SerializeField] private GameObject notif;
    [SerializeField] private GameObject checkpointObject;
    [SerializeField] private List<Vector3> checkpoints;
    [SerializeField] private GameObject levelUI;
    
    private bool inLevel;
    private MainManager mainManager;
    
    public float roundDelay;
    public float roundCountdown;
    
    void Start() {
        DontDestroyOnLoad(this);
        roundCountdown = roundDelay;
        mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
        disasterNotificationText = notif.GetComponent<Text>();
        levelUI.SetActive(false);
        inLevel = false;
        roundDelay = 5f;
    }

    public void loadLevel(Level level) {
        currentLevel = level;
        levelUI.SetActive(true);
        currentLevel.loadCheckpoints();
        checkpoints = currentLevel.getCheckpoints();
        foreach (var round in currentLevel.rounds)
	        round.init();
        inLevel = true;
    }

    public void exitLevel() {
	    currentLevel = null;
	    levelUI.SetActive(false);
    }

    public Level getCurrentLevel() {
        return currentLevel;
    }

    public void displayDisasterNotification(string str) {
        disasterNotificationText.text = "WARNING:    " + str + " incoming!";
    }
    
	public void removeEnemy(int enemy) {
		currentLevel.rounds[currentLevel.currentRound].enemies.remove(enemy);
	}

	void Update() {
		if (inLevel) {
			if (currentLevel.roundState == RoundState.InProgress) {
				if (!hasEnemies())
					endRound();
			}
		}
	}

	public void startRound() {
		if (currentLevel.roundState == RoundState.Waiting) {
			StartCoroutine(spawnRound(currentLevel.rounds[currentLevel.currentRound]));
		}
		else
			Debug.Log("Round already started");
	}

	void endRound() {
		roundCountdown = roundDelay;
		
		currentLevel.rounds[currentLevel.currentRound].enemies.clear();

		if (currentLevel.currentRound + 1 >= currentLevel.rounds.Length) {
			currentLevel.currentRound = 0;
			Debug.Log("All rounds completed. Looping to first round");
			foreach (var round in currentLevel.rounds)
				round.isComplete = false;
		}
		else
			currentLevel.rounds[currentLevel.currentRound++].isComplete = true;

		currentLevel.roundState = RoundState.Waiting;
		Debug.Log("Current Level new Round State: WAITING");
		
		if (mainManager.getSettingsManager().getAutoStartRounds())
			startRound();
	}

	bool hasEnemies() {
		return !currentLevel.rounds[currentLevel.currentRound].enemies.IsEmpty;
	}

	IEnumerator spawnRound(Round curRound) {
		Debug.Log("Spawning Round: " + currentLevel.currentRound);
		currentLevel.roundState = RoundState.Spawning;
		Debug.Log("Current Level new Round State: SPAWNING");

		for (int i = 0; i < curRound.enemyCount; i++) {
			spawnEnemy(curRound.enemy);
			yield return new WaitForSeconds(1f / curRound.spawnRate);
		}
		
		currentLevel.roundState = RoundState.InProgress;
		Debug.Log("Current Level new Round State: IN PROGRESs");
	}

	void spawnEnemy(Transform enemyToSpawn) {
		var tempEnemy = Instantiate(enemyToSpawn, currentLevel.getCheckpoints()[0], Quaternion.identity);
		tempEnemy.gameObject.GetComponent<Enemy>().setWaypoints(currentLevel.getCheckpoints());
		currentLevel.rounds[currentLevel.currentRound].enemies.enqueue(tempEnemy.gameObject.GetInstanceID());
	}
}
