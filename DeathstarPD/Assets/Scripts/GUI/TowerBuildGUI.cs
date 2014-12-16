using UnityEngine;
using UnityEngine.UI;

public class TowerBuildGUI : MonoBehaviour, MessageReceiver {

	private Color cNormal;
	private Color cHighlight;

	private Button b1;
	private Button b2;
	private Button b3;
	private Button b4;
	private Button b5;

	private Image i1;
	private Image i2;
	private Image i3;
	private Image i4;
	private Image i5;

	private Text desc;

	private Text money;
	private Image moneyImg;

	private Text cookies;
	private Image cookiesImg;


	void Start() {
		cNormal = new Color(1f, 1f, 1f, 0.6f);
		cHighlight = new Color(0.6f, 1f, 0.6f, 0.6f);

		Transform t1 = transform.FindChild("MGTower Button");
		Transform t2 = transform.FindChild("LaserTower Button");
		Transform t3 = transform.FindChild("LightningTower Button");
		Transform t4 = transform.FindChild("RocketTower Button");
		Transform t5 = transform.FindChild("SupportTower Button");

		b1 = t1.GetComponent<Button>();
		b2 = t2.GetComponent<Button>();
		b3 = t3.GetComponent<Button>();
		b4 = t4.GetComponent<Button>();
		b5 = t5.GetComponent<Button>();

		i1 = t1.GetComponent<Image>();
		i2 = t2.GetComponent<Image>();
		i3 = t3.GetComponent<Image>();
		i4 = t4.GetComponent<Image>();
		i5 = t5.GetComponent<Image>();

		desc = transform.FindChild("TowerDescriptionText").GetComponent<Text>();

		money = transform.FindChild("moneyText").GetComponent<Text>();
		moneyImg = transform.FindChild("moneyImage").GetComponent<Image>();

		cookies = transform.FindChild("cookiesText").GetComponent<Text>();
		cookiesImg = transform.FindChild("cookiesImage").GetComponent<Image>();

		EnableDesc(false);

		/*
		t1.transform.FindChild("RessourceCostText").GetComponent<Text>().text = MGTower.money_table[0].ToString();
		t1.transform.FindChild("EnergyCostText").GetComponent<Text>().text = MGTower.cookie_table[0].ToString();

		t2.transform.FindChild("RessourceCostText").GetComponent<Text>().text = LaserTower.money_table[0].ToString();
		t2.transform.FindChild("EnergyCostText").GetComponent<Text>().text = LaserTower.cookie_table[0].ToString();

		t3.transform.FindChild("RessourceCostText").GetComponent<Text>().text = LightningTower.money_table[0].ToString();
		t3.transform.FindChild("EnergyCostText").GetComponent<Text>().text = LightningTower.cookie_table[0].ToString();

		t4.transform.FindChild("RessourceCostText").GetComponent<Text>().text = RocketTower.money_table[0].ToString();
		t4.transform.FindChild("EnergyCostText").GetComponent<Text>().text = RocketTower.cookie_table[0].ToString();

		t5.transform.FindChild("RessourceCostText").GetComponent<Text>().text = SupportTower.money_table[0].ToString();
		t5.transform.FindChild("EnergyCostText").GetComponent<Text>().text = SupportTower.cookie_table[0].ToString();
		*/

		Observer.I.Add("TowerBuildSelected", this);
		object m = Observer.I.Add("MoneyChange", this);
		object c = Observer.I.Add("CookieChange", this);
		if(m != null) ChangeMoney((int)m);
		if(c != null) ChangeCookies((int)c);
	}

	void OnDestroy(){
		Observer.I.Remove("TowerBuildSelected", this);
		Observer.I.Remove("MoneyChange", this);
		Observer.I.Remove("CookieChange", this);
	}
	
	
	
	private void EnableDesc(bool b){
		//Ein bzw. ausblenden
		desc.enabled = b;
		money.enabled = b;
		moneyImg.enabled = b;
		cookies.enabled = b;
		cookiesImg.enabled = b;
		//Farben zurücksetzen
		i1.color = cNormal;
		i2.color = cNormal;
		i3.color = cNormal;
		i4.color = cNormal;
		i5.color = cNormal;
	}



	private void SetDesc(Image i, string name, int m, int c){
		desc.text = name;
		money.text = m.ToString();
		cookies.text = c.ToString();
		EnableDesc(true);
		i.color = cHighlight;
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
		Check(b1, m, c, MGTower.money_table[0], MGTower.cookie_table[0]);
		Check(b2, m, c, LaserTower.money_table[0], LaserTower.cookie_table[0]);
		Check(b3, m, c, LightningTower.money_table[0], LightningTower.cookie_table[0]);
		Check(b4, m, c, RocketTower.money_table[0], RocketTower.cookie_table[0]);
		Check(b5, m, c, SupportTower.money_table[0], SupportTower.cookie_table[0]);
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
		case "TowerBuildSelected":
			Tower t = msg.extraInfo as Tower;
			if(t == null){
				//Kein Turm ausgewählt
				EnableDesc(false);
			} else {
				//Turm ändern
				int m = t.MoneyBuildCost;
				int c = t.CookieBuildCost;
				switch(t.GetType().ToString()){
				case "MGTower": SetDesc(i1, "MG-Turm", m, c ); break;
				case "LaserTower": SetDesc(i2, "Laserturm", m, c ); break;
				case "LightningTower": SetDesc(i3, "Blitzturm", m, c ); break;
				case "RocketTower": SetDesc(i4, "Raketenturm", m, c ); break;
				case "SupportTower": SetDesc(i5, "Unterstützungsturm", m, c ); break;
				default: break;
				}
			}
			return true;
		default: return false;
		}
	}


	public void Select(int index){
		TowerBuilding.I.Select(index);
	}


}
