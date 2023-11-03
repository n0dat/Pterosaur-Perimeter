using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuHandler : MonoBehaviour
{
    public void Update()
    {
        if (!m_currentTower)
            return;

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

    //Exposed api for the tower script to interface with.
    public void upgrade(Tower currentTower)
    {
        m_currentTower = currentTower;
        if (!hasAllReferences())
            return;
        
        setUpgradeLevels(currentTower.getDamageUpgradeLevel(), currentTower.getRangeUpgradeLevel(), currentTower.getAttackSpeedUpgradeLevel());
        m_healthValue = m_currentTower.getHealth();
        m_healthElementHandler.setHealth(m_healthValue);
        
        if (m_isOpen)
            return;
        
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
            default:
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
        if (!m_healthElementHandler || !m_damageElementTicks || !m_rangeElementTicks || !m_speedElementTicks || !m_levelManager || !m_currentTower || !m_animator)
        {
            Debug.Log("Missing required references in UpgradeMenuHandlerScript script.");
            return false;
        }
        return true;
    }

    public void upgradeDamageButton()
    {
        if (m_damageUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgradeForTower(UpgradeType.Damage);
    }

    public void upgradeRangeButton()
    {
        if (m_rangeUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgradeForTower(UpgradeType.Range);
    }

    public void upgradeSpeedButton()
    {
        if (m_speedUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgradeForTower(UpgradeType.Speed);
    }

    public void healButton()
    {
        if (!hasAllReferences() || m_healthValue >= 100)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        m_healthValue = 100;
        m_healthElementHandler.setHealth(m_healthValue);
        m_currentTower.setHealth(m_healthValue);
    }

    public void exitButton()
    {
        if (!hasAllReferences())
            return;

        m_currentTower = null;
        m_isOpen = false;
        m_animator.SetTrigger("slideIn");
    }
}
