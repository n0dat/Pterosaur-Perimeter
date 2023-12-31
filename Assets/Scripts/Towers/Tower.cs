using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour {
    
    // globalss
    public enum TowerType { Water, Land }
    private enum AttackState { Attacking, Waiting, Initial };
    private enum TargetingMode { First, Last, Strong };
    
    [SerializeField] private Animator m_animator;

    // member vars
    [SerializeField] private int towerCost, repairCost, radiusLineSegments = 50, killCount;
    [SerializeField] private float attackSpeed, attackDamage, attackRange;
    [SerializeField] private bool isHeld, selected, validPosition, attackInProgress;
    
    // references
    [SerializeField] private LineRenderer radiusLine;
    [SerializeField] private Collider2D[] enemiesInRange;
    [SerializeField] private GameObject enemyToAttack;
    [SerializeField] private AttackState attackState = AttackState.Initial;
    [SerializeField] private TargetingMode targetingMode = TargetingMode.First;
    [SerializeField] private AttackType attackType;
    [SerializeField] private TowerType towerType;
    [SerializeField] private SpriteRotator spriteRotator;
    [SerializeField] private LevelManager levelManager;
    
    public Color radiusColor;
    
    // upgrade system
    private UpgradeMenuHandler m_upgradeMenuInterface;
    private int m_damageUpgradeLevel = 0;
    private int m_rangeUpgradeLevel = 0;
    private int m_attackSpeedUpgradeLevel = 0;
    private int m_health = 100;
    [SerializeField] private int m_value = 0;

    // ui
    [SerializeField] private HealthUIHandler m_healthBar;
    
    // tower healing
    [SerializeField] private bool isHealingTower = false, readyToHeal = true;

    [SerializeField] private TowerAudioSpawner m_audioHandler;
    
    // tower stats
    public float attackCountdown = 0f;
    public TowerStats towerStats;
    public GameObject towerStatsObj;
    public bool hasStats = false;
    
    
    // tower selection
    public bool readyToBeSelected = false;
    
    // initialize most global values
    void Awake() {
        repairCost = 0;
        killCount = 0;
        
        isHeld = false;
        selected = false;
        validPosition = true;
        attackInProgress = false;
        radiusLine.enabled = true;
        
        towerStats = towerStatsObj.GetComponent<TowerStats>();
        towerStats.tower = GetComponent<Tower>();
        towerStatsObj.SetActive(false);
        
        setLineColorGrey();
        drawRadiusCircle();
        
        enemiesInRange = Array.Empty<Collider2D>();
        enemyToAttack = null;
        
        spriteRotator = gameObject.GetComponent<SpriteRotator>();
        
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        m_upgradeMenuInterface = GameObject.Find("UpgradeMenu").GetComponent<UpgradeMenuHandler>(); //Must be called UpgradeMenu.
        deselect();
        
        StartCoroutine(spawnDelay());
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
                if (enemiesInRange.Length != initLength) {
                    Debug.Log("Enemies in range changed in length, breaking out");
                    break;
                }
                
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
            
            var speed = getAttackSpeed();
            var remain = speed % 0.1f;
            attackCountdown = speed;
            
            for (int i = 0; i < speed / 0.1; i++) {
                yield return new WaitForSeconds(0.1f);
                attackCountdown -= 0.1f;
            }
            
            yield return new WaitForSeconds(remain);
            attackInProgress = false;
        }
    }

    private IEnumerator finishAttack(WaitForAttack waitForAttack) {
        attackInProgress = true;
        attackType.attack(enemyToAttack, gameObject);
        m_audioHandler.shoot();
        if (m_animator)
            m_animator.SetTrigger("shoot");
        waitForAttack.setAttackFinished();
        yield return null;
    }
    // Gets enemies in a radius of tower to attack
    private void getEnemiesInRadius() {
        var temp = new List<Collider2D>();
        foreach (var enemy in Physics2D.OverlapCircleAll(transform.position, getAttackRange()))
            if (enemy.gameObject.CompareTag("Enemy"))
                temp.Add(enemy);
        
        enemiesInRange = temp.ToArray();
    }
    // The enemy was killed by the tower
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
            //Debug.Log("Colliding with: " + collision.gameObject.name);
            var col = collision.gameObject;

            if (col.CompareTag("TowerCollider") || col.CompareTag("Tower")) {
                setValidPosition(false);
                return;
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
            //Debug.Log("Stopped Colliding with: " + collision.gameObject.name);
            var col = collision.gameObject;
            
            if (col.CompareTag("TowerCollider") || col.CompareTag("Tower")) {
                setValidPosition(true);
                return;
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
    // Sets the raidues color of grey to show that it is a valid loaction to place tower
    public void setLineColorGrey() {
        var gradient = new Gradient();
        var tempColorKeys = radiusLine.colorGradient.colorKeys;
        for (int i = 0; i < tempColorKeys.Length; i++)
            tempColorKeys[i].color = new Color(.5f, .5f, 1f, 0.05f);
            
        gradient.colorKeys = tempColorKeys;
        radiusLine.colorGradient = gradient;
    }
    // Sets the color of the radius to red if invalid location of tower
    public void setLineColorRed() {
        var gradient = new Gradient();
        var tempColorKeys = radiusLine.colorGradient.colorKeys;
        for (int i = 0; i < tempColorKeys.Length; i++)
            tempColorKeys[i].color = Color.red;
            
        gradient.colorKeys = tempColorKeys;
        radiusLine.colorGradient = gradient;
    }
    // This method draws the radius around the tower
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
    // return a bool if the tower is being held 
    public bool beingHeld() {
        return isHeld;
    }
    // return a bool if the tower is selected
    public bool isSelected() {
        return selected;
    }
    // Deselect the tower that was selected form dino tower UI
    public void deselect() {
        //Debug.Log("Deselect called on Tower");
        selected = false;
        hideRadiusCircle();
    }

    public void hardDeselect() {
        //Debug.Log("Hard deselect called on Tower");
        selected = false;
        hideRadiusCircle();
        m_upgradeMenuInterface.exitButton();
    }
    // the tower has been selected
    public void select() {
        //Debug.Log("Select called on Tower");
        selected = true;
        showRadiusCircle();
        m_upgradeMenuInterface.upgrade(this);
    }
    // the tower is held 
    public void holdTower() {
        isHeld = true;
        showRadiusCircle();
    }
    // the tower has been droped on the map 
    public void dropTower() {
        isHeld = false;
        hideRadiusCircle();
    }
    // sets the radius color based on postion is valid for tower 
    private void setRadiusColor() {
        if (validPosition)
            setLineColorGrey();
        else
            setLineColorRed();
    }
    // return a boolean if tower position is valid 
    public bool isValidPosition() {
        return validPosition;
    }
    // sets that valid position with a bool value
    public void setValidPosition(bool val) {
        validPosition = val;
    }
    // show the radius of tower if the enaable is set to true
    public void showRadiusCircle() {
        radiusLine.enabled = true;
    }
    // hides the radius if enabled is set to false
    public void hideRadiusCircle() {
        radiusLine.enabled = false;
    }

    //
    //
    // GETTERS AND SETTERS
    //
    //
    // return a int of the tower cost
    public int getTowerCost() {
        return towerCost;
    }
    // sets the tower cost
    public void setTowerCost(int val) {
        towerCost = val;
    }
    // returns an in of the tower repair cost
    public int getRepairCost() {
        return repairCost;
    }
    // sets the repair cost of a tower
    public void setRepairCost(int val) {
        repairCost = val;
    }
    // return the float attack speed of a tower
    public float getAttackSpeed() {
        return (float)(attackSpeed - (m_attackSpeedUpgradeLevel * 0.35));
    }
    // sets the attack speed of tower
    public void setAttackSpeed(float val) {
        attackSpeed = val;
    }
    // gets the attack damage of tower by returning a float
    public float getAttackDamage() {
        return attackDamage + (m_damageUpgradeLevel * 15);
    }
    // sets the attack damage of a tower
    public void setAttackDamage(float val) {
        attackDamage = val;
    }
    // return a float value of attack range
    public float getAttackRange() {
        return attackRange + (m_rangeUpgradeLevel * 8);
    }
    // sets the attack range
    public void setAttackRange(float val) {
        attackRange = val;
    }
    // return a bool if the tower is held
    public bool getIsHeld() {
        return isHeld;
    }


    //Getters and setters for the upgrade system.
    public int getRangeUpgradeLevel() {
        return m_rangeUpgradeLevel;
    }
    // sets the upgrade range of tower
    public void setRangeUpgradeLevel(int val)
    {
        if (val < 0 || val > 3)
            return;
        m_rangeUpgradeLevel = val;
        drawRadiusCircle();
        showRadiusCircle();
    }
    // gets the attack speed of upgrade tower
    public int getAttackSpeedUpgradeLevel() {
        return m_attackSpeedUpgradeLevel;
    }
    // sets the attack speed of upgrade tower
    public void setAttackSpeedUpgradeLevel(int val) {
        if (val < 0 || val > 3)
            return;
        m_attackSpeedUpgradeLevel = val;
    }
    // gets the damage level of tower upgrade
    public int getDamageUpgradeLevel() {
        return m_damageUpgradeLevel;
    }
    // sets the damange level of upgrade tower
    public void setDamageUpgradeLevel(int val) {
        if (val < 0 || val > 3)
            return;
        m_damageUpgradeLevel = val;
    }
    // get tower health
    public int getHealth()
    {
        return m_health;
    }
    // sets the tower health
    public void setHealth(int val)
    {
        if (val < 0)
            val = 0;

        if (val > 100)
            val = 100;

        m_health = val;
        m_healthBar.setHealth(m_health);
        
        if (m_health == 0)
            Destroy(gameObject);
    }

    // damage to tower
    public void takeDamage(int val) {
        //if (isHeld)
        //    return;
        
        StartCoroutine(hit());
        setHealth(getHealth() - val);
    }

    IEnumerator hit() {
        spriteRotator.getRenderer().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRotator.getRenderer().color = Color.white;
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
    
    // tower value calculation for sell button
    public void calculateValue() {
        //Debug.Log("Damage Upgrade Level: " + m_damageUpgradeLevel);
        //Debug.Log("Range Upgrade Level: " + m_rangeUpgradeLevel);
        //Debug.Log("Attack Speed Upgrade Level: " + m_attackSpeedUpgradeLevel);
        m_value = (int) (towerCost * 0.8f + m_damageUpgradeLevel * 40 + m_rangeUpgradeLevel * 40 + m_attackSpeedUpgradeLevel * 40);
    }

    public int getValue() {
        return m_value;
    }

    
    
    // tower statistics
    // Show the stats with the SetActive to true
    public void showStats() {
        towerStatsObj.SetActive(true);
        towerStats.start();
        hasStats = true;
    }
    // Hides the states with setactive method set to false
    public void hideStats() {
        towerStatsObj.SetActive(false);
        towerStats.stop();
        hasStats = false;
    }
    // return a bool to show stats 
    public bool showingStats() {
        return hasStats;
    }
    
    // disaster death
    public void kill() {
        Destroy(this);
    }
    
    // tower timer before being able to run upgrade menu
    private IEnumerator spawnDelay() {
        yield return new WaitForSeconds(0.2f);
        readyToBeSelected = true;
    }
}
