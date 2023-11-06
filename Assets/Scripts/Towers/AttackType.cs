using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;

public class AttackType : MonoBehaviour {

    private enum AttackStyle { Single, Triple, Circle, Heal };
    [SerializeField] private AttackStyle attackStyle;
    public Transform projectile;

    public void attack(GameObject sourceEnemy, GameObject sourceTower) {
        var tower = sourceTower.GetComponent<Tower>();
        var enemy = sourceEnemy.GetComponent<Enemy>();
        
        var direction = (enemy.transform.position - transform.position).normalized;

        switch (attackStyle) {
            case AttackStyle.Single:
                // TODO: PLAY ATTACK ANIMATION HERE
                var laserSingle = Instantiate(projectile, transform.position, Quaternion.identity);
                var laserSingleComp = laserSingle.GetComponent<Laser>();
                laserSingleComp.parent = tower;
                laserSingleComp.shoot(direction, enemy);
                break;
            case AttackStyle.Triple:
                // TODO: PLAY ATTACK ANIMATION HERE
                for (int i = -15; i <= 15; i += 15) {
                    var targetDirection = enemy.transform.position - transform.position;
                    var baseAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                    var angle = baseAngle + i;
                    var rot = Quaternion.Euler(0f, 0f, angle);
                    Vector3 localDirection = rot * Vector3.right;
                    Vector3 worldDirection = transform.TransformDirection(localDirection);
                    var laserTriple = Instantiate(projectile, transform.position, rot);
                    var laserTripleComp = laserTriple.GetComponent<Laser>();
                    laserTripleComp.parent = tower;
                    laserTripleComp.shoot(worldDirection, enemy);
                }
                break;
            case AttackStyle.Circle:
                // TODO: PLAY ATTACK ANIMATION HERE
                for (int i = 0; i < 8; i++) {
                    float angle = i * 360f / 8;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                    Vector3 localDirection = Quaternion.Euler(0.0f, 0.0f, angle) * Vector3.right;
                    Vector3 worldDirection = transform.TransformDirection(localDirection);
                    var laserCircle = Instantiate(projectile, transform.position, rotation);
                    var laserCircleComp = laserCircle.GetComponent<Laser>();
                    laserCircleComp.parent = tower;
                    laserCircleComp.shoot(worldDirection, enemy);
                }
                break;
            case AttackStyle.Heal:
                // TODO: PLAY HEALING ANIMATION HERE
                foreach (var enemyCollider in Physics2D.OverlapCircleAll(transform.position, tower.getAttackRange())) {
                    if (enemyCollider.gameObject.CompareTag("Tower")) {
                        var targetTower = enemyCollider.gameObject.GetComponent<Tower>();
                        targetTower.setHealth(targetTower.getHealth() + 5);
                    }
                }
                break;
        }
    }
}
