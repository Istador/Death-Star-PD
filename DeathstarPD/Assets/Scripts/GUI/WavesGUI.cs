using UnityEngine;
using UnityEngine.UI;

public class WavesGUI : MonoBehaviour, MessageReceiver {

	private Text waveText;
	private Text aliveText;
	private Button nextWave;


	void Start () {
		waveText = transform.FindChild("Wave").GetComponent<Text>();
		aliveText = transform.FindChild("Alive").GetComponent<Text>();
		nextWave = transform.FindChild("NextWaveButton").GetComponent<Button>();

		object w = Observer.I.Add("Wave", this);
		object a = Observer.I.Add("AliveEnemies", this);
		if(w != null) ChangeWave((int)w);
		if(a != null) ChangeAlive((int)a);
	}


	private void ChangeWave(int x){
		waveText.text = "Welle "+x.ToString()+" von "+Waves.I.LastWaveNumber.ToString();
	}


	private void ChangeAlive(int x){
		if(x == 0){
			aliveText.text = "keine Gegner";
		} else {
			aliveText.text = "noch "+x.ToString()+" Gegner";
		}

		nextWave.interactable = !Waves.I.WaveRunning && Waves.I.WaveNumber != Waves.I.LastWaveNumber;
	}

	public void SpawnNextWave(){
		if(!Waves.I.WaveRunning && Waves.I.WaveNumber != Waves.I.LastWaveNumber)
			Waves.I.NextWave();
	}


	public bool HandleMessage(Telegram msg){
		if(this == null) return true;
		switch(msg.message){
		case "Wave":
			ChangeWave((int) msg.extraInfo);
			return true;
		case "AliveEnemies":
			ChangeAlive((int) msg.extraInfo);
			return true;
		default: return false;
		}
	}

}
