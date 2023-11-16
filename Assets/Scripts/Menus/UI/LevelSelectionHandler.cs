using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionHandler : MonoBehaviour
{
    [SerializeField] private List<LevelSelectButton> m_levelSelectionButtons;

    public void Start()
    {
        unlockTier(5);
    }
    
    public void unlockTier(int tier)
    {
        if (tier < 0)
            return;

        if (tier > m_levelSelectionButtons.Count)
            tier = m_levelSelectionButtons.Count;

        int i = 0;
        for (; i < tier; i++)
        {
            m_levelSelectionButtons[i].unlockLevel();
        }

        for (; i < m_levelSelectionButtons.Count; i++)
        {
            m_levelSelectionButtons[i].lockLevel();
        }
    }
}
