﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    
    // ui
    [SerializeField] public GameObject disasterPrompt;
    [SerializeField] private GameObject levelUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject roundCounter;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject startRoundButton;
    
    // event stuff
    public enum EventState { NoEvent, Event1, Event2 };
    public EventState eventState;
    public EventState futureEvent;
    public int eventType;
    
    // player interaction
    public PlayerManager playerManager;
    public bool levelWon;
    
    // disasters
    [SerializeField] private DisasterEffectHandler disasterEffectHandler;
    [SerializeField] private Vector3 event1Location, event2Location;
    [SerializeField] private float disasterDestructionRange;

    void Awake() {
        mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
        playerManager = GameObject.Find("PlayerManager").gameObject.GetComponent<PlayerManager>();
        
        GameObject.FindWithTag("PauseMenu").GetComponent<PauseMenuHandler>().getNewCamera();
        
        resetLevel();
    }
    
	public void removeEnemy(int enemy) {
		Debug.Log("Remove enemy called");
		var ret = !rounds[currentRound].enemies.remove(enemy);
		if (ret) {
			Debug.Log("Unable to remove enemy from current round list: " + ret);
		}
	}

	public void switchButtons() {
		if (pauseButton.activeSelf) {
			pauseButton.SetActive(false);
			startRoundButton.SetActive(true);
		}
		else {
			pauseButton.SetActive(true);
			startRoundButton.SetActive(false);
		}
	}

	void Update() {
		if (currentRound == disasterRound) {
			Debug.Log("PLAYING DISASTER!!!");
			eventState = futureEvent;
			triggerEvent();
			disasterRound = -2;
		}
		if (currentRound != 0 && (currentRound + 1) % 3 == 0 && disasterRound == -1) {
			Debug.Log("Checking for disaster.");
			if (Random.Range(1, 101) < eventTriggerThreshold) {
				Debug.Log("We have a disaster incoming");
				showDisasterPrompt();
				disasterRound = currentRound + 3;
			}
		}
		if (roundState == RoundState.InProgress) {
			if (hasEnemies())
				endRound();
		}
	}

	
	public bool forceStart = true;
	public void startRound() {
		if (roundState == RoundState.Waiting) {
			forceStart = false;
			roundState = RoundState.Spawning;
			StartCoroutine(spawnRound(rounds[currentRound]));
			setRoundCounter();
			startRoundButton.GetComponent<Button>().enabled = false;
			//switchButtons();
			Debug.Log("End of startRound()");
		}
		else
			Debug.Log("Round already started");
	}

	void endRound() {
		Debug.Log("End Round called");
		startRoundButton.GetComponent<Button>().enabled = true;
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
		
		//switchButtons();
		
		if (mainManager.getSettingsManager().getAutoStartRounds())
			startRound();
	}

	bool hasEnemies() {
		//Debug.Log("total enemies: " + rounds[currentRound].enemies.Count);
		//var enemyList = GameObject.FindGameObjectsWithTag("Enemy");
		return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
		//return !rounds[currentRound].enemies.IsEmpty;
	}

	IEnumerator spawnRound(Round curRound) {
		Debug.Log("Spawning Round: " + currentRound);
		
		Debug.Log("Current Level new Round State: SPAWNING");

		if (curRound.subRounds.Length == 0) {
			Debug.Log("Rounds are empty, returning");
			roundState = RoundState.Waiting;
			yield return null;
		}

		foreach (var round in curRound.subRounds)
			StartCoroutine(spawnSubRound(round));
		
		roundState = RoundState.InProgress;
		Debug.Log("Current Level new Round State: IN PROGRESS");

		yield return null;
	}

	IEnumerator spawnSubRound(Round curRound) {
		Debug.Log("Spawning sub round with start delay:" + curRound.startDelay);
		if (curRound.startDelay != 0)
			yield return new WaitForSeconds(curRound.startDelay + 2f);
		Debug.Log("Round ready to spawn");
		
		for (int i = 0; i < curRound.enemyCount; i++) {
			spawnEnemy(curRound.enemy);
			yield return new WaitForSeconds(curRound.spawnRate + 0.2f);
		}
	}

	private void spawnEnemy(Transform enemyToSpawn) {
		var tempEnemy = Instantiate(enemyToSpawn, getCheckpoints()[0], Quaternion.identity);
		rounds[currentRound].enemies.enqueue(tempEnemy.gameObject.GetInstanceID());
		var enemyInstance = tempEnemy.gameObject.GetComponent<Enemy>();
		enemyInstance.setWaypoints(getCheckpoints());
		enemyInstance.levelManager = GetComponent<LevelManager>();
	}

	private void showDisasterPrompt() {
		disasterPrompt.SetActive(true);
		mainManager.getStateManager().showDisasterPrompt();
	}

	public void acceptDisasterPrompt() {
		disasterPrompt.SetActive(false);
		mainManager.getStateManager().hideDisasterPrompt();
	}

	private List<Vector3> getCheckpoints() {
		return eventState switch {
			EventState.Event1 => checkpointsEvent1,
			EventState.Event2 => checkpointsEvent2,
			_ => checkpointsNoEvent
		};
	}

	private void loadCheckpoints() {
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

	private void triggerEvent() {
		if (eventState == EventState.Event1) {
			if (eventType == 1)
				disasterEffectHandler.meteorDisaster(event1Location, 3f);
			else 
				disasterEffectHandler.earthQuakeDisaster(event1Location, 3f);
			
			StartCoroutine(switchMap(3.1f, 1));
		}
		if (eventState == EventState.Event2) {
			if (eventType == 1)
				disasterEffectHandler.meteorDisaster(event2Location, 3f);
			else 
				disasterEffectHandler.earthQuakeDisaster(event2Location, 3f);
			
			StartCoroutine(switchMap(3.1f, 2));
		}
	}

	private IEnumerator switchMap(float duration, int eventNum) {
		yield return new WaitForSeconds(duration);

		// here we need to switch the map and also do a raycast to remove all enemies and towers within that raycast
		
		switch (eventNum) {
			case 1:
				destroyWithinRadius(event1Location, disasterDestructionRange);
			
				mapNoEvent.SetActive(false);
				mapEvent1.SetActive(true);
				mapEvent2.SetActive(false);
				break;
			case 2:
				destroyWithinRadius(event2Location, disasterDestructionRange);

				mapNoEvent.SetActive(false);
				mapEvent1.SetActive(false);
				mapEvent2.SetActive(true);
				break;
		}
	}

	private void destroyWithinRadius(Vector3 target, float range) {
		var transforms = new List<Transform>();
		foreach (var colr in Physics2D.OverlapCircleAll(transform.position, range)) {
			if (colr.gameObject.CompareTag("Tower"))
				transforms.Add(colr.gameObject.transform);
			else if (colr.gameObject.CompareTag("Enemy"))
				transforms.Add(colr.gameObject.transform);
		}

		if (transforms.Count <= 0) return;
		
		for (var i = transforms.Count - 1; i >= 0; i--)
			Destroy(transforms[i].gameObject);

	}

	private void loadMaps() {
		mapNoEvent.SetActive(true);
		mapEvent1.SetActive(false);
		mapEvent2.SetActive(false);
	}

	private void resetLevel() {
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (var t in enemies)
			Destroy(t);

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
        
		futureEvent = Random.Range(1, 3) == 1 ? EventState.Event1 : EventState.Event2;
		
		winUI.SetActive(false);
		loseUI.SetActive(false);
		
		pauseButton.SetActive(false);
		startRoundButton.SetActive(true);
	}

	public void endLevel(bool condition) {
		Time.timeScale = 0;
		if (condition) {
			levelWon = true;
			mainManager.completeLevel(levelIndex);
			winUI.SetActive(true);
			loseUI.SetActive(false);
		}
		else {
			levelWon = false;
			winUI.SetActive(false);
			loseUI.SetActive(true);
		}
	}

	private void setRoundCounter() {
		roundCounter.GetComponent<TextMeshProUGUI>().SetText("Round: " + currentRound);
	}
}
