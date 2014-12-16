using UnityEngine;
using System.Collections.Generic;

public class AudioTaggedSource : MonoBehaviour {

	public enum EAudioTag {
		/// <summary>
		/// Musik
		/// </summary>
		  BGM
		/// <summary>
		/// Sounds
		/// </summary>
		, SFX
		/// <summary>
		/// NPC Dialog
		/// </summary>
		, Dialog
	}

	public EAudioTag AudioTag = EAudioTag.SFX;
	public AudioClip clip;
	public bool mute;
	public bool playOnAwake;
	public bool loop;
	public float volume = 0.5f;
	public AudioRolloffMode rolloffMode = AudioRolloffMode.Linear;
	public float panLevel = 1f;
	public float spread = 0f;
	public float maxDistance = 200f;

	public void Start(){
		Add(this);
		gameObject.AddComponent<AudioSource>();
		audio.clip = clip;
		audio.mute = mute;
		audio.playOnAwake = playOnAwake;
		audio.loop = loop;
		audio.volume = volume * Volume(AudioTag);
		audio.rolloffMode = rolloffMode;
		audio.panLevel = panLevel;
		audio.spread = spread;
		audio.maxDistance = maxDistance;
		audio.bypassEffects = true;
		audio.bypassListenerEffects = true;
		audio.bypassReverbZones = true;
		audio.dopplerLevel = 0;
		if(audio.playOnAwake)
			audio.Play();
	}

	public void OnDestroy(){
		Remove(this);
	}

	private void ChangeVolume(float volume){
		audio.volume = this.volume * volume;
	}


	//Alle Sounds, die einerm bestimmten Typ angehören
	private static Dictionary<EAudioTag, HashSet<AudioTaggedSource>> map = new Dictionary<EAudioTag, HashSet<AudioTaggedSource>>();

	private static void Add(AudioTaggedSource ats){
		EAudioTag tag = ats.AudioTag;
		if(!map.ContainsKey(tag)){
			map[tag] = new HashSet<AudioTaggedSource>();
			volumes[tag] = 1f;
		}
		if(!map[tag].Contains(ats))
			map[tag].Add(ats);
	}

	private static void Remove(AudioTaggedSource ats){
		EAudioTag tag = ats.AudioTag;
		if(!map.ContainsKey(tag))
			return;
		if(map[tag].Contains(ats))
			map[tag].Remove(ats);
	}

	private static Dictionary<EAudioTag, float> volumes = new Dictionary<EAudioTag, float>();

	public static void SetVolume(EAudioTag tag, float volume){
		volumes[tag] = Mathf.Clamp(volume, 0f, 1f);
		if(!map.ContainsKey(tag)){
			map[tag] = new HashSet<AudioTaggedSource>();
		} else {
			foreach(AudioTaggedSource ats in map[tag]){
				ats.ChangeVolume(volumes[tag]);
			}
		}
	}

	public static float Volume(EAudioTag tag) {
		//Tag ist vorhanden
		if(map.ContainsKey(tag)) return volumes[tag];
		//Tag ist nicht vorhanden
		else return 1f;
	}
}
