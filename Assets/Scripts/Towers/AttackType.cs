using UnityEngine;

public class AttackType : MonoBehaviour {

    private enum AttackStyle { Single, Triple, Circle, Heal };
    [SerializeField] private AttackStyle attackStyle;
    public Transform projectile;
    
    public Tower source;

    // attack the enemy passed in
    // sourceEnemy - enemy to attack
    // sourceTower - tower that called attack
    public void attack(GameObject sourceEnemy, GameObject sourceTower) {
        var tower = sourceTower.GetComponent<Tower>();
        var enemy = sourceEnemy.GetComponent<Enemy>();
        
        var direction = (enemy.transform.position - transform.position).normalized;

        switch (attackStyle) {
            // single fire attack
            case AttackStyle.Single:
                var laserSingle = Instantiate(projectile, transform.position, Quaternion.identity);
                var laserSingleComp = laserSingle.GetComponent<Laser>();
                laserSingleComp.parent = tower;
                laserSingleComp.shoot(direction, enemy);
                break;
            case AttackStyle.Triple:
                // triple shot attack
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
                // circular attack
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
        }
    }

    // heal function for heal tower
    // raycast in circle to get all Tower colliders
    // then heal all
    public void heal(float range) {
        foreach (var collider in Physics2D.OverlapCircleAll(transform.position, range)) {
            if (collider.gameObject.CompareTag("Tower")) {
                var targetTower = collider.gameObject.GetComponent<Tower>();
                if (targetTower)
                    targetTower.heal(5);
            }
        }
    }
}
