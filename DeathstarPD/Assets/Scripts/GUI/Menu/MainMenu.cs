using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Hauptmenü, bei dem sich die Kamera dreht. Script auf Main Camera legen.
/// </summary>
public class MainMenu: MonoBehaviour {



	private GameObject main;
	private GameObject settings;



	public void Start(){
		main = GameObject.Find("MainMenuCanvas");
		settings = GameObject.Find("SettingsMenuCanvas");
	}



	public void StartGame(){ Game.LoadLevel(1); }

	public void OpenSettings(){
		if(settings != null){
			if(main != null){
				main.GetComponent<Canvas>().enabled = false;
				main.GetComponent<GraphicRaycaster>().enabled = false;
			}
			settings.GetComponent<Canvas>().enabled = true;
			settings.GetComponent<GraphicRaycaster>().enabled = true;
		}
	}

	public void OpenHighscore(){ /* TODO: Highscore anzeigen */ }

	public void QuitGame(){ Game.Quit(); }



	void FixedUpdate(){
		//Rotiere die Kamera
		float s = Time.fixedDeltaTime * 0.5f; //skalierung mit der tatsächlich vergangenen  Zeit
		transform.Rotate(s, 2*s, 0f);
	}

}
