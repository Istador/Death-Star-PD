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
	private Slider bgm;
	private Slider sfx;

	void Start(){
		//Referenzen auf die Widgets laden
		Transform panel = transform.FindChild("SettingsMenuPanel");
		vsync = panel.FindChild("VSyncToggle").GetComponent<Toggle>();
		simpleModels = panel.FindChild("SimpleModelsToggle").GetComponent<Toggle>();
		antialias = panel.FindChild("AntiAliasingSlider").GetComponent<Slider>();
		antialiastext = antialias.transform.FindChild("Text").GetComponent<Text>();
		anisotr = panel.FindChild("AnisotropicFilteringToggle").GetComponent<Toggle>();
		bgm = panel.FindChild("BGMVolumeSlider").GetComponent<Slider>();
		sfx = panel.FindChild("SFXVolumeSlider").GetComponent<Slider>();

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
		if(antialiastext != null && antialiastext.text != null && antialias != null)
			antialiastext.text = ToAA((int)antialias.value).ToString();
	}

	private void Load(){
		//Setze Interface entsprechend der gespeicherten Einstellungen
		vsync.isOn = GameGraphicSettings.I.VSync;
		simpleModels.isOn = GameGraphicSettings.I.SimpleModells;
		antialias.value = FromAA(GameGraphicSettings.I.AntiAliasing);
		antialiastext.text = GameGraphicSettings.I.AntiAliasing.ToString();
		anisotr.isOn = GameGraphicSettings.I.AnisotrFiltering;
		bgm.value = GameAudioSettings.I.BGMVolume;
		sfx.value = GameAudioSettings.I.SFXVolume;
	}

	public void Save(){
		GameGraphicSettings.I.VSync = vsync.isOn;
		GameGraphicSettings.I.SimpleModells = simpleModels.isOn;
		GameGraphicSettings.I.AntiAliasing = ToAA((int)antialias.value);
		GameGraphicSettings.I.AnisotrFiltering = anisotr.isOn;
		GameGraphicSettings.I.Save();

		GameAudioSettings.I.BGMVolume = bgm.value;
		GameAudioSettings.I.SFXVolume = sfx.value;
		GameAudioSettings.I.Save();

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
