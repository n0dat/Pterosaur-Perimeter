using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {
    
    // globals
    public int levelIndex = 0;
    
    // pathing
    [SerializeField] private List<Vector3> checkpointsNoEvent, checkpointsEvent1, checkpointsEvent2;
    [SerializeField] private GameObject checkpointObjectNoEvent, checkpointObjectEvent1, checkpointObjectEvent2;
    [SerializeField] private GameObject mapNoEvent, mapEvent1, mapEvent2;
    
    // managers
    private MainManager mainManager;
    
    public RoundState roundState = RoundState.Waiting;
    public Round[] rounds;
    public int currentRound;
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
	    
	    // set managers
        mainManager = GameObject.Find("MainManager").gameObject.GetComponent<MainManager>();
        playerManager = GameObject.Find("PlayerManager").gameObject.GetComponent<PlayerManager>();
        
        GameObject.FindWithTag("PauseMenu").GetComponent<PauseMenuHandler>().getNewCamera();
        
        resetLevel();
    }

	void Update() {
		if (currentRound == disasterRound) { // disaster to play
			Debug.Log("PLAYING DISASTER!!!");
			eventState = futureEvent;
			triggerEvent();
			disasterRound = -2;
		}
		if (currentRound != 0 && (currentRound + 1) % 3 == 0 && disasterRound == -1) { // checking for a disaster
			Debug.Log("Checking for disaster.");
			if (Random.Range(1, 101) < eventTriggerThreshold) { // determined we have a disaster
				Debug.Log("We have a disaster incoming");
				showDisasterPrompt();
				disasterRound = currentRound + 3;
			}
		}
		if (roundState == RoundState.InProgress) {
			
			// end the round if we have no more enemies
			if (hasEnemies())
				endRound();
		}
	}
    
	// Starts the round
	public void startRound() {
		
		// only start round if no round is currently in progress or spawning
		if (roundState == RoundState.Waiting) {
			roundState = RoundState.Spawning;
			startRoundButton.GetComponent<Button>().enabled = false;
			setRoundCounter();
			// start spawning the current / next round
			StartCoroutine(spawnRound(rounds[currentRound]));
		}
		else
			Debug.Log("Round already started");
	}

	// end the round
	void endRound() {
		startRoundButton.GetComponent<Button>().enabled = true;
		
		// last round completed
		if (currentRound + 1 >= rounds.Length) {
			Debug.Log("All rounds completed.");
			endLevel(true); // end the level as a win
		}
		else
			rounds[currentRound++].isComplete = true;

		// set state back to waiting to be able to start the next round
		roundState = RoundState.Waiting;
        
		if (mainManager.getSettingsManager().getAutoStartRounds())
			startRound();
	}

	// check for enemies in the scene
	bool hasEnemies() {
		return GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
	}

	// spawn the round of enemies
	IEnumerator spawnRound(Round curRound) {
		Debug.Log("Spawning Round: " + currentRound);
        
		// if for some reason we didn't add the rounds in the editor, can probably remove this
		if (curRound.subRounds.Length == 0) {
			Debug.Log("Rounds are empty, returning");
			roundState = RoundState.Waiting;
			yield return null;
		}

		// spawn sub rounds (different enemy types per round)
		foreach (var round in curRound.subRounds)
			StartCoroutine(spawnSubRound(round));
		
		roundState = RoundState.InProgress;

		yield return null;
	}

	// spawn a sub round after waiting on it's start delay
	IEnumerator spawnSubRound(Round curRound) {
		if (curRound.startDelay != 0)
			yield return new WaitForSeconds(curRound.startDelay + 2f);
		
		for (int i = 0; i < curRound.enemyCount; i++) {
			spawnEnemy(curRound.enemy);
			yield return new WaitForSeconds(curRound.spawnRate + 0.2f);
		}
	}

	// spawn the enemy into the scene
	private void spawnEnemy(Transform enemyToSpawn) {
		var tempEnemy = Instantiate(enemyToSpawn, getCheckpoints()[0], Quaternion.identity);
		rounds[currentRound].enemies.enqueue(tempEnemy.gameObject.GetInstanceID());
		var enemyInstance = tempEnemy.gameObject.GetComponent<Enemy>();
		
		// setting the waypoints / checkpoints so the enemy knows where to go
		enemyInstance.setWaypoints(getCheckpoints());
		enemyInstance.levelManager = GetComponent<LevelManager>();
	}

	// show the disaster prompt that the disaster is coming in 3 rounds
	private void showDisasterPrompt() {
		disasterPrompt.SetActive(true);
		mainManager.getStateManager().showDisasterPrompt();
	}

	// accept the disaster prompt, effectively closing it
	public void acceptDisasterPrompt() {
		disasterPrompt.SetActive(false);
		mainManager.getStateManager().hideDisasterPrompt();
	}

	// return the current checkpoints object based on which event is active
	private List<Vector3> getCheckpoints() {
		return eventState switch {
			EventState.Event1 => checkpointsEvent1,
			EventState.Event2 => checkpointsEvent2,
			_ => checkpointsNoEvent
		};
	}

	// read in all checkpoints for each disaster location / event
	// this works by reading child objects and getting their locations within the scene
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

	// spawn in the event (disaster) based on the predetermined event location at the start of the level
	// this will also switch the map to the new location
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

	// change the map to the new one for post disaster
	// we need to wait a little bit so it changes during the white flash of the disaster
	private IEnumerator switchMap(float duration, int eventNum) {
		yield return new WaitForSeconds(duration);
		
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

	// send a circular raycast based on a specific point at a set range
	// detect all enemies and towers within the raycast and remove them
	private void destroyWithinRadius(Vector3 target, float range) {
		var transforms = new List<Transform>();
		foreach (var colr in Physics2D.OverlapCircleAll(target, range)) {
			Debug.Log("Collider hit for disaster:" + colr.name);
			if (colr.gameObject.CompareTag("TowerCollider")) {
				transforms.Add(colr.gameObject.transform);
			}
			else if (colr.gameObject.CompareTag("Enemy"))
				transforms.Add(colr.gameObject.transform);
		}
		
		if (transforms.Count <= 0) {
			Debug.Log("Did not hit any towers for the disaster");
			return;
		}
		
		Debug.Log("Hit " + transforms.Count + " towers");
		
		for (var i = transforms.Count - 1; i >= 0; i--)
			Destroy(transforms[i].parent.gameObject);
	}

	// load the maps and set the default map
	private void loadMaps() {
		mapNoEvent.SetActive(true);
		mapEvent1.SetActive(false);
		mapEvent2.SetActive(false);
	}

	// set the level variables
	private void resetLevel() {
		
		// destroy all enemies on the screen, if present 
        foreach (var t in GameObject.FindGameObjectsWithTag("Enemy"))
			Destroy(t);

		disasterPrompt.SetActive(false);
        
		eventState = EventState.NoEvent;
		disasterRound = -1;
		levelWon = false;
        
		loadCheckpoints();
		loadMaps();
        
		// initialize all rounds for the level
		foreach (var round in rounds)
			round.init();
        
		// calculate which event to play later on in the level
		futureEvent = Random.Range(1, 3) == 1 ? EventState.Event1 : EventState.Event2;
		
		winUI.SetActive(false);
		loseUI.SetActive(false);
		
		pauseButton.SetActive(false);
		startRoundButton.SetActive(true);
	}

	// end the level
	// true if level is won
	// false if the level is marked as a lost
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

	// set round counter
	private void setRoundCounter() {
		roundCounter.GetComponent<TextMeshProUGUI>().SetText("Round: " + (currentRound + 1) + " / " + rounds.Length);
	}
}
