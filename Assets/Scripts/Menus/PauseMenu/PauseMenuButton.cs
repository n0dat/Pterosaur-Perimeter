using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResumeMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler  {
	
	[SerializeField] private Image image;
	[SerializeField] private Sprite newSprite, oldSprite;
    
    // swap sprite on click
	public void OnPointerEnter(PointerEventData eventData) {
		image.sprite = newSprite;
	}

	// swap sprite on click
	public void OnPointerExit(PointerEventData eventData) {
		image.sprite = oldSprite;        
	}

	// reset the sprite after being clicked (new scene)
	public void resetSprite() {
		image.sprite = oldSprite;
	}
}