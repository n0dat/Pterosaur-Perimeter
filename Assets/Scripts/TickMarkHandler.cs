using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickMarkHandler : MonoBehaviour
{
    //Set references to one of three ticks objects in the UI.
    [SerializeField] private GameObject m_tick1;
    [SerializeField] private GameObject m_tick2;
    [SerializeField] private GameObject m_tick3;

    [SerializeField] private Sprite m_greyCircle;
    [SerializeField] private Sprite m_orangeCircle;

    //Exposed function for setting the tick level.
    public void setTicksLevel(int level)
    {
        if (!hasAllReferences() || level < 0 || level > 3)
            return;

        Image image1 = m_tick1.GetComponent<Image>();
        Image image2 = m_tick2.GetComponent<Image>();
        Image image3 = m_tick3.GetComponent<Image>();
        
        setAllGrey();

        switch (level)
        {
            case 1:
                image1.sprite = m_orangeCircle;
                break;
            case 2:
                image1.sprite = m_orangeCircle;
                image2.sprite = m_orangeCircle;
                break;
            case 3:
                setAllOrange();
                break;
            default:
                break;
        }
    }
    
    private bool hasAllReferences()
    {
        if (!m_tick1 || !m_tick2 || !m_tick3 || !m_greyCircle || !m_orangeCircle)
        {
            Debug.Log("Missing required references in TowerDropper script.");
            return false;
        }
        return true;
    }

    //Set all tick marks to grey.
    private void setAllGrey()
    {
        Image image1 = m_tick1.GetComponent<Image>();
        Image image2 = m_tick2.GetComponent<Image>();
        Image image3 = m_tick3.GetComponent<Image>();

        image1.sprite = m_greyCircle;
        image2.sprite = m_greyCircle;
        image3.sprite = m_greyCircle;
    }
    
    //Set all tick marks to orange.
    private void setAllOrange()
    {
        Image image1 = m_tick1.GetComponent<Image>();
        Image image2 = m_tick2.GetComponent<Image>();
        Image image3 = m_tick3.GetComponent<Image>();

        image1.sprite = m_orangeCircle;
        image2.sprite = m_orangeCircle;
        image3.sprite = m_orangeCircle;
    }
}
