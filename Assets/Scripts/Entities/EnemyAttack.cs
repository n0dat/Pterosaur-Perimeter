using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
    
    [SerializeField] private Enemy m_enemy;
    public float attackDelay = 1.5f;
    public bool readyToAttack = true;
    public int damage = 20;
    public float range = 20f;

    void Update() {
        if (readyToAttack) {
            var towers = new List<Tower>();
            foreach (var tower in Physics2D.OverlapCircleAll(transform.position, range))
                if (tower.gameObject.CompareTag("Tower"))
                    towers.Add(tower.gameObject.GetComponent<Tower>());
            
            if (towers.Count > 0)
                StartCoroutine(attack(towers[Random.Range(0, towers.Count)]));
        }
    }

    IEnumerator attack(Tower towerToAttack) {
        if (towerToAttack) {
            readyToAttack = false;
            m_enemy.attack();
            towerToAttack.takeDamage(damage);
            yield return new WaitForSeconds(attackDelay);
            readyToAttack = true;
        }
        
        yield return null;
    }
}
