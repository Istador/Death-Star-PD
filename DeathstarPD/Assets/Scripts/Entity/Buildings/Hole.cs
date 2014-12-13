using UnityEngine;

public class Hole : Building {

	protected override void Start() {
		MaxHealth = 2000;
		
		//Start in Superklasse
		base.Start();
	}

	public override void DeathEffect(){
		//Spiel verloren
		Game.GameOver();
		
		base.DeathEffect();
	}

}
