using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDropUIHandler : MonoBehaviour
{
    //These variables MUST be set in unity for the script to work.
    [SerializeField] private GameObject towerContainer;
    [SerializeField] private int slideDelta;
    private bool isExpanded;
    
    // Start is called before the first frame update
    void Start()
    {
        //Default when level loads will be expanded.
        this.isExpanded = true;
    }

    //Called when the mouse hovers over expandHandle.
    public void OnMouseEnter()
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
