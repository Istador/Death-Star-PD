using UnityEngine;
using System.Collections;
using System;

public class GameResources {

	//Geld
	public int Money {
		get {return _money;}
		set {_money = Math.Max(0, value); Observer.I.Update(this, "MoneyChange", _money); }
	}
	private int _money = 7331;

	// Kekse (Energie)
	public int Cookies {
		get {return _cookies;}
		set {_cookies = Math.Max(0, value); Observer.I.Update(this, "CookieChange", _cookies);}
	}
	private int _cookies = 42;


	public void Reset(){
		Money = 1337;
		Cookies = 42;
	}

	public bool EnoughMoney(int amount){ return Money >= amount; }
	public bool EnoughCookies(int amount){ return Cookies >= amount; }
	public void ChangeMoney(int diff){ Money += diff; }
	public void ChangeCookies(int diff){ Cookies += diff; }

	
	/**
	 * Singleton
	*/
	private static GameResources instance;
	private GameResources(){}
	public static GameResources Instance{get{
			if(instance==null) instance = new GameResources();
			return instance;
		}}
	public static GameResources I{get{return Instance;}}
}
