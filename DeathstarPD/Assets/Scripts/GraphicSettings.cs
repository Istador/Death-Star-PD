using UnityEngine;

public class GraphicSettings {

	public bool VSync {
		get{ return _vsync; }
		set{ _vsync = value; QualitySettings.vSyncCount = value ? 1 : 0 ; }
	}
	private bool _vsync;



	public bool SimpleModells {
		get{ return _simpMods; }
		set{ _simpMods = value; /* TODO Robin: Simple Modelle */ }
	}
	private bool _simpMods;
	
	
	
	public int AntiAliasing {
		get{ return _antiAliasing; }
		set{
			if(value == 0 || value == 2 || value == 4 || value == 8){
				_antiAliasing = value;
				QualitySettings.antiAliasing = value;
			}
		}
	}
	private int _antiAliasing;



	public bool AnisotrFiltering {
		get{ return _aniso; }
		set{
			_aniso = value;
			QualitySettings.anisotropicFiltering = 
				value ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable ;
		}
	}
	private bool _aniso;



	private GraphicSettings(){
		//Defaults laden
		_vsync = QualitySettings.vSyncCount > 0;
		_simpMods = false;
		_antiAliasing = QualitySettings.antiAliasing;
		_aniso = QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable ? false : true;

		//Überschreibe Werte von Datei, falls vorhanden
		Load();
	}



	private static void SetBool(string key, bool variable){
		PlayerPrefs.SetInt(key, variable ? 1 : 0);
	}



	public void Load(){
		if(PlayerPrefs.HasKey("vSync"))
			VSync = PlayerPrefs.GetInt("vSync", 0) != 0;

		if(PlayerPrefs.HasKey("simpleModells"))
			SimpleModells = PlayerPrefs.GetInt("simpleModells", 0) != 0;

		if(PlayerPrefs.HasKey("antiAliasing"))
			AntiAliasing = PlayerPrefs.GetInt("antiAliasing", 0);

		if(PlayerPrefs.HasKey("anisotropicFiltering"))
			AnisotrFiltering = PlayerPrefs.GetInt("anisotropicFiltering", 0) != 0;
	}



	public void Save(){
		SetBool("vSync", VSync);
		SetBool("simpleModells", SimpleModells);
		PlayerPrefs.SetInt("antiAliasing", AntiAliasing);
		SetBool("anisotropicFiltering", AnisotrFiltering);
		PlayerPrefs.Save();
	}


	
	/**
	 * Singleton
	*/
	private static GraphicSettings instance;
	public static GraphicSettings Instance{get{
			if(instance==null) instance = new GraphicSettings();
			return instance;
		}}
	public static GraphicSettings I{get{return Instance;}}

}
