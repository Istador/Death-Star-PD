using UnityEngine;

public class Hangar : Building {

	protected override void Start() {
		MaxHealth = 1500;
		
		//Start in Superklasse
		base.Start();
	}

	public override void DeathEffect(){
		//TODO Robin: Zerstöre alle Verteidiger-Schiffe
		base.DeathEffect();
	}

}
