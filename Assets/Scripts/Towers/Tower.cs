using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour {
    
    // globals
    
    public enum AttackState { ATTACKING, WAITING, INITIAL };

    [SerializeField]
    private int towerCost, repairCost, radiusLineSegments, killCount;
    
    [SerializeField]
    private float durability, attackSpeed, attackDamage, attackRange;
    
    [SerializeField]
    private bool hasRangeUpgrade, hasAttackSpeedUpgrade, hasDamageUpgrade, showRadius, isHeld, selected, validPosition, targetStrong, targetFirst;
    
    [SerializeField]
    private LineRenderer radiusLine;
   
    
    [SerializeField]
    private Collider2D[] enemiesInRange;
    
    [FormerlySerializedAs("firstEnemy")] [SerializeField]
    private GameObject enemyToAttack;
    
    [SerializeField]
    private AttackState attackState = AttackState.INITIAL;

    [SerializeField]
    private RoundManager roundManager;
    // some basics
    
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
        targetStrong = false;
        targetFirst = true;
        
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
    
    void Update() {
        if (isHeld) {
            setRadiusColor();
            drawRadiusCircle();
        }
        else {

            // handle enemy targeting
            getEnemiesInRadius();
            if (enemiesInRange.Length > 0) { // we have come enemies in range
                if (targetFirst) { // target the furthest along the track in our range
                    float distance = 0f;
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
                else if (targetStrong) { // target the strongest in range
                    var strength = 0f;
                    var initLength = enemiesInRange.Length;
                    foreach (var enemy in enemiesInRange) {
                        if (enemiesInRange.Length != initLength)
                            break;
                        var health = enemy.gameObject.GetComponent<Enemy>().getTravelDistance();
                        if (health >= strength) {
                            strength = health;
                            enemyToAttack = enemy.gameObject;
                        }
                    }
                }
                
                var direction = enemyToAttack.transform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1f);

                if (enemyToAttack != null && attackState != AttackState.ATTACKING) {
                    attackState = AttackState.ATTACKING;
                    StartCoroutine(attackRoutine());
                }
            }
            else {
                attackState = AttackState.WAITING;
            }
        }
    }
    
    public IEnumerator attackRoutine() {
        while (attackState == AttackState.ATTACKING) {
            var enemy = enemyToAttack.GetComponent<Enemy>();
            if (enemy != null) {
                attack(enemy, gameObject.GetComponent<Tower>());
                yield return new WaitForSeconds(attackSpeed);
            }
            else {
                yield break;
            }
        }
    }

    public void attack(Enemy enemy, Tower tower) {
        Debug.Log("Attacked enemy");
        enemy.takeDamage(attackDamage, tower);
    }

    private void getEnemiesInRadius() {
        var temp = new List<Collider2D>();
        foreach (var enemy in Physics2D.OverlapCircleAll(transform.position, attackRange))
            if (enemy.gameObject.CompareTag("Enemy"))
                temp.Add(enemy);
        
        enemiesInRange = temp.ToArray();
    }

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

    public void enemyKilled() {
        Debug.Log("Tower: " + GetInstanceID() + " killed an enemy.");
        StopCoroutine(attackRoutine());
        attackState = AttackState.WAITING;
        killCount++;
    }

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

    // main functionality

    public bool isValidPosition() {
        return validPosition;
    }

    public void setValidPosition(bool val) {
        validPosition = val;
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

    public void showRadiusCircle() {
        radiusLine.enabled = true;
    }

    public void hideRadiusCircle() {
        radiusLine.enabled = false;
    }

    // getters and setters

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
