using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuHandler : MonoBehaviour
{
    public enum UpgradeType
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

    //Allows you to set all three upgrade levels at once. Exposed.
    public void setUpgradeLevels(int damageLevel, int rangeLevel, int speedLevel)
    {
        if (!hasAllReferences())
            return;
        
        setDamageUpgradeLevel(damageLevel);
        setRangeUpgradeLevel(rangeLevel);
        setSpeedUpgradeLevel(speedLevel);
    }
    //Handles upgrading a specific skill by one. Exposed.
    private void incrementUpgrade(UpgradeType type)
    {
        if (!hasAllReferences())
            return;
        
        switch (type)
        {
            case UpgradeType.Damage:
                setDamageUpgradeLevel(m_damageUpgradeLevel + 1);
                break;
            case UpgradeType.Range:
                setRangeUpgradeLevel(m_rangeUpgradeLevel + 1);
                break;
            case UpgradeType.Speed:
                setSpeedUpgradeLevel(m_speedUpgradeLevel + 1);
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
        if (!m_damageElementTicks || !m_rangeElementTicks || !m_speedElementTicks || !m_levelManager)
        {
            Debug.Log("Missing required references in TowerDropper script.");
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
        
        incrementUpgrade(UpgradeType.Damage);
    }

    public void upgradeRangeButton()
    {
        if (m_rangeUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgrade(UpgradeType.Range);
    }

    public void upgradeSpeedButton()
    {
        if (m_speedUpgradeLevel >= 3)
            return;

        if (!m_levelManager.skullsCost(100))
            return;
        
        incrementUpgrade(UpgradeType.Speed);
    }
}
