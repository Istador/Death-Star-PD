using UnityEngine;
using UnityEngine.UI;

public class GameResourcesGUI : MonoBehaviour, MessageReceiver {

	private Text moneyText;
	private Text cookieText;

	void Start() {
		moneyText = transform.FindChild("Money").GetComponent<Text>();
		cookieText = transform.FindChild("Cookies").GetComponent<Text>();

		object m = Observer.I.Add("MoneyChange", this);
		object c = Observer.I.Add("CookieChange", this);
		if(m != null) ChangeMoney((int)m);
		if(c != null) ChangeCookies((int)c);

		GameResources.I.Reset();
	}

	private void ChangeMoney(int x){ moneyText.text = x.ToString(); }
	private void ChangeCookies(int x){ cookieText.text = x.ToString(); }

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
