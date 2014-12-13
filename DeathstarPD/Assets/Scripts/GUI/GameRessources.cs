using UnityEngine;
using System.Collections;

public class GameRessources : MonoBehaviour {

	//Geld
	public static int Money { get; set; }

	// Kekse (Energie)
	public static int Cookies { get; set; }

	public static void Reset(){
		Money = 1000;
		Cookies = 25;
	}

	void Start(){
		Reset();
	}

	public bool EnoughMoney(int amount){ return Money >= amount; }
	public bool EnoughCookies(int amount){ return Cookies >= amount; }
	public void ChangeMoney(int diff){ Money += diff; }
	public void ChangeCookies(int diff){ Cookies += diff; }

}
