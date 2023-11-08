using Customs;
using UnityEngine;

[System.Serializable]
public class Round {
    public int roundNumber;
    public Transform enemy;
    public int enemyCount;
    public float spawnRate;
    public bool isComplete;
    public CircularBuffer<int> enemies;
    public float startDelay;
    public Round[] subRounds;

    public void init() {
        enemies = new CircularBuffer<int>(enemyCount);
    }

}