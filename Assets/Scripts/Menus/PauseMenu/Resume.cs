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

    [SerializeField]
    private GameObject deactPauseMenue;
    
    // Start is called before the first frame update
    
    public void OnPointerEnter(PointerEventData eventData)
    {   
            image.sprite = newSprite;
    }

    public void OnPointerExit(PointerEventData eventData){
            image.sprite = oldSprite;        
    }

    public void restResumeImage(){
        image.sprite = oldSprite;
    }



}
