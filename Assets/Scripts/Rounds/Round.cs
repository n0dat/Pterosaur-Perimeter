using Customs;
using UnityEngine;

// round class to hold round information per level
[System.Serializable]
public class Round {
    public int roundNumber;
    public Transform enemy;
    public int enemyCount;
    public float spawnRate;
    public bool isComplete = false;
    public CircularBuffer<int> enemies;
    public float startDelay;
    public Round[] subRounds;

    public void init() {
        enemies = new CircularBuffer<int>(enemyCount);
    }

}