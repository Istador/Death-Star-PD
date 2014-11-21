using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {

	/// <summary>
	/// Das Canvas, das wieder aktiviert wird, sobald dieses Fenster geschlossen wird
	/// </summary>
	public GameObject returnCanvas;

	private Toggle vsync;
	private Toggle simpleModels;
	private Slider antialias;
	private Text antialiastext;
	private Toggle anisotr;


	void Start(){
		//Referenzen auf die Widgets laden
		Transform panel = transform.FindChild("SettingsMenuPanel");
		vsync = panel.FindChild("VSyncToggle").GetComponent<Toggle>();
		simpleModels = panel.FindChild("SimpleModelsToggle").GetComponent<Toggle>();
		antialias = panel.FindChild("AntiAliasingSlider").GetComponent<Slider>();
		antialiastext = antialias.transform.FindChild("Text").GetComponent<Text>();
		anisotr = panel.FindChild("AnisotropicFilteringToggle").GetComponent<Toggle>();

		Load();
	}

	private int ToAA(int val){
		switch(val){
		default: return 0;
		case 0:  return 0;
		case 1:  return 2;
		case 2:  return 4;
		case 3:  return 8;
		}
	}

	private int FromAA(int val){
		switch(val){
		default: return 0;
		case 0:  return 0;
		case 2:  return 1;
		case 4:  return 2;
		case 8:  return 3;
		}
	}

	public void ChangeAALabel(){
		antialiastext.text = ToAA((int)antialias.value).ToString();
	}

	private void Load(){
		//Setze Interface entsprechend der gespeicherten Einstellungen
		vsync.isOn = GraphicSettings.I.VSync;
		simpleModels.isOn = GraphicSettings.I.SimpleModells;
		antialias.value = FromAA(GraphicSettings.I.AntiAliasing);
		antialiastext.text = GraphicSettings.I.AntiAliasing.ToString();
		anisotr.isOn = GraphicSettings.I.AnisotrFiltering;
	}

	public void Save(){
		GraphicSettings.I.VSync = vsync.isOn;
		GraphicSettings.I.SimpleModells = simpleModels.isOn;
		GraphicSettings.I.AntiAliasing = ToAA((int)antialias.value);
		GraphicSettings.I.AnisotrFiltering = anisotr.isOn;
		GraphicSettings.I.Save();

		//Frame schließen
		Cancel();
	}

	public void Cancel(){
		//ausblenden
		GetComponent<Canvas>().enabled = false;
		GetComponent<GraphicRaycaster>().enabled = false;

		//einblenden vom vorigem Frame
		if(returnCanvas != null){
			returnCanvas.GetComponent<Canvas>().enabled = true;
			returnCanvas.GetComponent<GraphicRaycaster>().enabled = true;
		}

		Load();
	}

}
