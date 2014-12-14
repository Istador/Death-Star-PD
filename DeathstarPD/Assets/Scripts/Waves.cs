using UnityEngine;
using System.Collections.Generic;

public class Waves : GeneralObject {

	private struct Wave {
		public readonly float strength;
		public readonly string[] ships;
		public readonly int[] amounts;
		public Wave(float strength, string[] ships, int[] amounts){
			this.strength = strength;
			this.ships = ships;
			this.amounts = amounts;
		}
	}


	/// <summary>
	/// Container in den alle Gegner Schiffe kommen
	/// </summary>
	public static Transform Container {
		get{
			if(_container == null)
				_container = GameObject.Find("Entities").transform.FindChild("Aggressors");
			return _container;
		}
	}
	private static Transform _container = null;



	private static List<Wave> waves = null;



	public Waves(){
		instance = this;

		if(waves == null){
			waves = new List<Wave>();

			//Mini Fighter Vorstellung
			waves.Add( new Wave(0.50f, new string[]{"MyLittleDestroyer"}, new int[]{5}) );
			waves.Add( new Wave(0.75f, new string[]{"MyLittleDestroyer", "MyLittleDestroyer"}, new int[]{5, 5}) );

			//Fighter Vorstellung
			waves.Add( new Wave(0.50f, new string[]{"Fighter"}, new int[]{3}) );
			waves.Add( new Wave(0.75f, new string[]{"Fighter", "Fighter"}, new int[]{3, 3}) );

			//Bomber Vorstellung
			waves.Add( new Wave(0.50f, new string[]{"Bomber"}, new int[]{2}) );
			waves.Add( new Wave(0.75f, new string[]{"Bomber", "Bomber"}, new int[]{2, 2}) );

			// Normal und einzelnd 
			waves.Add( new Wave(1.00f, new string[]{"MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer"}, new int[]{5, 5, 5, 5}) );
			waves.Add( new Wave(1.00f, new string[]{"Fighter", "Fighter", "Fighter", "Fighter"}, new int[]{3, 3, 3, 3}) );
			waves.Add( new Wave(1.00f, new string[]{"Bomber", "Bomber", "Bomber", "Bomber"}, new int[]{2, 2, 2, 2}) );
			//Normal Kombinationen
			waves.Add( new Wave(1.00f, new string[]{"Fighter", "Fighter", "Fighter", "Fighter", "Bomber", "Bomber"}, new int[]{3, 3, 3, 3, 2, 2}) );
			waves.Add( new Wave(1.00f, new string[]{"MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer", "Fighter", "Fighter"}, new int[]{5, 5, 5, 5, 3}) );

			//Schwer einzelnd
			waves.Add( new Wave(1.50f, new string[]{"MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer"}, new int[]{5, 5, 5, 5}) );
			waves.Add( new Wave(1.50f, new string[]{"Fighter", "Fighter", "Fighter", "Fighter"}, new int[]{3, 3, 3, 3}) );
			waves.Add( new Wave(1.50f, new string[]{"Bomber", "Bomber", "Bomber", "Bomber"}, new int[]{2, 2, 2, 2}) );
			//Schwer Kombinationen
			waves.Add( new Wave(1.50f, new string[]{"Fighter", "Fighter", "Fighter", "Fighter", "Bomber", "Bomber"}, new int[]{3, 3, 3, 3, 2, 2}) );
			waves.Add( new Wave(1.50f, new string[]{"MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer", "Fighter", "Fighter"}, new int[]{5, 5, 5, 5, 3}) );

			//Unmöglich
			waves.Add( new Wave(1.50f, new string[]{"MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer", "MyLittleDestroyer", "Fighter", "Fighter", "Fighter", "Bomber", "Bomber"}, new int[]{7, 7, 7, 7, 5, 5, 5, 3, 3}) );
		}
	}



	public int WaveNumber { get; private set; }
	public int LastWaveNumber { get { return waves.Count; }}
	public bool WaveRunning { get { return AliveEnemies != 0; }}
	public int AliveEnemies { get; private set; }



	protected override void Start(){
		WaveNumber = 0;
		AliveEnemies = 0;

		//Spawnen über die Tastatur
		if(Debug.isDebugBuild)
			Inputs.I.Register(KeyCode.W, NextWave);

		//Bekomme Todesnachrichten
		Observer.I.Add("Death", this);

		base.Start();
	}



	public void NextWave(){
		if(WaveRunning){
			Debug.LogError("Versuch eine Welle zu starten, während noch eine läuft");
			return;
		}
		if(WaveNumber == LastWaveNumber){
			Debug.LogError("Versuch eine Welle zu starten, obwohl die letzte bereits gestartet wurde");
			return;
		}

		Spawn(waves[WaveNumber++]);
	}


	private void Spawn(Wave w){
		int n = System.Math.Min(w.ships.Length, w.amounts.Length);
		for(int i = 0; i < n; i++){
			//TODO Robin: Gegner Spawn Point zufällig um die Kugel verteilen 
			Vector3 pos = new Vector3(200f, 0f, 0f);
			Spawn(w.strength, w.ships[i], w.amounts[i], pos);
		}
	}

	// Erzeugt zufällig um den Punkt herum mehrere Fighter
	private void Spawn(float strength, string name, int amount, Vector3 pos){
		for(; amount > 0 ; amount--){
			float x = Utility.NextFloat(0f, 15f);
			float y = Utility.NextFloat(0f, 15f);
			float z = Utility.NextFloat(0f, 15f);
			Ship s = Instantiate(Resource.Prefab["Enemies/"+name], pos + new Vector3(x, y, z)).GetComponent<Ship>();
			s.Strength = strength; // Stärke setzen
			s.transform.parent = Container; // in den Container

			//erhöhe die Anzahl Schiffe
			AliveEnemies++;
		}
	}


	public override bool HandleMessage(Telegram msg){
		if(this == null || !enabled) return true;
		
		switch(msg.message){
		case "Death":
			if(msg.sender == null) return true;
			Entity e = (Entity)msg.sender;
			//wenn es ein Gegner ist
			if(e.gameObject.layer == 13){
				//Verringere Anzahl lebender Gegner
				AliveEnemies--;
			}
			return true;
		default:
			return base.HandleMessage(msg);
		}
	}



	
	/**
	 * Singleton Reference (not Object :( )
	*/
	private static Waves instance;
	public static Waves Instance{get{return instance;}}
	public static Waves I{get{return Instance;}}
}
