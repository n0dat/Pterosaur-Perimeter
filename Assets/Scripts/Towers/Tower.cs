using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour {
    
    // globals
    public enum TowerType { Water, Land }
    private enum AttackState { Attacking, Waiting, Initial };
    private enum TargetingMode { First, Last, Strong };
    
    private Animator m_animator;

    [SerializeField]
    private int towerCost, repairCost, radiusLineSegments = 50, killCount;
    [SerializeField]
    private float attackSpeed, attackDamage, attackRange;
    [SerializeField]
    private bool isHeld, selected, validPosition, attackInProgress;
    
    /// Diego Testing healing working
    //[SerializeField] public int towerHealth = 50;
    ///


    [SerializeField] private LineRenderer radiusLine;
    [SerializeField] private Collider2D[] enemiesInRange;
    [SerializeField] private GameObject enemyToAttack;
    [SerializeField] private AttackState attackState = AttackState.Initial;
    [SerializeField] private TargetingMode targetingMode = TargetingMode.First;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private AttackType attackType;
    [SerializeField] private TowerType towerType;
    [SerializeField] private SpriteRotator spriteRotator;
    [SerializeField] private LevelManager levelManager;
    
    //Upgrade system
    private UpgradeMenuHandler m_upgradeMenuInterface;
    private int m_damageUpgradeLevel = 0;
    private int m_rangeUpgradeLevel = 0;
    private int m_attackSpeedUpgradeLevel = 0;
    private int m_health = 100;

    // ui
    [SerializeField] private HealthUIHandler m_healthBar;
    
    // towr healing
    [SerializeField] private bool isHealingTower = false, readyToHeal = true;

    [SerializeField] private TowerAudioSpawner m_audioHandler;
    
    // initialize most global values
    void Awake() {
        repairCost = 0;
        killCount = 0;
        
        isHeld = false;
        selected = false;
        validPosition = true;
        attackInProgress = false;
        radiusLine.enabled = true;
        
        setLineColorGrey();
        drawRadiusCircle();
        
        enemiesInRange = Array.Empty<Collider2D>();
        enemyToAttack = null;
        
        spriteRotator = gameObject.GetComponent<SpriteRotator>();
        m_animator = gameObject.GetComponent<Animator>();
        
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        m_upgradeMenuInterface = GameObject.Find("UpgradeMenu").GetComponent<UpgradeMenuHandler>(); //Must be called UpgradeMenu.
        deselect();
    }
    
    // called every frame
    void Update() {
        if (isHeld) { // only ran when tower is going to be placed
            setRadiusColor();
            drawRadiusCircle();
        }
        else { // ran when tower is placed
            if (isHealingTower) {
                if (readyToHeal && levelManager.roundState != RoundState.Waiting)
                    StartCoroutine(healTowers());
                
                return;
            }
            
            // check if enemies are within attack radius of tower
            getEnemiesInRadius();
            
            // we got enemies in radius
            if (enemiesInRange.Length > 0) {
                setTargetingMode(targetingMode);
                
                var direction = enemyToAttack.transform.position - transform.position;
                if (!isHealingTower)
                    spriteRotator.setDir(direction);
                
                if (!attackInProgress) {
                    // start attacking
                    if (enemyToAttack && attackState != AttackState.Attacking) {
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
        while (attackState == AttackState.Attacking && enemyToAttack) {
            WaitForAttack waitForAttack = new WaitForAttack();
            StartCoroutine(finishAttack(waitForAttack));
            yield return waitForAttack;
            yield return new WaitForSeconds(getAttackSpeed());
            attackInProgress = false;
        }
    }

    private IEnumerator finishAttack(WaitForAttack waitForAttack) {
        attackInProgress = true;
        attackType.attack(enemyToAttack, gameObject);
        m_audioHandler.shoot();
        //m_animator.SetTrigger("Attack");
        waitForAttack.setAttackFinished();
        yield return null;
    }

    private void getEnemiesInRadius() {
        var temp = new List<Collider2D>();
        foreach (var enemy in Physics2D.OverlapCircleAll(transform.position, getAttackRange()))
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
            Debug.Log("Colliding with: " + collision.gameObject.name);
            var col = collision.gameObject;

            if (col.CompareTag("TowerCollider") || col.CompareTag("Tower")) {
                setValidPosition(false);
            }
            
            if (towerType == TowerType.Land) {
                if (col.CompareTag("GroundCollider"))
                    setValidPosition(true);
            }
            else if (towerType == TowerType.Water) {
                if (col.CompareTag("WaterCollider"))
                    setValidPosition(true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (isHeld) {
            Debug.Log("Stopped Colliding with: " + collision.gameObject.name);
            var col = collision.gameObject;
            
            if (col.CompareTag("TowerCollider") || col.CompareTag("Tower")) {
                setValidPosition(true);
            }

            if (towerType == TowerType.Land) {
                if (col.CompareTag("GroundCollider"))
                    setValidPosition(false);
            }
            else if (towerType == TowerType.Water) {
                if (col.CompareTag("WaterCollider"))
                    setValidPosition(false);
            }
        }
    }

    //
    //
    // UI REPRESENT
    //
    //

    public void setLineColorGrey() {
        var gradient = new Gradient();
        var tempColorKeys = radiusLine.colorGradient.colorKeys;
        for (int i = 0; i < tempColorKeys.Length; i++)
            tempColorKeys[i].color = Color.grey;
            
        gradient.colorKeys = tempColorKeys;
        radiusLine.colorGradient = gradient;
    }

    public void setLineColorRed() {
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
                points[i] = new Vector3(Mathf.Cos(angle) * getAttackRange(), Mathf.Sin(angle) * getAttackRange(), 0f) + transform.position;

            radiusLine.positionCount = points.Length;
            radiusLine.SetPositions(points);
        }
    }

    // reasons for this being called:
    // range upgrade
    // weather
    public void updateRadiusCircle() {
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
        Debug.Log("Deselect called on Tower");
        selected = false;
        hideRadiusCircle();
    }

    public void hardDeselect() {
        Debug.Log("Hard deselect called on Tower");
        selected = false;
        hideRadiusCircle();
        m_upgradeMenuInterface.exitButton();
    }

    public void select() {
        Debug.Log("Select called on Tower");
        selected = true;
        showRadiusCircle();
        m_upgradeMenuInterface.upgrade(this);
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
    
    public float getAttackSpeed() {
        return (float)(attackSpeed - (m_attackSpeedUpgradeLevel * 0.35));
    }

    public void setAttackSpeed(float val) {
        attackSpeed = val;
    }

    public float getAttackDamage() {
        return attackDamage + (m_damageUpgradeLevel * 15);
    }

    public void setAttackDamage(float val) {
        attackDamage = val;
    }

    public float getAttackRange() {
        return attackRange + (m_rangeUpgradeLevel * 10);
    }
    
    public void setAttackRange(float val) {
        attackRange = val;
    }

    
    //Getters and setters for the upgrade system.
    public int getRangeUpgradeLevel() {
        return m_rangeUpgradeLevel;
    }

    public void setRangeUpgradeLevel(int val)
    {
        if (val < 0 || val > 3)
            return;
        m_rangeUpgradeLevel = val;
        drawRadiusCircle();
        showRadiusCircle();
    }

    public int getAttackSpeedUpgradeLevel() {
        return m_attackSpeedUpgradeLevel;
    }

    public void setAttackSpeedUpgradeLevel(int val) {
        if (val < 0 || val > 3)
            return;
        m_attackSpeedUpgradeLevel = val;
    }

    public int getDamageUpgradeLevel() {
        return m_damageUpgradeLevel;
    }

    public void setDamageUpgradeLevel(int val) {
        if (val < 0 || val > 3)
            return;
        m_damageUpgradeLevel = val;
    }

    public int getHealth()
    {
        return m_health;
    }

    public void setHealth(int val)
    {
        if (val < 0)
            val = 0;

        if (val > 100)
            val = 100;

        m_health = val;
        m_healthBar.setHealth(m_health);
        
        if (m_health == 0)
            Destroy(this.gameObject);
    }

    // damage to tower
    public void takeDamage(int val) {
        Debug.Log("Taking damage");
        StartCoroutine(hit());
        setHealth(getHealth() - val);
    }

    IEnumerator hit() {
        Debug.Log("Tower called hit");
        var prevColor = spriteRotator.getRenderer().color;
        spriteRotator.getRenderer().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRotator.getRenderer().color = prevColor;
    }
    
    // tower gets healed by support tower
    public void heal(int val) {
        setHealth(getHealth() + val);
        // TODO: PLAY HEALED SOUND
    }

    private IEnumerator healTowers() {
        readyToHeal = false;
        attackType.heal(getAttackRange());
        yield return new WaitForSeconds(getAttackSpeed());
        readyToHeal = true;
    }

    // Was using this method to test to make sure that the healing was working
    // Diego Morales
    /*public void testHealth(int valueHealth){
        towerHealth += valueHealth;
    }*/
}
