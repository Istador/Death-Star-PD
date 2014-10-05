using UnityEngine;
using System.Collections;

/// 
/// Bullets sind Geschosse die von Soldaten verschossen werden
/// 
/// Sie bewegen sich stur in eine Richtung und haben eine begrenzte Lebensdauer
/// 
public class PBullet : Projektile {
	
	
	/// <summary>
	/// Richtung in die der Gegner guckt.
	/// Lokales Koordinatensystem.
	/// </summary>
	public override Vector3 Heading { get{ return heading; } }
	public Vector3 heading = Vector3.zero; //Instanzvariable die von der Property verwendet wird
	
	
	/// <summary>
	/// Schaden den das Projektil beim Spieler verursacht
	/// </summary>
	/// <value>
	/// in ganzen Trefferpunkten
	/// </value>
	public override int Damage { get{return 10;} }
	
	
	
	/// <summary>
	/// Zeitpunkt an dem die Kugel erstellt wurde, um die Lebenszeit berechnen zu 
	/// können und wenn es so weit ist zu sterben.
	/// </summary>
	private double d_startTime;
	
	
	
	/// <summary>
	/// Zeit in Sekunden nach der die Kugel automatisch stirbt
	/// </summary>
	public static readonly double d_timeToLife = 5.0;
	
	
	
	protected override void Start() {
		//Zielposition setzen
		TargetPos = Pos + Heading.normalized * MaxSpeed;
		
		//u.a. Rotieren zum Ziel
		base.Start();
		
		//Sprite-Eigenschaften
		/*
		txtCols = 8;
		txtRows = 1;
		txtFPS = 8;

		//SpriteController einschalten
		Animated = true;
		*/
		
		//Geschwindigkeit setzen
		MaxSpeed = 8.4f;
		MaxForce = 8.4f;
		
		//Startzeitpunkt merken
		d_startTime = Time.time;
	}
	
	
	
	protected override void FixedUpdate() {
		//Zielposition aktualisieren
		TargetPos = Pos + Heading.normalized * MaxSpeed;
		
		//Rotieren zum Ziel, Bewegung umsetzen
		base.FixedUpdate();
		
		//Sterben nach einer bestimmten Zeit
		if(d_startTime + d_timeToLife <= Time.time)
			Death();
	}
	
	
	
}
