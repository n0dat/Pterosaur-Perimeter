using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDropUIHandler : MonoBehaviour, IPointerEnterHandler
{
    //These variables MUST be set in unity for the script to work.
    [SerializeField] private GameObject towerContainer;
    private bool isExpanded;
    
    // Start is called before the first frame update
    void Start()
    {
        //Default when level loads will be expanded.
        this.isExpanded = true;
    }

    public void closeMenu()
    {
        if (!isExpanded)
            return;
        
        Animator animator = towerContainer.GetComponent<Animator>();
        this.isExpanded = false;
        animator.SetTrigger("HoveredWhileOpened");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Animator animator = towerContainer.GetComponent<Animator>();
        if (this.isExpanded)
        {
            this.isExpanded = false;
            animator.SetTrigger("HoveredWhileOpened");
        }
        else
        {
            this.isExpanded = true;
            animator.SetTrigger("HoveredWhileClosed");
        }
    }
}
