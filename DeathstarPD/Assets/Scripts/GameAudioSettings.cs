using UnityEngine;

public class GameAudioSettings {

    public float BGMVolume {
        get{ return AudioTaggedSource.Volume(AudioTaggedSource.EAudioTag.BGM); }
		set{ AudioTaggedSource.SetVolume(AudioTaggedSource.EAudioTag.BGM, value); }
    }

	public float SFXVolume {
		get{ return AudioTaggedSource.Volume(AudioTaggedSource.EAudioTag.SFX); }
		set{ AudioTaggedSource.SetVolume(AudioTaggedSource.EAudioTag.SFX, value); }
	}

	public float DialogVolume {
		get{ return AudioTaggedSource.Volume(AudioTaggedSource.EAudioTag.Dialog); }
		set{ AudioTaggedSource.SetVolume(AudioTaggedSource.EAudioTag.Dialog, value); }
	}



	private GameAudioSettings(){
        //Werte aus Datei lesen falls vorhanden
        Load();
    }



    public void Load(){
        if(PlayerPrefs.HasKey("Audio.Volume.BGM"))
			BGMVolume = PlayerPrefs.GetFloat("Audio.Volume.BGM", 1f);

		if(PlayerPrefs.HasKey("Audio.Volume.SFX"))
			SFXVolume = PlayerPrefs.GetFloat("Audio.Volume.SFX", 1f);

		if(PlayerPrefs.HasKey("Audio.Volume.Dialog"))
			DialogVolume = PlayerPrefs.GetFloat("Audio.Volume.Dialog", 1f);
    }



    public void Save(){
		PlayerPrefs.SetFloat("Audio.Volume.BGM", BGMVolume);
		PlayerPrefs.SetFloat("Audio.Volume.SFX", SFXVolume);
		PlayerPrefs.SetFloat("Audio.Volume.Dialog", DialogVolume);
        PlayerPrefs.Save();
    }



    /**
     * Singleton
    */
	private static GameAudioSettings instance;
	public static GameAudioSettings Instance{get{
			if(instance==null) instance = new GameAudioSettings();
            return instance;
        }}
	public static GameAudioSettings I{get{return Instance;}}

}
