using UnityEngine;
using System.Collections;

/*
 * Abstrakte Oberklasse für Projektile von Gegnern
 * Gemeinsam für alle Projektile:
 * - Schaden Verursachen 
 * - Sterben wenn sie selbst Schaden erleiden (1 HP)
 * - Rotation zu Flugrichtung
 * 
*/
public abstract class Projektile<T> : MovableEntity {



	/// <summary>
	/// Schaden den das Projektil beim Spieler verursacht
	/// </summary>
	/// <value>
	/// in ganzen Trefferpunkten
	/// </value>
	protected abstract int Damage { get; }
	
	
	
	/// <summary>
	/// GameObject, dass dieses Projektil abgeschossen hat.
	/// </summary>
	public T Owner { get; set; }
	
	
	
	/// <summary>
	/// Position die das Projektil anstrebt
	/// </summary>
	/// <value>
	/// 3D-Koordinate der Zielposition
	/// </value>
	public Vector3 TargetPos { get; protected set; }
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Projektile`1"/> class.
	/// </summary>
	public Projektile() : base() {}
	
	
	
	protected override void Start() {
		//hier nicht base, weil evtl. erst Parameter übergeben werden müssen
	}


	public virtual void Init(){
		MaxHealth = 1;
		
		base.Start();
		
		//Zielposition anstreben
		Steering.DoSeek(TargetPos);
		
		//Rotiere das Projektil in Richtung Ziel
		Rotate();
	}
	
	
	
	/// <summary>
	/// Ziel für Steering Behaviors setzen, Rotiere zum Ziel
	/// </summary>
	protected override void FixedUpdate() {
		//Seek-Ziel auf Position setzen
		Steering.TargetPos = TargetPos;

		//Bewegen
		base.FixedUpdate();

		//Rotiere das Projektil in Richtung Ziel
		Rotate();
	}
	
	
	
	/// <summary>
	/// Rotiere das Projektil in Bewegungsrichtung
	/// </summary>

	protected void Rotate(){

		Vector3 rotate = rigidbody.velocity.Equals(Vector3.zero) ? TargetPos - Pos : rigidbody.velocity ;
		
		//Rotiere zum Ziel entlang der Z-Achse
		transform.rotation = Quaternion.LookRotation(rotate);
		
		//Drehe, so dass der Z-Vektor auf das Ziel zeigt
		transform.Rotate(0f, 90f, 90f);

	}


	
	/// <summary>
	/// Kollisionsbehandlungsroutine:
	/// Kollision mit Gegnern/Spieler -> Schaden verursachen
	/// </summary>
	/// <param name='other'>
	/// Objekt mit dem die Kollision stattfindet
	/// </param>
	protected virtual void OnTriggerEnter(Collider other) {
		//Kollision mit Gegnern?

		//Kein Schaden bei Levelobjekten verursachen
		if( ((int)Layer.Level & other.gameObject.layer) != 0 ){
			//Schaden verursachen
			DoDamage(other, Damage);
		}

		//auch bei Kollisionen, die mit dem Level sind, sterben.
		Death();
	}


	
	
	
}
