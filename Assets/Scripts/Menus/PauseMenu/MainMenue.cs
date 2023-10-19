using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MainMenue : MonoBehaviour, IPointerEnterHandler ,IPointerExitHandler
{
    public Image img;
    [SerializeField]
    private Sprite newSprite;
    [SerializeField]
    private Sprite oldSprite;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData){
        img.sprite = newSprite;
    }

    public void OnPointerExit(PointerEventData eventData){
        img.sprite = oldSprite;
    }

    public void resetSprite() {
        img.sprite = oldSprite;
    }



    




}
