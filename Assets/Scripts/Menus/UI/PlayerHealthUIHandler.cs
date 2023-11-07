using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUIHandler : MonoBehaviour
{
    [SerializeField] private EggsIconHandler m_eggsHandler;

    public void setHealthLevel(int val)
    {
        if (!hasAllReferences() || val < 0 || val > 3)
            return;
        m_eggsHandler.setEggsLevel(val);
    }
    
    private bool hasAllReferences()
    {
        if (!m_eggsHandler)
        {
            Debug.Log("Missing required references in EggIconHandler script.");
            return false;
        }
        return true;
    }
}
