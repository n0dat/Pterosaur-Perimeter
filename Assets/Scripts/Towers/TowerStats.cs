using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TowerStats : MonoBehaviour {
	
	// globals
	public Tower tower;
	public GameObject textObj;
	public TextMeshProUGUI text;
    
	// called before first update
	void Awake() {
		text = textObj.GetComponent<TextMeshProUGUI>();
	}

	// start showing stats
	public void start() {
		StartCoroutine(update());
	}

	// stop showing stats
	public void stop() {
		StopCoroutine(update());;
	}

	// update stats ui every 100 milliseconds (1/10th second)
	IEnumerator update() {
		while (tower) {
			var ret = "Health\t" + tower.getHealth() + "\n";
			ret += "Power\t" + tower.getAttackDamage() + "\n";
			ret += "Range\t" + tower.getAttackRange() + "\n";
			ret += "Speed\t" + MathF.Round(1 / tower.getAttackSpeed(), 2) + "/s\n";
			ret += "Next Move\t" + (tower.attackCountdown < 0.05f ? 0 : MathF.Round(tower.attackCountdown, 1)) + "s";
			text.SetText(ret);
			
			yield return new WaitForSeconds(0.1f);
		}
	}
}