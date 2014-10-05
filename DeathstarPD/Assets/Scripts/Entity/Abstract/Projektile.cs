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
public abstract class Projektile : MovableEntity {
	
	
	
	/// <summary>
	/// Schaden den das Projektil beim Spieler verursacht
	/// </summary>
	/// <value>
	/// in ganzen Trefferpunkten
	/// </value>
	public abstract int Damage { get; }
	
	
	
	/// <summary>
	/// GameObject, dass dieses Projektil abgeschossen hat.
	/// </summary>
	public GameObject owner;
	
	
	
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
		MaxHealth = 1;

		base.Start();
		
		//Projektile hinter die Gegner Positionieren
		transform.position = transform.position + new Vector3(0.0f, 0.0f, 0.01f);
		
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
		
		//Rotiere das Projektil in Richtung Ziel
		Rotate();
		
		base.FixedUpdate();
	}
	
	
	
	/// <summary>
	/// Rotiere das Projektil in Bewegungsrichtung
	/// </summary>
	protected void Rotate(){
		Vector3 rotate = rigidbody.velocity.Equals(Vector3.zero) ? TargetPos - Pos : rigidbody.velocity ;
		
		//Rotiere zum Ziel entlang der Z-Achse
		transform.rotation = Quaternion.LookRotation(rotate, zvector);
		
		//Drehe Sprite um 90°
		transform.Rotate(-90.0f, 0.0f, -90.0f);
	}
	
	/// <summary>
	/// Rotation um die Z-Achse
	/// </summary>
	private static readonly Vector3 zvector = new Vector3(0.0f, 0.0f, 1.0f);
	
	
	
	/// <summary>
	/// Kollisionsbehandlungsroutine:
	/// Kollision mit Spieler -> Schaden verursachen
	/// </summary>
	/// <param name='other'>
	/// Objekt mit dem die Kollision stattfindet
	/// </param>
	protected virtual void OnTriggerEnter(Collider other) {
		//nicht null, wenn Projektil von selben Typ
		Projektile p = other.gameObject.GetComponent<Projektile>();
		
		//Nicht mit dem Besitzer dieses Projektiles oder eines seiner anderen Projektile kollidieren
		if(other.gameObject != owner && (p==null || p.owner != owner ) ){
		
			//Kollision mit Spieler?
			if(other.gameObject.tag == "Player")
				//Schaden verursachen
				DoDamage(other, Damage);
			//auch bei Kollisionen die nicht mit dem Spieler sind sterben
			Death();
		}
	}
	
	
	
}
