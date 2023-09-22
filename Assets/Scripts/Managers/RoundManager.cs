using System.Collections;
using System.Collections.Generic;
using Customs;
using UnityEngine;

public class RoundManager : MonoBehaviour {

	public enum RoundState { Spawning, Waiting, InProgress };

	[System.Serializable]
	public class Round {
		public int roundNumber;
		public Transform enemy;
		public int enemyCount;
		public float spawnRate;
		public bool isComplete;
		public CircularBuffer<int> enemies;
		public float startDelay;

		public void init() {
			enemies = new CircularBuffer<int>(enemyCount);
		}
	}
	
	public RoundState state = RoundState.Waiting;
	public Round[] rounds;
	public int currentRound = 0;
	
	public float roundDelay = 5f;
	public float roundCountdown;
	
	
	[SerializeField]
	private GameObject checkpointObject;
	[SerializeField]
	private List<Vector3> checkpoints;
	
	public void removeEnemy(int enemy) {
		rounds[currentRound].enemies.remove(enemy);
	}

	public bool loadCheckpointObject() {
		checkpointObject = GameObject.Find("Checkpoints") as GameObject;
		return (checkpointObject != null);
	}

	public void loadCheckpoints() {
		if (!loadCheckpointObject())
			return;
		foreach (Transform checkpoint in checkpointObject.transform) {
			var checkpointPos = checkpoint.gameObject.transform.position;
			checkpoints.Add(new Vector3(checkpointPos.x, checkpointPos.y, 0f));
		}
	}

	public void clearCheckpoints() {
		checkpoints.Clear();
	}

	void Start() {
		loadCheckpoints();
		roundCountdown = roundDelay;
		foreach (var round in rounds)
			round.init();
	}

	void Update() {
		if (state == RoundState.InProgress) {
			if (!hasEnemies())
				endRound();
			else
				return;
		}

		if (roundCountdown <= 0) {
			if (state != RoundState.Spawning)
				StartCoroutine(spawnRound(rounds[currentRound]));
		}
		else
			roundCountdown -= Time.deltaTime;
	}

	void endRound() {
		roundCountdown = roundDelay;
		
		rounds[currentRound].enemies.clear();

		if (currentRound + 1 >= rounds.Length) {
			currentRound = 0;
			Debug.Log("All rounds completed. Looping to first round");
			foreach (var round in rounds)
				round.isComplete = false;
		}
		else
			rounds[currentRound++].isComplete = true;

		state = RoundState.Waiting;
	}

	bool hasEnemies() {
		return !rounds[currentRound].enemies.IsEmpty;
	}

	IEnumerator spawnRound(Round curRound) {
		Debug.Log("Spawning Round: " + curRound.roundNumber);
		state = RoundState.Spawning;

		for (int i = 0; i < curRound.enemyCount; i++) {
			spawnEnemy(curRound.enemy);
			yield return new WaitForSeconds(1f / curRound.spawnRate);
		}
		
		state = RoundState.InProgress;
	}

	void spawnEnemy(Transform enemyToSpawn) {
		Debug.Log("Spawning Enemy: " + enemyToSpawn.name);
		var tempEnemy = Instantiate(enemyToSpawn, checkpoints[0], Quaternion.identity);
		rounds[currentRound].enemies.enqueue(tempEnemy.gameObject.GetInstanceID());
		tempEnemy.gameObject.GetComponent<Enemy>().waypoints = checkpoints;
	}
}
