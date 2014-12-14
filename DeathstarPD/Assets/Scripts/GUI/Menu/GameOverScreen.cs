using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

	private GameObject gameCanvas;
	private GameObject settings;
	private GameObject pause;

	// Use this for initialization
	private void Start() {
		gameCanvas = GameObject.Find("GameCanvas");
		settings = GameObject.Find("SettingsMenuCanvas");
		pause = GameObject.Find("PauseMenuCanvas");

		if(Debug.isDebugBuild) Inputs.I.Register(KeyCode.F4, Game.GameOver);
	}

	private void Show(GameObject obj, bool on){
		if(obj != null){
			Canvas c = obj.GetComponent<Canvas>();
			if(c != null) c.enabled = on;

			GraphicRaycaster rc = obj.GetComponent<GraphicRaycaster>();
			if(rc != null) rc.enabled = on;
		}
	}

	public void GameOver(){
		//Spiel anhalten
		Time.timeScale = 0f;

		//GUI ausblenden
		Show(gameCanvas, false);
		Show(settings, false);
		Show(pause, false);
		Pause.I.enabled = false;

		//selbst einblenden
		Show(gameObject, true);
	}

	public void QuitGame(){ Game.Quit(); }
	
	public void ToMainMenu() { Game.ToMainMenu(); }
}
