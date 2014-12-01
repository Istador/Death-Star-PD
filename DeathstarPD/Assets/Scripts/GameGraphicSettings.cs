using UnityEngine;

public class GameGraphicSettings {

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
            // nur gültige Werte erlauben
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



	private GameGraphicSettings(){
        //Default Werte aus den Unity Settings laden
        _vsync = QualitySettings.vSyncCount > 0;
        _simpMods = false;
        _antiAliasing = QualitySettings.antiAliasing;
        _aniso = QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable ? false : true;

        //Werte aus Datei lesen falls vorhanden
        Load();
    }



    private static void SetBool(string key, bool variable){
        PlayerPrefs.SetInt(key, variable ? 1 : 0);
    }



    public void Load(){
        if(PlayerPrefs.HasKey("Graphics.VSync"))
            VSync = PlayerPrefs.GetInt("Graphics.VSync", 0) != 0;

        if(PlayerPrefs.HasKey("Graphics.SimpleModells"))
            SimpleModells = PlayerPrefs.GetInt("Graphics.SimpleModells", 0) != 0;

        if(PlayerPrefs.HasKey("Graphics.AntiAliasing"))
            AntiAliasing = PlayerPrefs.GetInt("Graphics.AntiAliasing", 0);

        if(PlayerPrefs.HasKey("Graphics.AnisotropicFiltering"))
            AnisotrFiltering = PlayerPrefs.GetInt("Graphics.AnisotropicFiltering", 0) != 0;
    }



    public void Save(){
        SetBool("Graphics.VSync", VSync);
        SetBool("Graphics.SimpleModells", SimpleModells);
        PlayerPrefs.SetInt("Graphics.AntiAliasing", AntiAliasing);
        SetBool("Graphics.AnisotropicFiltering", AnisotrFiltering);
        PlayerPrefs.Save();
    }



    /**
     * Singleton
    */
	private static GameGraphicSettings instance;
	public static GameGraphicSettings Instance{get{
			if(instance==null) instance = new GameGraphicSettings();
            return instance;
        }}
	public static GameGraphicSettings I{get{return Instance;}}

}
