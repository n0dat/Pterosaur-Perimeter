using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class SettingsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    public UnityEngine.UI.Image img;
    public Sprite newSprite;
    public Sprite oldSprite;

    private bool isHover;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isHover = false;
    }

    public void OnPointerEnter (PointerEventData eventData) {
        img.sprite = newSprite;
    }

    public void OnPointerExit(PointerEventData eventData){
        img.sprite = oldSprite;
    }
}
