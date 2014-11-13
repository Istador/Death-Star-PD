using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Das Pausen-Menü wird aktiviert wenn man im Spiel Escape drückt (oder 
/// vergleichbare Buttons auf anderen Plattformen).
/// 
/// Das Spiel wird angehalten, und der Spieler kann entweder das Spiel wieder
/// fortsetzen, oder das laufende Spiel beenden und zum Hauptmenü zurückkehren
/// </summary>
public class Pause : MonoBehaviour {


	//bool prellschutz = false;
	
	//Zum Hauptmenü Aktion
	void toMainMenu() {
		//Nachrichtenwarteschlange leeren
		MessageDispatcher.I.EmptyQueue();
		//Hauptmenü laden
		Application.LoadLevel(0);
		//Spiel fortsetzen
		ResumeGame();
	}
	
	void Start(){
		
		//Spiel fortsetzen falls noch pausiert
		Time.timeScale = 1.0f;
		paused = false;
		
		Inputs.I.Register("Pause", ()=>{
			Debug.Log("pause");
			//prüfen ob das Spiel pausiert
		    if(!paused) PauseGame();
		    //oder fortgesetzt werden soll
		    else ResumeGame();
		});
	}
	
	
	
	/// <summary>
	/// Ist das Spiel jetzt gerade pausiert?
	/// </summary>
	private bool paused = false;
	public bool Paused{get{return paused;}}
	
	
	
	/// <summary>
	/// Pausiere das Spiel
	/// </summary>
	private void PauseGame(){
		paused = true;

		//Zeit anhalten
		Time.timeScale = 0.0f;
	}

	
	/// <summary>
	/// Setze das pausierte Spiel fort
	/// </summary>
	private void ResumeGame(){
		paused = false;
				
		//Zeit weiterlaufen lassen
		Time.timeScale = 1.0f;
	}
	
	
}
