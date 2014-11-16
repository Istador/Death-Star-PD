using UnityEngine;

/// <summary>
/// Hauptmenü, bei dem sich die Kamera dreht. Script auf Main Camera legen.
/// </summary>
public class MainMenu: MonoBehaviour {

	public void StartGame(){ Game.LoadLevel(1); }

	public void OpenHighscore(){ /* TODO: Highscore anzeigen */ }

	public void QuitGame(){ Game.Quit(); }
	
	void FixedUpdate(){
		//Rotiere die Kamera
		float s = Time.deltaTime * 0.5f; //skalierung mit der tatsächlich vergangenen  Zeit
		transform.Rotate(s, 2*s, 0f);
	}

}
