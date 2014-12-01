using UnityEngine;
using System.Collections;

public abstract class Building : ImmovableEntity {



	protected override void Start() {
		//Start in Superklasse
		base.Start();

		//Namen setzen
		name = GetType().ToString();

		//Füge zur Towers-Collection hinzu
		Buildings.I.Add(this);
	}



	public override void Death(){
		//Entferne das Gebäude von der Towers-Collection
		Buildings.I.Remove(this);
		
		//Death von Superklasse
		base.Death();
	}



}
