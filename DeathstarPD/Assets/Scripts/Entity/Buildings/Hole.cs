using UnityEngine;

public class Hole : Building {

	protected override void Start() {
		MaxHealth = 7500;
		
		//Start in Superklasse
		base.Start();
	}

	public override void DeathEffect(){
		//Spiel verloren
		Game.GameOver();
		
		base.DeathEffect();
	}

}
