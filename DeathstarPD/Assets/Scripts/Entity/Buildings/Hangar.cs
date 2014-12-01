using UnityEngine;

public class Hangar : Building {

	protected override void Start() {
		MaxHealth = 3000;
		
		//Start in Superklasse
		base.Start();
	}

	public override void DeathEffect(){
		//TODO: Zerstöre alle Verteidiger-Schiffe
		base.DeathEffect();
	}

}
