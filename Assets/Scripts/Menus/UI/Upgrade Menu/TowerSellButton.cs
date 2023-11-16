using TMPro;
using UnityEngine;

public class TowerSellButton : MonoBehaviour {
	
	public TextMeshProUGUI text;
	public UpgradeMenuHandler upgradeMenuHandler;
	public PlayerManager playerManager;
	public Tower tower;

	public void setValue(int value) {
		text.SetText("$" + value);
	}

	public void sellTower() {
		tower = upgradeMenuHandler.getTower();
		if (tower) {
			playerManager.skullsCredit(tower.getValue());
			upgradeMenuHandler.exitButton();
			Destroy(tower.gameObject);
		}
	}
}
