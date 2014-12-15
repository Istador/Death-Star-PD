using UnityEngine;
using UnityEngine.UI;

public class TowerBuildGUI : MonoBehaviour, MessageReceiver {

	private Button t1;
	private Button t2;
	private Button t3;
	private Button t4;
	private Button t5;

	void Start() {
		t1 = transform.FindChild("MGTower Button").GetComponent<Button>();
		t2 = transform.FindChild("LaserTower Button").GetComponent<Button>();
		t3 = transform.FindChild("RocketTower Button").GetComponent<Button>();
		t4 = transform.FindChild("LightningTower Button").GetComponent<Button>();
		t5 = transform.FindChild("SupportTower Button").GetComponent<Button>();

		t1.transform.FindChild("RessourceCostText").GetComponent<Text>().text = MGTower.money_table[0].ToString();
		t1.transform.FindChild("EnergyCostText").GetComponent<Text>().text = MGTower.cookie_table[0].ToString();

		t2.transform.FindChild("RessourceCostText").GetComponent<Text>().text = LaserTower.money_table[0].ToString();
		t2.transform.FindChild("EnergyCostText").GetComponent<Text>().text = LaserTower.cookie_table[0].ToString();

		t3.transform.FindChild("RessourceCostText").GetComponent<Text>().text = RocketTower.money_table[0].ToString();
		t3.transform.FindChild("EnergyCostText").GetComponent<Text>().text = RocketTower.cookie_table[0].ToString();

		t4.transform.FindChild("RessourceCostText").GetComponent<Text>().text = LightningTower.money_table[0].ToString();
		t4.transform.FindChild("EnergyCostText").GetComponent<Text>().text = LightningTower.cookie_table[0].ToString();

		t5.transform.FindChild("RessourceCostText").GetComponent<Text>().text = SupportTower.money_table[0].ToString();
		t5.transform.FindChild("EnergyCostText").GetComponent<Text>().text = SupportTower.cookie_table[0].ToString();


		object m = Observer.I.Add("MoneyChange", this);
		object c = Observer.I.Add("CookieChange", this);
		if(m != null) ChangeMoney((int)m);
		if(c != null) ChangeCookies((int)c);
	}

	private void ChangeMoney(int m){
		int c = GameResources.I.Cookies;
		ChangeBoth(m, c);
	}

	private void ChangeCookies(int c){
		int m = GameResources.I.Money;
		ChangeBoth(m, c);
	}

	private void ChangeBoth(int m, int c){
		Check(t1, m, c, MGTower.money_table[0], MGTower.cookie_table[0]);
		Check(t2, m, c, LaserTower.money_table[0], LaserTower.cookie_table[0]);
		Check(t3, m, c, RocketTower.money_table[0], RocketTower.cookie_table[0]);
		Check(t4, m, c, LightningTower.money_table[0], LightningTower.cookie_table[0]);
		Check(t5, m, c, SupportTower.money_table[0], SupportTower.cookie_table[0]);
	}

	private void Check(Button b, int m, int c, int mn, int cn){
		b.interactable = (mn <=m) && (cn <= c);
	}
	
	public bool HandleMessage(Telegram msg){
		if(this == null) return true;
		switch(msg.message){
		case "MoneyChange":
			ChangeMoney((int) msg.extraInfo);
			return true;
		case "CookieChange":
			ChangeCookies((int) msg.extraInfo);
			return true;
		default: return false;
		}
	}
}
