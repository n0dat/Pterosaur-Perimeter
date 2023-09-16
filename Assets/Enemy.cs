using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float speed = 5f;
	
	private Vector3 target;
	private int waypointIndex = 0;
	
	public List<Vector3> checkpoints;

	void Start() {
		target = checkpoints[0];
	}

	void Update() {
		Vector3 dir = target - transform.position;
		transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
		
		if (Vector3.Distance(transform.position, target) <= 0.4f)
			GetNextCheckpoint();
	}

	void GetNextCheckpoint() {
		if (waypointIndex >= checkpoints.Count - 1) {
			Destroy(gameObject);
			return;
		}
		
		target = checkpoints[++waypointIndex];
	}
}