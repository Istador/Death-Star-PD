using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Das Pausen-Menü wird aktiviert wenn man im Spiel Escape drückt (oder 
/// vergleichbare Buttons auf anderen Plattformen).
/// 
/// Das Spiel wird angehalten, und der Spieler kann entweder das Spiel wieder
/// fortsetzen, oder das laufende Spiel beenden und zum Hauptmenü zurückkehren
/// </summary>
public class Pause : MonoBehaviour {

	//bool prellschutz = false;
	
	private Canvas canvas;
	private GraphicRaycaster raycaster;
	private GraphicRaycaster gameRaycaster;
	private SettingsMenu settings;

	void Start(){
		canvas = GetComponent<Canvas>();
		raycaster = GetComponent<GraphicRaycaster>();
		gameRaycaster = GameObject.Find("GameCanvas").GetComponent<GraphicRaycaster>();
		settings = GameObject.Find("SettingsMenuCanvas").GetComponent<SettingsMenu>();

		//Spiel fortsetzen falls noch pausiert
		Time.timeScale = 1.0f;
		Paused = false;

		Inputs.I.Register("Pause", ()=>{
			//prüfen ob das Spiel pausiert
			if(!Paused) PauseGame();
			//oder fortgesetzt werden soll
			else if(canvas.enabled) ResumeGame();
			//oder die Settings geschlossen werden soll
			else settings.Cancel();
		});
	}

	public void QuitGame(){ Game.Quit(); }
	
	public void ToMainMenu() { Game.ToMainMenu(); }

	public void OpenSettings(){
		if(settings != null){
			canvas.enabled = false;
			raycaster.enabled = false;

			settings.GetComponent<Canvas>().enabled = true;
			settings.GetComponent<GraphicRaycaster>().enabled = true;
		}
	}

	/// <summary>
	/// Ist das Spiel jetzt gerade pausiert?
	/// </summary>
	private bool _paused = false;
	public bool Paused { 
		get{return _paused;} 
		set{if(value) PauseGame(); else ResumeGame();}
	}

	/// <summary>
	/// Pausiere das Spiel
	/// </summary>
	public void PauseGame(){
		_paused = true;

		//Pausen Menü einblenden
		canvas.enabled = true;
		//Pausen Menü  anklickbar
		raycaster.enabled = true;
		//Game Interface nicht anklickbar
		gameRaycaster.enabled = false;
		
		//Zeit anhalten
		Time.timeScale = 0.0f;
	}

	
	/// <summary>
	/// Setze das pausierte Spiel fort
	/// </summary>
	public void ResumeGame(){
		_paused = false;

		//Pausen Menü ausblenden
		canvas.enabled = false;
		//Pausen Menü nicht anklickbar
		raycaster.enabled = false;
		//Game Interface wieder anklickbar
		gameRaycaster.enabled = true;
		
		//Zeit weiterlaufen lassen
		Time.timeScale = 1.0f;
	}
	
	
	/**
	 * Singleton
	*/
	private static Pause instance;
	public Pause(){ instance = this; }
	public static Pause Instance{get{return instance;}}
	public static Pause I{get{return Instance;}}
}

