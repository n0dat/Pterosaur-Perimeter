using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUIHandler : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider m_slider;
    private int m_maxHealth = 100;

    public void setHealth(int val)
    {
        if (!hasAllReferences())
            return;

        if (val < 0)
            val = 0;
        
        if (val > m_maxHealth)
            val = m_maxHealth;
        
        m_slider.value =  (float)val / (float)m_maxHealth;
    } 
    
    private bool hasAllReferences()
    {
        if (m_slider == null)
        {
            Debug.Log("Missing required references in HealthBarUIHandler script.");
            return false;
        }

        return true;
    }

    public void setMaxHealth(int max)
    {
        m_maxHealth = max;
    }
}
