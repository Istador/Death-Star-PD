using UnityEngine;
using UnityEngine.UI;

public class SelectionGUI : MonoBehaviour, MessageReceiver {

	Text damage;
	Text defense;
	Text range;
	Text firerate;

	Button up;
	Text upMoney;
	Text upCookies;

	//Button sell;
	Text sellMoney;
	Text sellCookies;

	void Start () {
		damage = transform.FindChild("dmgText").GetComponent<Text>();
		defense = transform.FindChild("defText").GetComponent<Text>();
		range = transform.FindChild("dexText").GetComponent<Text>();
		firerate = transform.FindChild("agiText").GetComponent<Text>();

		up = transform.FindChild("upButton").GetComponent<Button>();
		upMoney = transform.FindChild("upMoneyText").GetComponent<Text>();
		upCookies = transform.FindChild("upCookiesText").GetComponent<Text>();

		//sell = transform.FindChild("sellButton").GetComponent<Button>();
		sellMoney = transform.FindChild("sellMoneyText").GetComponent<Text>();
		sellCookies = transform.FindChild("sellCookiesText").GetComponent<Text>();

		Observer.I.Add("TowerSelectSelected", this);
		Observer.I.Add("MoneyChange", this);
		Observer.I.Add("CookieChange", this);
	}

	void OnDestroy(){
		Observer.I.Remove("TowerSelectSelected", this);
		Observer.I.Remove("MoneyChange", this);
		Observer.I.Remove("CookieChange", this);
	}

	public bool HandleMessage(Telegram msg){
		if(this == null) return true;
		switch(msg.message){
		case "TowerSelectSelected":
			Tower t = msg.extraInfo as Tower;
			if(t == null){
				gameObject.SetActive(false);
			} else {

				//Positioniere Panel über den Turm
				transform.position = Camera.main.WorldToScreenPoint(t.Pos);
				//fülle SelectPanel mit den Turmdaten des ausgewählten Turms
				damage.text = t.Damage.ToString();
				defense.text = t.MaxHealth.ToString();
				range.text = t.Range.ToString();
				firerate.text = t.AttackCooldown.ToString();

				upMoney.text = t.MoneyUpgradeCost.ToString();
				upCookies.text = t.CookieUpgradeCost.ToString();

				sellMoney.text = t.MoneyROI.ToString();
				sellCookies.text = t.CookieROI.ToString();

				CheckResources(t);

				//zeige das Panel an
				gameObject.SetActive(true);
			}
			return true;
		case "MoneyChange":
			if(TowerSelect.I.Selected != null){
				CheckResources(TowerSelect.I.Selected);
			}
			return true;
		case "CookieChange":
			if(TowerSelect.I.Selected != null){
				CheckResources(TowerSelect.I.Selected);
			}
			return true;
		default: return false;
		}
	}

	public void CheckResources(Tower t){
		up.interactable = 
			t.Level <3
			&& GameResources.I.EnoughMoney(t.MoneyUpgradeCost)
			&& GameResources.I.EnoughCookies(t.CookieUpgradeCost);
	}

	public void UpgradeTower(){
		TowerSelect.I.UpgradeTower();
	}

	public void SellTower(){
		TowerSelect.I.SellTower();
	}

}
