using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuHandler : MonoBehaviour
{
    //Keeps health updated.
    public void Update()
    {
        if (!m_currentTower)
        {
            if (m_isOpen)
            {
                m_isOpen = false;
                exitButton();
            }
            return;
        }
            

        if (m_currentTower.getHealth() == m_healthValue)
            return;

        m_healthValue = m_currentTower.getHealth();
        m_healthElementHandler.setHealth(m_healthValue);
    }
    private enum UpgradeType
    {
        Damage,
        Range,
        Speed
    };
    
    //Below variables are references to the tick containers. For handling logic that turns them orange.
    [SerializeField] private TickMarkHandler m_damageElementTicks;
    private int m_damageUpgradeLevel = 0;
    
    [SerializeField] private TickMarkHandler m_rangeElementTicks;
    private int m_rangeUpgradeLevel = 0;
    
    [SerializeField] private TickMarkHandler m_speedElementTicks;
    private int m_speedUpgradeLevel = 0;

    [SerializeField] private PlayerManager m_levelManager; //For determining purchases.
    private Tower m_currentTower = null;

    [SerializeField] private HealthElementHandler m_healthElementHandler;
    private int m_healthValue;

    [SerializeField] private Animator m_animator;
    
    private bool m_isOpen = false;
    
    [SerializeField] private TowerSellButton m_towerSellButton;

    //Exposed api for the tower script to interface with.
    public void upgrade(Tower currentTower)
    {
        
        //Debug.Log("Upgrade called in upgrade menu handler");
        m_currentTower = currentTower;
        if (!hasAllReferences())
            return;
        
        m_currentTower.calculateValue();
        m_towerSellButton.setValue(m_currentTower.getValue());
        
        setUpgradeLevels(currentTower.getDamageUpgradeLevel(), currentTower.getRangeUpgradeLevel(), currentTower.getAttackSpeedUpgradeLevel());
        m_healthValue = m_currentTower.getHealth();
        m_healthElementHandler.setHealth(m_healthValue);
        
        if (m_isOpen)
            return;
        
        m_animator.ResetTrigger("slideIn");
        m_animator.SetTrigger("slideOut");
        m_isOpen = true;
    }
    
    //Allows you to set all three upgrade levels at once.
    private void setUpgradeLevels(int damageLevel, int rangeLevel, int speedLevel)
    {
        if (!hasAllReferences())
            return;
        
        setDamageUpgradeLevel(damageLevel);
        setRangeUpgradeLevel(rangeLevel);
        setSpeedUpgradeLevel(speedLevel);
    }
    
    //Handles upgrading a specific skill by one. This is where calls from towers will be fowarded too.
    private void incrementUpgradeForTower(UpgradeType type)
    {
        if (!hasAllReferences())
            return;
        
        if (!m_currentTower)
            Debug.Log("Missing the tower ref, you gonna get a null noob");
        
        switch (type)
        {
            case UpgradeType.Damage:
                setDamageUpgradeLevel(m_damageUpgradeLevel + 1);
                m_currentTower.setDamageUpgradeLevel(m_damageUpgradeLevel);
                break;
            case UpgradeType.Range:
                setRangeUpgradeLevel(m_rangeUpgradeLevel + 1);
                m_currentTower.setRangeUpgradeLevel(m_rangeUpgradeLevel);
                break;
            case UpgradeType.Speed:
                setSpeedUpgradeLevel(m_speedUpgradeLevel + 1);
                m_currentTower.setAttackSpeedUpgradeLevel(m_speedUpgradeLevel);
                break;
        }
    }

    private void setDamageUpgradeLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel > 3)
            return;
        m_damageUpgradeLevel = newLevel;
        m_damageElementTicks.setTicksLevel(newLevel);
    }
    
    private void setRangeUpgradeLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel > 3)
            return;
        m_rangeUpgradeLevel = newLevel;
        m_rangeElementTicks.setTicksLevel(newLevel);
    }
    
    private void setSpeedUpgradeLevel(int newLevel)
    {
        if (newLevel < 0 || newLevel > 3)
            return;
        m_speedUpgradeLevel = newLevel;
        m_speedElementTicks.setTicksLevel(newLevel);
    }
    
    private bool hasAllReferences()
    {
        /*
        if (!m_healthElementHandler || !m_damageElementTicks || !m_rangeElementTicks || !m_speedElementTicks || !m_levelManager || !m_currentTower || !m_animator)
        {
            if (!m_currentTower)
                Debug.Log("Missing the tower reference");
            Debug.Log("Missing required references in UpgradeMenuHandlerScript script. Nobody knows which lol.");
            return false;
        }
        */
        return true;
    }

    //Called by the upgrade damage button. Will handle the logic for upgrading dino damage.
    public void upgradeDamageButton()
    {
        if (!hasAllReferences())
            return;
        
        if (m_damageUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgradeForTower(UpgradeType.Damage);
        
        m_currentTower.calculateValue();
        m_towerSellButton.setValue(m_currentTower.getValue());
    }

    //Called by the upgrade range button. Will handle the logic for upgrading dino range.
    public void upgradeRangeButton()
    {
        if (!hasAllReferences())
            return;
        
        if (m_rangeUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgradeForTower(UpgradeType.Range);
        
        m_currentTower.calculateValue();
        m_towerSellButton.setValue(m_currentTower.getValue());
    }

    //Called by the upgrade speed button. Will handle the logic for upgrading dino speed.
    public void upgradeSpeedButton()
    {
        if (!hasAllReferences())
            return;
        
        if (m_speedUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgradeForTower(UpgradeType.Speed);
        
        m_currentTower.calculateValue();
        m_towerSellButton.setValue(m_currentTower.getValue());
    }

    //Handles healing logic.
    public void healButton()
    {
        if (!hasAllReferences() || m_healthValue >= 100)
            return;

        if (!m_levelManager.skullsCost(200))
            return;
        
        m_healthValue = 100;
        m_healthElementHandler.setHealth(m_healthValue);
        m_currentTower.setHealth(m_healthValue);
    }

    //Will close the upgrade menu.
    public void exitButton()
    {
        if (!hasAllReferences())
            return;
        
        Debug.Log("exitButton Called");
        m_currentTower = null;
        m_isOpen = false;
        
        m_animator.ResetTrigger("slideOut");
        m_animator.SetTrigger("slideIn");
    }

    public Tower getTower() {
        return m_currentTower;
    }

    public bool isOpen() {
        return m_isOpen;
    }

}
