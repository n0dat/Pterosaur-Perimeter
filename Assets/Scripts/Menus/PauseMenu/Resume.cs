using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Resume : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public Sprite newSprite;
    public Sprite oldSprite;

    // Need to have clicked bool here
    
    // Start is called before the first frame update
    void Start()
    {
        isHover = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
            image.sprite = newSprite;
    }

    public void OnPointerExit(PointerEventData eventData){
            image.sprite = oldSprite;        
    }
}
