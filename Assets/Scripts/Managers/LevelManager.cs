using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {
    
    // globals
    [SerializeField] private int startingCurrency;
    public int levelIndex = 0;
    
    // pathing
    [SerializeField] private List<Vector3> checkpointsNoEvent, checkpointsEvent1, checkpointsEvent2;
    [SerializeField] private GameObject checkpointObjectNoEvent, checkpointObjectEvent1, checkpointObjectEvent2;
    [SerializeField] private GameObject mapNoEvent, mapEvent1, mapEvent2;
    
    // managers
    private MainManager mainManager;
    
    // round stuff
    public float roundDelay; // old
    public float roundCountdown; // old
    
    public RoundState roundState = RoundState.Waiting;
    public Round[] rounds;
    public int currentRound = 0;
    public int disasterRound;
    
    // disaster stuff
    public int eventTriggerThreshold; // this is chance every 3 levels that the event triggers

	private Disaster disaster;
    
    // ui
    [SerializeField] public GameObject disasterPrompt;
    [SerializeField] private GameObject levelUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    
    // event stuff
    public enum EventState { NoEvent, Event1, Event2 };
    public EventState eventState;
    public EventState futureEvent;
    
    // player interaction
    public PlayerManager playerManager;
    public bool levelWon;
    
    void Awake() {
        mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
        playerManager = GameObject.Find("PlayerManager").gameObject.GetComponent<PlayerManager>();
        
        resetLevel();
    }
    
	public void removeEnemy(int enemy) {
		rounds[currentRound].enemies.remove(enemy);
	}
	
	void Update() {
		if (currentRound == disasterRound) {
			// play disaster before the round
			Debug.Log("PLAYING DISASTER!!!");
			//
			eventState = futureEvent;
			// random number for loaction to hit between 1 - 2
			int location = Random.Range(1,2);
			disaster.setLocation(location);
			triggerEvent();
			disasterRound = -2;
		}
		if (currentRound != 0 && (currentRound + 1) % 3 == 0 && disasterRound == -1) {
			Debug.Log("Checking for disaster.");
			if (Random.Range(1, 101) < eventTriggerThreshold) {
				showDisasterPrompt();
				disasterRound = currentRound + 3;
			}
		}
		if (roundState == RoundState.InProgress) {
			if (!hasEnemies())
				endRound();
		}
	}

	public void startRound() {
		if (roundState == RoundState.Waiting) {
			StartCoroutine(spawnRound(rounds[currentRound]));
		}
		else
			Debug.Log("Round already started");
	}

	void endRound() {
		roundCountdown = roundDelay;
		
		rounds[currentRound].enemies.clear();

		if (currentRound + 1 >= rounds.Length) {
			currentRound = 0;
			Debug.Log("All rounds completed. Looping to first round");
			// TODO end the game here as a win
			endLevel(true);
			foreach (var round in rounds)
				round.isComplete = false;
		}
		else
			rounds[currentRound++].isComplete = true;

		roundState = RoundState.Waiting;
		Debug.Log("Current Level new Round State: WAITING");
		
		if (mainManager.getSettingsManager().getAutoStartRounds())
			startRound();
	}

	bool hasEnemies() {
		return !rounds[currentRound].enemies.IsEmpty;
	}

	IEnumerator spawnRound(Round curRound) {
		Debug.Log("Spawning Round: " + currentRound);
		roundState = RoundState.Spawning;
		Debug.Log("Current Level new Round State: SPAWNING");

		foreach (var round in curRound.subRounds)
			StartCoroutine(spawnSubRound(round));
		
		roundState = RoundState.InProgress;
		Debug.Log("Current Level new Round State: IN PROGRESs");

		yield return null;
	}

	IEnumerator spawnSubRound(Round curRound) {
		Debug.Log("Spawning sub round");
		yield return new WaitForSeconds(curRound.startDelay);
		
		for (int i = 0; i < curRound.enemyCount; i++) {
			spawnEnemy(curRound.enemy);
			yield return new WaitForSeconds(1f / curRound.spawnRate);
		}
	}

	void spawnEnemy(Transform enemyToSpawn) {
		var tempEnemy = Instantiate(enemyToSpawn, getCheckpoints()[0], Quaternion.identity);
		tempEnemy.gameObject.GetComponent<Enemy>().setWaypoints(getCheckpoints());
		rounds[currentRound].enemies.enqueue(tempEnemy.gameObject.GetInstanceID());
	}
	
	public void showDisasterPrompt() {
		disasterPrompt.SetActive(true);
		mainManager.getStateManager().showDisasterPrompt();
	}

	public void acceptDisasterPrompt() {
		disasterPrompt.SetActive(false);
		mainManager.getStateManager().hideDisasterPrompt();
	}

	private List<Vector3> getCheckpoints() {
		if (eventState == EventState.Event1)
			return checkpointsEvent1;
		
		if (eventState == EventState.Event2)
			return checkpointsEvent2;
		
		return checkpointsNoEvent;
	}

	public void loadCheckpoints() {
		checkpointsNoEvent.Clear();
		checkpointsEvent1.Clear();
		checkpointsEvent2.Clear();
		
		foreach (Transform checkpoint in checkpointObjectNoEvent.transform) {
			var checkpointPos = checkpoint.gameObject.transform.position;
			checkpointsNoEvent.Add(new Vector3(checkpointPos.x, checkpointPos.y, 0f));
		}
		
		foreach (Transform checkpoint in checkpointObjectEvent1.transform) {
			var checkpointPos = checkpoint.gameObject.transform.position;
			checkpointsEvent1.Add(new Vector3(checkpointPos.x, checkpointPos.y, 0f));
		}
		
		foreach (Transform checkpoint in checkpointObjectEvent2.transform) {
			var checkpointPos = checkpoint.gameObject.transform.position;
			checkpointsEvent2.Add(new Vector3(checkpointPos.x, checkpointPos.y, 0f));
		}
	}

	public void triggerEvent() {
		if (eventState == EventState.Event1 && disaster.getLocationOne()) {
			mapNoEvent.SetActive(false);
			mapEvent1.SetActive(false);
			mapEvent2.SetActive(true);
		}
		else if (eventState == EventState.Event2 && disaster.getLocationTwo()) {
			mapNoEvent.SetActive(false);
			mapEvent1.SetActive(false);
			mapEvent2.SetActive(true);
		}
	}

	public void loadMaps() {
		mapNoEvent.SetActive(true);
		mapEvent1.SetActive(false);
		mapEvent2.SetActive(false);
	}

	public void resetLevel() {
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			Destroy(enemies[i]);
		}

		disasterPrompt.SetActive(false);
        
		roundCountdown = roundDelay;
		roundDelay = 5f;
		eventState = EventState.NoEvent;
		disasterRound = -1;
		levelWon = false;
        
		loadCheckpoints();
		loadMaps();
        
		foreach (var round in rounds)
			round.init();
        
		futureEvent = (Random.Range(1, 101) > 50) ? EventState.Event1 : EventState.Event2;
		
		winUI.SetActive(false);
		loseUI.SetActive(false);
	}

	public void endLevel(bool condition) {
		Time.timeScale = 0;
		if (condition) {
			levelWon = true;
			mainManager.getGameManager().completeLevel(levelIndex);
			winUI.SetActive(true);
			loseUI.SetActive(false);
		}
		else {
			levelWon = false;
			winUI.SetActive(false);
			loseUI.SetActive(true);
		}
	}
}
