using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    
    // globals
    
    [SerializeField]
    private int towerCost, repairCost;
    
    [SerializeField]
    private float durability, attackSpeed, attackDamage, attackRange;
    
    [SerializeField]
    private bool hasRangeUpgrade, hasAttackSpeedUpgrade, hasDamageUpgrade, showRadiusCircle;
    
    [SerializeField]
    private LineRenderer radiusLine;
    private int radiusLineSegments;

    // some basics
    
    void Start() {
        towerCost = 0;
        repairCost = 0;
        radiusLineSegments = 50;
        
        durability = 100.0f;
        attackSpeed = 5.0f;
        attackDamage = 10.0f;
        attackRange = 60.0f;
        
        hasRangeUpgrade = false;
        hasAttackSpeedUpgrade = false;
        hasDamageUpgrade = false;
        showRadiusCircle = false;
        
        radiusLine.startColor = new Color(49, 49, 49);;
        radiusLine.endColor = new Color(49, 49, 49);
    }

    void Update() {
        radiusLine.enabled = showRadiusCircle;
        drawRadiusCircle();
    }

    // main functionality
    
    private void drawRadiusCircle() {
        if (radiusLine.enabled) {
            Vector3[] points = new Vector3[radiusLineSegments + 1];
        
            float angle = 0f;
            float angleIncrement = 2 * Mathf.PI / radiusLineSegments;

            for (int i = 0; i < radiusLineSegments + 1; i++, angle += angleIncrement) {
                points[i] = new Vector3(Mathf.Cos(angle) * attackRange, Mathf.Sin(angle) * attackRange, 0f) + transform.position;
            }

            radiusLine.positionCount = points.Length;
            radiusLine.SetPositions(points);
        }
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

    public bool getShowRadiusCircle() {
        return showRadiusCircle;
    }

    public void setShowRadiusCircle(bool val) {
        showRadiusCircle = val;
    }
}
