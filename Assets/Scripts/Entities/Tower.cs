using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour {
    
    // globals
    
    [SerializeField]
    private int towerCost, repairCost, radiusLineSegments;
    
    [SerializeField]
    private float durability, attackSpeed, attackDamage, attackRange;
    
    [SerializeField]
    private bool hasRangeUpgrade, hasAttackSpeedUpgrade, hasDamageUpgrade, showRadius, isHeld, selected, validPosition;
    
    [SerializeField]
    private LineRenderer radiusLine;
    
    [SerializeField]
    private Material lineMaterial;

    // some basics
    
    void Awake() {
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
        isHeld = false;
        selected = false;
        validPosition = true;
        
        radiusLine.startColor = new Color(224f, 67f, 85f, 0.85f);
        radiusLine.endColor = new Color(224f, 67f, 85f, 0.85f);
        radiusLine.enabled = true;
        
        lineMaterial = radiusLine.material;
        
        setLineColorGrey();
        
        drawRadiusCircle();
        deselect();
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

    void Update() {
        if (isHeld) {
            setRadiusColor();
            drawRadiusCircle();
        }
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
