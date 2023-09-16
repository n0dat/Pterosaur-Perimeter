using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour {

	public enum RoundState { SPAWNING, WAITING, IN_PROGRESS };

	[System.Serializable]
	public class Round {
		public int roundNumber;
		public Transform enemy;
		public int enemyCount;
		public float spawnRate;
	}
	
	public Round[] rounds;
	public int currentRound = 0;
	
	public float roundDelay = 5f;

	public float roundCountdown;

	public RoundState state = RoundState.WAITING;
	
	[SerializeField]
	private GameObject checkpointObject;
	
	[SerializeField]
	private List<Vector3> checkpoints;

	public bool loadCheckpointObject() {
		checkpointObject = GameObject.Find("Checkpoints") as GameObject;
		return (checkpointObject != null);
	}

	public void loadCheckpoints() {
		if (!loadCheckpointObject())
			return;
		foreach (Transform checkpoint in checkpointObject.transform)
			checkpoints.Add(new Vector3(checkpoint.gameObject.transform.position.x, checkpoint.gameObject.transform.position.y, 0f));
	}

	public void clearCheckpoints() {
		checkpoints.Clear();
	}

	void Start() {
		loadCheckpoints();
		roundCountdown = roundDelay;
	}

	void Update() {
		if (state == RoundState.IN_PROGRESS) {
			if (!hasEnemies())
				endRound();
			else
				return;
		}

		if (roundCountdown <= 0) {
			if (state != RoundState.SPAWNING)
				StartCoroutine(SpawnRound(rounds[++currentRound]));
		}
		else
			roundCountdown -= Time.deltaTime;
	}

	void endRound() {
		Debug.Log("Ending Round");
        
		roundCountdown = roundDelay;

		if (currentRound + 1 >= rounds.Length - 1) {
			currentRound = 0;
			Debug.Log("All rounds completed. Looping to first round");
		}
		else
			currentRound++;

		state = RoundState.WAITING;
	}

	bool hasEnemies() {
		if (GameObject.FindGameObjectWithTag("Enemy") == null)
			return false;
		return true;
	}

	IEnumerator SpawnRound(Round curRound) {
		Debug.Log("Spawning Round: " + curRound.roundNumber);
		state = RoundState.SPAWNING;

		for (int i = 0; i < curRound.enemyCount; i++) {
			spawnEnemy(curRound.enemy);
			yield return new WaitForSeconds( 1f / curRound.spawnRate);
		}
		
		state = RoundState.IN_PROGRESS;
		
		yield break;
	}

	void spawnEnemy(Transform enemyToSpawn) {
		Debug.Log("Spawning Enemy: " + enemyToSpawn.name);
		Instantiate(enemyToSpawn, checkpoints[0], Quaternion.identity).gameObject.GetComponent<Enemy>().checkpoints = checkpoints;
	}

}
