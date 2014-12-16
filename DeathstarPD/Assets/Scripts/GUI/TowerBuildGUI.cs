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

	private Text name;
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

		name = transform.FindChild("TowerDescriptionNameText").GetComponent<Text>();
		desc = transform.FindChild("TowerDescriptionText").GetComponent<Text>();

		money = transform.FindChild("moneyText").GetComponent<Text>();
		moneyImg = transform.FindChild("moneyImage").GetComponent<Image>();

		cookies = transform.FindChild("cookiesText").GetComponent<Text>();
		cookiesImg = transform.FindChild("cookiesImage").GetComponent<Image>();

		EnableDesc(false);

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
		name.enabled = b;
		desc.enabled = b;
		money.enabled = b;
		moneyImg.enabled = b;
		cookies.enabled = b;
		cookiesImg.enabled = b;
	}


	private string getName(int index){
		switch(index){
		case 0: return "MG-Turm";
		case 1: return "Laserturm";
		case 2: return "Blitzturm";
		case 3: return "Raketenturm";
		case 4: return "Kraftwerk";
		default: return "";
		}
	}
	
	private string getDescription(int index){
		switch(index){
		case 0: return "Hohe Feuerrate, aber geringer Schaden. MG-Türme können genutzt werden um ein möglichst großes Gebiet abzudecken.";
		case 1: return "Sehr hohe Feuerrate, wenig Schaden. Lasertürme feuern einen konstanten Strahl auf ein Ziel um es schnell zu vernichten.";
		case 2: return "Moderater Schaden, hohe Feuerrate. 			Der Blitzturm schiesst Blitze auf mehrere Ziele und ist somit ideal zur Bereichsverteidigung.";
		case 3: return "Hoher Schaden, geringe Feuerrate. 						Der Raketenturm richtet hohen Bereichsschaden an, feuert aber sehr langsam.";
		case 4: return "Kein Schaden. 			Das Kraftwerk kann selber nicht angreifen, dafür verstärkt es Türme in Reichweite und produziert Energie.";
		default: return "";
		}
	}

	private int getMoney(int index){
		switch(index){
		case 0: return MGTower.money_table[0];
		case 1: return LaserTower.money_table[0];
		case 2: return LightningTower.money_table[0];
		case 3: return RocketTower.money_table[0];
		case 4: return SupportTower.money_table[0];
		default: return 0;
		}
	}

	private int getCookies(int index){
		switch(index){
		case 0: return MGTower.cookie_table[0];
		case 1: return LaserTower.cookie_table[0];
		case 2: return LightningTower.cookie_table[0];
		case 3: return RocketTower.cookie_table[0];
		case 4: return SupportTower.cookie_table[0];
		default: return 0;
		}
	}

	private Image getImage(int index){
		switch(index){
		case 0: return i1;
		case 1: return i2;
		case 2: return i3;
		case 3: return i4;
		case 4: return i5;
		default: return null;
		}
	}



	private void SetDesc(int index){
		name.text = getName(index);
		desc.text = getDescription(index);
		money.text = getMoney(index).ToString();
		cookies.text = getCookies(index).ToString();
		EnableDesc(true);
	}

	private void unhighlight(){
		//Farben zurücksetzen
		i1.color = cNormal;
		i2.color = cNormal;
		i3.color = cNormal;
		i4.color = cNormal;
		i5.color = cNormal;
	}

	private void highlight(int index){
		unhighlight();
		//Farbe setzen
		Image i = getImage(index);
		if(i != null) i.color = cHighlight;
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
		Check(b1, m, c, 0);
		Check(b2, m, c, 1);
		Check(b3, m, c, 2);
		Check(b4, m, c, 3);
		Check(b5, m, c, 4);
	}

	private void Check(Button b, int m, int c, int index){
		b.interactable = (getMoney(index) <=m) && (getCookies(index) <= c);
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
				unhighlight();
			} else {
				//Turm ändern
				SetDesc(TowerBuilding.I.SelectedIndex);
				highlight(TowerBuilding.I.SelectedIndex);
			}
			return true;
		default: return false;
		}
	}


	public void Select(int index){
		TowerBuilding.I.Select(index);
	}

	public void mouseOver(int index){
		SetDesc(index);
	}

	public void mouseOut(int index){
		if(TowerBuilding.I.SelectedIndex == -1){
			EnableDesc(false);
		} else {
			SetDesc(TowerBuilding.I.SelectedIndex);
		}
	}


}
