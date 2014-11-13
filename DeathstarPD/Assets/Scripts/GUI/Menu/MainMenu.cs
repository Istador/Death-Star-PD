using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class MainMenu: MonoBehaviour {

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
	GButton g_start;
	Vector2 pos_start;
	GButton g_quit = null;
	Vector2 pos_quit;

	public static void ToMainMenu(){
		//Nachrichtenwarteschlange leeren
		MessageDispatcher.I.EmptyQueue();
		//Hauptmenü laden
		Application.LoadLevel(0);
		//Spiel fortsetzen
		if(Pause.I != null)
			Pause.I.ResumeGame();
	}

	public static void QuitGame(){
		//Beendet das Spiel
		#if UNITY_EDITOR
		//Unity Editor
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		//Webplayer
		Application.OpenURL("https://games.blackpinguin.de/DeathstarPD/play.html");
		#else
		//Standalone Build
		Application.Quit();
		#endif
	}

	Action<GButton> a_start = (b) => {
		MessageDispatcher.I.EmptyQueue();
		Application.LoadLevel(1);
	};

	Action<GButton> a_quit = (b) => { QuitGame(); };



	void Start(){

		//g_title = GString.GetString("Brain of teh Zwarm");
		g_title = GEmpty.I;

		/*
		g_left = new GImage(Resource.Texture["love_left"]);
		g_right = new GImage(Resource.Texture["love_right"]);
		*/

		//g_start = new GButton(250, 40, GString.GetString("Spiel starten"), a_start);
		g_start = new GButton(250, 40, GEmpty.I, a_start);
		g_start.Padding.all = 10.0;
		g_start.Border.all = 4.0;

		//Quit nicht im WebPlayer zeigen
		if(!(Application.isWebPlayer)){
			//g_quit = new GButton(250, 40, GString.GetString("Spiel beenden"), a_quit);
			g_quit = new GButton(250, 40, GEmpty.I, a_quit);
			g_quit.Padding.all = 10.0;
			g_quit.Border.all = 4.0;
		}
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

			double height = g_start.Height(size_button * s);
			pos_start = new Vector2(
				(float)((sWidth - g_start.Width(size_button * s))/2.0),
				(float)((sHeight - height)/2.0)
			);

			if(g_quit != null){
				pos_quit = new Vector2(
					(float)((sWidth - g_quit.Width(size_button * s))/2.0),
					(float)((sHeight + height)/2 + 20.0*s)
				);
			}
		}
		
	}



	void OnGUI(){
		Resize();

		//Brain of teh Zwarm
		g_title.Draw(size_title * s, pos_title);

		//Bilder
		/*
		g_left.Draw(size_img * s, pos_left);
		g_right.Draw(size_img * s, pos_right);
		*/

		//Spiel starten
		g_start.Draw(size_button * s, pos_start);

		//Spiel beenden
		if(g_quit != null)
			g_quit.Draw(size_button * s, pos_quit);
	}



	void Update(){
		if(Input.GetKeyDown(KeyCode.F)){
			MessageDispatcher.I.EmptyQueue();
			Application.LoadLevel(3);
		}
	}

}
