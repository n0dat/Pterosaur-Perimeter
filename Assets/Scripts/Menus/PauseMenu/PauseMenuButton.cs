using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResumeMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	
	[SerializeField] private Image image;
	[SerializeField] private Sprite newSprite, oldSprite;
    
	public void OnPointerEnter(PointerEventData eventData) {
		image.sprite = newSprite;
	}

	public void OnPointerExit(PointerEventData eventData) {
		image.sprite = oldSprite;        
	}

	public void resetSprite() {
		image.sprite = oldSprite;
	}
}