using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthElementHandler : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider m_slider;

    public void setHealth(int val)
    {
        if (!hasAllReferences())
            return;

        if (val < 0)
            val = 0;
        
        if (val > 100)
            val = 100;
        
        m_slider.value =  (float)val / (float)100;
    } 
    
    private bool hasAllReferences()
    {
        if (m_slider == null)
        {
            Debug.Log("Missing required references in PlayerManager script.");
            return false;
        }

        return true;
    }
    
}
