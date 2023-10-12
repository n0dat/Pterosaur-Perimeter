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

    public void init() {
        enemies = new CircularBuffer<int>(enemyCount);
    }

    //public Transform spawnEnemy(Vector3 initialPosition) {
    //    
    //}
}