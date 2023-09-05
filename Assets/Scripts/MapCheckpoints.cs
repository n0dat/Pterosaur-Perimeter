using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class MapCheckpoints {
    
    private List<Vector3> checkpoints;
    private int currentCheckpoint;

	public MapCheckpoints() {
		checkpoints = new List<Vector3>();
		currentCheckpoint = 0;
	}

    public Vector3 next() {
        return checkpoints[currentCheckpoint++];
    }

    public bool hasNext() {
        return currentCheckpoint != checkpoints.Count;
    }

	public Vector3 last() {
		return checkpoints.LastOrDefault();
	}

	public void add(Vector3 newCheckpoint) {
		checkpoints.Add(newCheckpoint);
	}

	public void reset() {
		currentCheckpoint = 0;
	}
}
