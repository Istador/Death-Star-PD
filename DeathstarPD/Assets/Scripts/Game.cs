using UnityEngine;

public static class Game {

	public static void LoadLevel(int id){
		//Nachrichtenwarteschlange leeren
		MessageDispatcher.I.EmptyQueue();
		//Level laden
		Application.LoadLevel(id);
		//Spiel fortsetzen
		if(Pause.I != null)
			Pause.I.ResumeGame();
	}

	public static void ToMainMenu(){
		LoadLevel(0);
	}

	public static void Quit(){
		//Beendet das Spiel
		#if UNITY_EDITOR
		//Unity Editor: Spiel stoppen
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		//Webplayer: Seite neu laden
		Application.OpenURL("https://games.blackpinguin.de/DeathstarPD/play.html");
		#else
		//Standalone Build: Programm beenden
		Application.Quit();
		#endif
	}

	public static void GameOver(){
		//Spiel anhalten
		//Time.timeScale = 0.0f;

		//TODO: GameOver-Screen anzeigen

		ToMainMenu();
	}

}
