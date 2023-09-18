using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float speed = 5f;
	
	[SerializeField]
	private float totalDistance = 0f, distanceCovered = 0f;
	
	public float health = 0f;
	
	
	private Vector3 target;
	private int waypointIndex = 0;
	
	public List<Vector3> checkpoints;

	void Start() {
		target = checkpoints[0];
		findTotalDistance();
	}

	void Update() {
		
		// used for targeting
		distanceCovered += (speed * Time.deltaTime) / totalDistance;
		
		Vector3 dir = target - transform.position;
		transform.Translate(dir.normalized * (Time.deltaTime * speed), Space.World);
		
		if (Vector3.Distance(transform.position, target) <= 0.6f)
			GetNextCheckpoint();
	}

	void GetNextCheckpoint() {
		if (waypointIndex >= checkpoints.Count - 1) {
			Destroy(gameObject);
			return;
		}
		target = checkpoints[++waypointIndex];
	}

	private void OnDestroy() {
		var roundManager = GameObject.Find("RoundSpawner").GetComponent<RoundManager>();
		if (roundManager != null)
			roundManager.removeEnemy(this.gameObject.GetInstanceID());
	}

	public float getTotalDistance() {
		return totalDistance;
	}

	public float getTravelDistance() {
		return distanceCovered;
	}

	private void findTotalDistance() {
		totalDistance = 0f;
		for (int i = 0; i < checkpoints.Count - 1; i++)
			totalDistance += Vector3.Distance(checkpoints[i], checkpoints[i + 1]);
	}
}