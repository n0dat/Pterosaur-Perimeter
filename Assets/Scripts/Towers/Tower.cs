using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour {
    
    // globals
    public enum TowerType { Water, Land };
    private enum AttackState { Attacking, Waiting, Initial };

    private enum TargetingMode { First, Last, Strong };

    [SerializeField]
    private int towerCost, repairCost, radiusLineSegments, killCount;
    
    [SerializeField]
    private float durability, attackSpeed, attackDamage, attackRange;
    
    [SerializeField]
    private bool hasRangeUpgrade, hasAttackSpeedUpgrade, hasDamageUpgrade, isHeld, selected, validPosition, attackInProgress;
    
    [SerializeField]
    private LineRenderer radiusLine;
    
    [SerializeField]
    private Collider2D[] enemiesInRange;
    
    [SerializeField]
    private GameObject enemyToAttack;
    
    [SerializeField]
    private AttackState attackState = AttackState.Initial;
    
    [SerializeField]
    private TargetingMode targetingMode = TargetingMode.First;

    [SerializeField]
    private RoundManager roundManager;
    
    [SerializeField]
    private AttackType attackType;

    [SerializeField]
    private TowerType towerType;
    
    
    // initialize most global values
    void Awake() {
        towerCost = 0;
        repairCost = 0;
        radiusLineSegments = 50;
        killCount = 0;
        
        durability = 100.0f;
        attackSpeed = 1.7f;
        attackDamage = 25.0f;
        attackRange = 60.0f;
        
        hasRangeUpgrade = false;
        hasAttackSpeedUpgrade = false;
        hasDamageUpgrade = false;
        isHeld = false;
        selected = false;
        validPosition = true;
        attackInProgress = false;
        
        radiusLine.startColor = new Color(224f, 67f, 85f, 0.85f);
        radiusLine.endColor = new Color(224f, 67f, 85f, 0.85f);
        radiusLine.enabled = true;
        
        setLineColorGrey();
        
        drawRadiusCircle();
        deselect();
        
        enemiesInRange = Array.Empty<Collider2D>();
        
        enemyToAttack = null;
        
        roundManager = GameObject.Find("RoundSpawner").GetComponent<RoundManager>();
    }
    
    // called every frame
    void Update() {
        if (isHeld) { // only ran when tower is going to be placed
            setRadiusColor();
            drawRadiusCircle();
        }
        else { // ran when tower is placed
            
            // check if enemies are within attack radius of tower
            getEnemiesInRadius();
            
            // we got enemies in radius
            if (enemiesInRange.Length > 0) {
                if (!attackInProgress) {
                    setTargetingMode(targetingMode);
                
                    // face the enemy we are going to attack
                    var direction = enemyToAttack.transform.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);

                    // start attacking
                    if (enemyToAttack != null && attackState != AttackState.Attacking) {
                        attackState = AttackState.Attacking;
                        StartCoroutine(attackRoutine());
                    }
                }
            }
            // no enemies in radius, so waiting
            else {
                attackState = AttackState.Waiting;
            }
        }
    }
    
    //
    //
    // ATTACK LOGIC
    //
    //
    private void setTargetingMode(TargetingMode targetMode) {
        if (targetMode == TargetingMode.First) { // target enemy within radius that is furthest along the track
            var distance = 0f;
            var initLength = enemiesInRange.Length;
            foreach (var enemy in enemiesInRange) {
                if (enemiesInRange.Length != initLength)
                    break;
                var tempDistance = enemy.gameObject.GetComponent<Enemy>().getTravelDistance();
                if (tempDistance >= distance) {
                    distance = tempDistance;
                    enemyToAttack = enemy.gameObject;
                }
            }
        }
        else if (targetMode == TargetingMode.Last) { // target enemy within radius that is least furthest along the track
            var distance = 1f;
            var initLength = enemiesInRange.Length;
            foreach (var enemy in enemiesInRange) {
                if (enemiesInRange.Length != initLength)
                    break;
                var tempDistance = enemy.gameObject.GetComponent<Enemy>().getTravelDistance();
                if (tempDistance <= distance) {
                    distance = tempDistance;
                    enemyToAttack = enemy.gameObject;
                }
            }
        }
        else if (targetMode == TargetingMode.Strong) { // target enemy within radius with the most health
            var strength = 0f;
            var initLength = enemiesInRange.Length;
            foreach (var enemy in enemiesInRange) {
                if (enemiesInRange.Length != initLength)
                    break;
                var health = enemy.gameObject.GetComponent<Enemy>().health;
                if (health >= strength) {
                    strength = health;
                    enemyToAttack = enemy.gameObject;
                }
            }
        }
    }

    public IEnumerator attackRoutine() {
        while (attackState == AttackState.Attacking && enemyToAttack != null) {
            WaitForAttack waitForAttack = new WaitForAttack();
            StartCoroutine(finishAttack(waitForAttack));
            yield return waitForAttack;
            attackInProgress = false;
            yield return new WaitForSeconds(attackSpeed);
        }
    }

    private IEnumerator finishAttack(WaitForAttack waitForAttack) {
        attackInProgress = true;
        attackType.attack(enemyToAttack, gameObject);
        waitForAttack.setAttackFinished();
        yield return null;
    }

    private void getEnemiesInRadius() {
        var temp = new List<Collider2D>();
        foreach (var enemy in Physics2D.OverlapCircleAll(transform.position, attackRange))
            if (enemy.gameObject.CompareTag("Enemy"))
                temp.Add(enemy);
        
        enemiesInRange = temp.ToArray();
    }
    
    public void enemyKilled() {
        StopCoroutine(attackRoutine());
        attackState = AttackState.Waiting;
        killCount++;
    }
    
    //
    //
    // PLACEMENT LOGIC
    //
    //

    void OnCollisionEnter2D(Collision2D collision) {
        if (isHeld) {
            if (collision.gameObject.CompareTag("Tower") || collision.gameObject.CompareTag("Path")) {
                setValidPosition(false);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (isHeld) {
            if (collision.gameObject.CompareTag("Tower") || collision.gameObject.CompareTag("Path")) {
                setValidPosition(true);
            }
        }
    }


    //
    //
    // UI REPRESENT
    //
    //

    public void setLineColorGrey() {
        //radiusLine.material.SetColor("_Color",  new Color(49, 49, 49));
        // set the color to the material on the radius line instead
        //radiusLine.startColor = new Color(49, 49, 49);
        //radiusLine.endColor = new Color(49, 49, 49);
        var gradient = new Gradient();
        var tempColorKeys = radiusLine.colorGradient.colorKeys;
        for (int i = 0; i < tempColorKeys.Length; i++)
            tempColorKeys[i].color = Color.grey;
            
        gradient.colorKeys = tempColorKeys;
            
        radiusLine.colorGradient = gradient;
    }

    public void setLineColorRed() {
        //radiusLine.material.SetColor("_Color", new Color(224f, 67f, 85f, 0.85f));
        //radiusLine.startColor = new Color(224f, 67f, 85f, 0.85f);
        //radiusLine.endColor = new Color(224f, 67f, 85f, 0.85f);
        
        var gradient = new Gradient();
        var tempColorKeys = radiusLine.colorGradient.colorKeys;
        for (int i = 0; i < tempColorKeys.Length; i++)
            tempColorKeys[i].color = Color.red;
            
        gradient.colorKeys = tempColorKeys;
            
        radiusLine.colorGradient = gradient;
    }
    
    private void drawRadiusCircle() {
        if (radiusLine.enabled) {
            Vector3[] points = new Vector3[radiusLineSegments + 1];
        
            float angle = 0f;
            float angleIncrement = 2 * Mathf.PI / radiusLineSegments;

            for (int i = 0; i < radiusLineSegments + 1; i++, angle += angleIncrement)
                points[i] = new Vector3(Mathf.Cos(angle) * attackRange, Mathf.Sin(angle) * attackRange, 0f) + transform.position;

            radiusLine.positionCount = points.Length;
            radiusLine.SetPositions(points);
        }
    }

    public void updateRadiusCircle() {
        //
        drawRadiusCircle();
    }
    
    //
    //
    // UI INTERACTION
    //
    //
    public bool beingHeld() {
        return isHeld;
    }

    public bool isSelected() {
        return selected;
    }

    public void deselect() {
        selected = false;
        hideRadiusCircle();
    }

    public void select() {
        selected = true;
        showRadiusCircle();
    }

    public void holdTower() {
        isHeld = true;
        showRadiusCircle();
    }

    public void dropTower() {
        isHeld = false;
        hideRadiusCircle();
    }
    
    private void setRadiusColor() {
        if (validPosition)
            setLineColorGrey();
        else
            setLineColorRed();
    }
    
    public bool isValidPosition() {
        return validPosition;
    }

    public void setValidPosition(bool val) {
        validPosition = val;
    }

    public void showRadiusCircle() {
        radiusLine.enabled = true;
    }

    public void hideRadiusCircle() {
        radiusLine.enabled = false;
    }

    //
    //
    // GETTERS AND SETTERS
    //
    //

    public int getTowerCost() {
        return towerCost;
    }

    public void setTowerCost(int val) {
        towerCost = val;
    }

    public int getRepairCost() {
        return repairCost;
    }

    public void setRepairCost(int val) {
        repairCost = val;
    }

    public float getDurability() {
        return durability;
    }

    public void setDurability(float val) {
        durability = val;
    }

    public float getAttackSpeed() {
        return attackSpeed;
    }

    public void setAttackSpeed(float val) {
        attackSpeed = val;
    }

    public float getAttackDamage() {
        return attackDamage;
    }

    public void setAttackDamage(float val) {
        attackDamage = val;
    }

    public float getAttackRange() {
        return attackRange;
    }

    public void setAttackRange(float val) {
        attackRange = val;
    }

    public bool getHasRangeUpgrade() {
        return hasRangeUpgrade;
    }

    public void setHasRangeUpgrade(bool val) {
        hasRangeUpgrade = val;
    }

    public bool getHasAttackSpeedUpgrade() {
        return hasAttackSpeedUpgrade;
    }

    public void setHasAttackSpeedUpgrade(bool val) {
        hasAttackSpeedUpgrade = val;
    }

    public bool getHasDamageUpgrade() {
        return hasDamageUpgrade;
    }

    public void setHasDamageUpgrade(bool val) {
        hasDamageUpgrade = val;
    }
}
