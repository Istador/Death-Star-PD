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

	double sHeight;
	double sWidth;
	double s;
	
	Glyph g_title;
	Vector2 pos_title;
	double size_title = 0.65;

	/*
	double size_img = 0.18;
	Glyph g_left;
	Vector2 pos_left;
	Glyph g_right;
	Vector2 pos_right;
	*/
	
	double size_button = 1.5;
	GButton g_resume;
	Vector2 pos_resume;
	GButton g_menu = null;
	Vector2 pos_menu;

	//bool prellschutz = false;

	void Start(){
		
		//Spiel fortsetzen falls noch pausiert
		Time.timeScale = 1.0f;
		paused = false;

		//Spiel fortsetzen Aktion
		Action<GButton> a_resume = (b) => {
			ResumeGame();
		};

		//Zum Hauptmenü Aktion
		Action<GButton> a_menu = (b) => {
			//Nachrichtenwarteschlange leeren
			MessageDispatcher.I.EmptyQueue();
			//Hauptmenü laden
			Application.LoadLevel(0);
			//Spiel fortsetzen
			ResumeGame();
		};

		//g_title = GString.GetString("Pause");
		g_title = GEmpty.I;

		/*
		g_left = new GImage(Resource.Texture["love_left"]);
		g_right = new GImage(Resource.Texture["love_right"]);
		*/
		
		//g_resume = new GButton(250, 40, GString.GetString("Spiel fortsetzen"), a_resume);
		g_resume = new GButton(250, 40, new GImage(Resource.Texture["DeathStar"]), a_resume);
		g_resume.Padding.all = 10.0;
		g_resume.Border.all = 4.0;

		//g_menu = new GButton(250, 40, GString.GetString("Zum Hauptmenü"), a_menu);
		g_menu = new GButton(250, 40, GEmpty.I, a_menu);
		g_menu.Padding.all = 10.0;
		g_menu.Border.all = 4.0;

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



	void Resize(){
		if(sWidth != Screen.width || sHeight != Screen.height){
			sWidth = Screen.width;
			sHeight = Screen.height;
			
			double aspect = (sWidth / sHeight) / (1680/1050);
			s = (sHeight / 1050) * aspect;
			
			pos_title = new Vector2(
				(float)((sWidth - g_title.Width(size_title * s))/2.0),
				(float)( 100.0 * s )
				);

			/*
			pos_left = new Vector2(
				(float)( 20.0 * s ),
				(float)(sHeight - 20.0*s - g_left.Height(size_img * s))
				);
			
			pos_right = new Vector2(
				(float)(sWidth - 20.0*s - g_left.Width(size_img * s)),
				(float)(sHeight - 20.0*s - g_left.Height(size_img * s))
				);
			*/

			double height = g_resume.Height(size_button * s);
			pos_resume = new Vector2(
				(float)((sWidth - g_resume.Width(size_button * s))/2.0),
				(float)((sHeight - height)/2.0)
				);

			pos_menu = new Vector2(
				(float)((sWidth - g_menu.Width(size_button * s))/2.0),
				(float)((sHeight + height)/2.0 + 20.0*s)
			);
		}
		
	}


	
	
	//GUI zeichnen
	void OnGUI(){
		//nur wenn das Spiel pausiert wurde den Pausen-Bildschirm zeichnen
		if(paused){ //draw
			Utility.DrawRectangle(new Rect( 0, 0, Screen.width, Screen.height), Color.white);

			Resize();

			//Pause
			g_title.Draw(size_title * s, pos_title);
			
			//Bilder
			/*
			g_left.Draw(size_img * s, pos_left);
			g_right.Draw(size_img * s, pos_right);
			*/
			
			//Spiel fortsetzen
			g_resume.Draw(size_button * s, pos_resume);
			
			//Hauptmenü
			g_menu.Draw(size_button * s, pos_menu);
		}
	}
	
	
	
}
