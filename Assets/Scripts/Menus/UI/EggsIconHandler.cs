using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EggsIconHandler : MonoBehaviour
{
    //Set references to one of three egg objects in the UI.
    [SerializeField] private GameObject m_egg1;
    [SerializeField] private GameObject m_egg2;
    [SerializeField] private GameObject m_egg3;

    [SerializeField] private Sprite m_fullEgg;
    [SerializeField] private Sprite m_halfEgg;

    //Exposed function for setting the egg level.
    public void setEggsLevel(int level)
    {
        if (!hasAllReferences() || level < 0 || level > 3)
            return;

        Image image1 = m_egg1.GetComponent<Image>();
        Image image2 = m_egg2.GetComponent<Image>();
        Image image3 = m_egg3.GetComponent<Image>();
        
        setAllFull();

        switch (level)
        {
            case 0:
                setAllHalf();
                break;
            case 1:
                image2.sprite = m_halfEgg;
                image3.sprite = m_halfEgg;
                break;
            case 2:
                image3.sprite = m_halfEgg;
                break;
            default:
                break;
        }
    }
    
    private bool hasAllReferences()
    {
        if (!m_egg1 || !m_egg2 || !m_egg3 || !m_fullEgg || !m_halfEgg)
        {
            Debug.Log("Missing required references in EggIconHandler script.");
            return false;
        }
        return true;
    }

    //Set all eggs to full.
    private void setAllFull()
    {
        Image image1 = m_egg1.GetComponent<Image>();
        Image image2 = m_egg2.GetComponent<Image>();
        Image image3 = m_egg3.GetComponent<Image>();

        image1.sprite = m_fullEgg;
        image2.sprite = m_fullEgg;
        image3.sprite = m_fullEgg;
    }
    
    //Set all eggs to half.
    private void setAllHalf()
    {
        Image image1 = m_egg1.GetComponent<Image>();
        Image image2 = m_egg2.GetComponent<Image>();
        Image image3 = m_egg3.GetComponent<Image>();

        image1.sprite = m_halfEgg;
        image2.sprite = m_halfEgg;
        image3.sprite = m_halfEgg;
    }
}
