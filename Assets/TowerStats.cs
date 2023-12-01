using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TowerStats : MonoBehaviour {
	
	public Tower tower;
	//public GameObject towerObj;
	public GameObject textObj;
	public TextMeshProUGUI text;

	void Awake() {
		text = textObj.GetComponent<TextMeshProUGUI>();
		//tower = towerObj.GetComponent<Tower>();
	}

	public void start() {
		StartCoroutine(update());
	}

	public void stop() {
		StopCoroutine(update());;
	}

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