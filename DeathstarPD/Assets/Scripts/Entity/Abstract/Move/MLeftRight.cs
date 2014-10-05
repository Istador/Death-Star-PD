using UnityEngine;
using System.Collections;

/// 
/// Diese Klasse Beschränkt die Bewegung auf die Horizontale,
/// also Bewegung ist nur nach Links oder Rechts möglich
/// 
public abstract class MLeftRight : MovableEntity {
	
	
	
	// Konstruktor
	
	/// <summary>
	/// Initializes a new instance of the <see cref="MLeftRight`1"/> class.
	/// </summary>
	/// <param name='maxHealth'>
	/// Maximale Trefferpunkte des Gegners. Bei 0 HP stirbt der Gegner.
	/// </param>
	public MLeftRight() : base(){
		
	}
		
	
	// FilterForce
	
	/// <summary>
	/// Beschränkt die Bewegung auf Links/Rechts durch Drehung des Vectors
	/// </summary>
	protected override Vector3 FilterForce(Vector3 vIn){
		//Keine Bewegung
		if(vIn == Vector3.zero) 
			return vIn;
		
		//Winkel zwischen Vector nach Links und der Kraft
		float a = Vector3.Angle(Vector3.left, vIn);
		
		//Winkel kleiner 90° bedeutet die Kraft geht eher nach Links
		if(a < 90.000f)
			//also die Bewegung nach Links drehen
			return Vector3.left * vIn.magnitude;
		//Winkel größer 90° bedeutet die Kraft geht eher nach Rechts
		else if(a > 90.000f)
			//also die Bewegung nach Rechts drehen
			return Vector3.right * vIn.magnitude;
		//Weder Links noch Rechts
		return Vector3.zero;
	}

	
	
	/// <summary>
	/// Richtung in die der Gegner guckt.
	/// Lokales Koordinatensystem.
	/// </summary>
	public override Vector3 Heading { get{
			return Vector3.right;
		}
	}
	
	
	
	
	/// <summary>
	/// Methode ob in der Bewegungsrichtung ein Hindernis ist.
	/// Funktioniert sowohl mit Links, Rechts, Oben sowie Unten.
	/// </summary>
	/// <returns>
	/// <c>true</c> wenn kein Hindernis vor einem ist; ansonsten, <c>false</c>.
	/// </returns>
	/// <param name='heading'>
	/// Die Richtung Vector3.left, Vector3.right, Vector3.up oder Vector3.down
	/// </param>
	protected bool NoObstacle(Vector3 heading){
		Vector3 direction;
		Vector3 side;
		
		//Links/Rechts
		if(heading == Vector3.left || heading == Vector3.right){
			direction = Pos + heading * Width / 1.95f;
			side = Vector3.up * Height / 2.15f;
		}
		//Oben/Unten
		else if(heading == Vector3.up || heading == Vector3.down){
			direction = Pos + heading * Height / 1.95f;
			side = Vector3.left * Width / 2.15f;
		}
		//Unbekannte Richtung
		else {
			return false;
		}
		
		Debug.DrawLine(Pos, direction, Color.red);
		Debug.DrawLine(Pos + side, direction + side, Color.red);
		Debug.DrawLine(Pos - side, direction - side, Color.red);
		
		//alle drei Kollisionstests schlagen fehl
		return  ! Linecast(Pos, direction, Layer.Level) 
			&&  ! Linecast(Pos + side, direction + side, Layer.Level) 
			&&  ! Linecast(Pos - side, direction - side, Layer.Level)
		;
	}
	
	
	
	/// <summary>
	/// Ob der Gegner sich in die Richtung (Links/Rechts) bewegen kann.
	/// </summary>
	/// <returns>
	/// <c>true</c> wenn kein Hindernis und unter einem Boden; ansonsten, <c>false</c>.
	/// </returns>
	/// <param name='heading'>
	/// Die Richtung Vector3.left oder Vector3.right
	/// </param>
	private bool CanMoveToHeading(Vector3 heading){
		
		Vector3 direction = Pos + heading * Width / 1.95f; // aus dem Collider heraus
		Vector3 direction2 = Pos + heading * Width / 4.0f; //nicht aus dem Collider heraus
		Vector3 down = Vector3.down * Height / 1.8f; // aus dem Collider heraus
	
		Debug.DrawLine(direction, direction + down, Color.blue);
		Debug.DrawLine(direction2, direction2 + down, Color.blue);
		
		return 
			//vor einem ist kein Hindernis
			NoObstacle(heading)
			//und vor einem ist Boden auf dem gestanden werden kann
			&& Linecast(direction, direction + down, Layer.Level)
			&& Linecast(direction2, direction2 + down, Layer.Level)
		;
	}
	
	
	
	/// <summary>
	/// Ob der Spieler nach Links gehen kann.
	/// </summary>
	/// <returns>
	/// <c>true</c> wenn kein Hindernis den Weg blockiert und Boden vorhanden ist; ansonsten, <c>false</c>.
	/// </returns>
	public bool CanMoveLeft{ get{
			return CanMoveToHeading(Vector3.left);
		}
	}
	
	
	
	/// <summary>
	/// Ob der Spieler nach Rechts gehen kann.
	/// </summary>
	/// <returns>
	/// <c>true</c> wenn kein Hindernis den Weg blockiert und Boden vorhanden ist; ansonsten, <c>false</c>.
	/// </returns>
	public bool CanMoveRight{ get{
			return CanMoveToHeading(Vector3.right);
		}
	}
	
	
	
	/// <summary>
	/// Ob der Spieler in Richtung (Links/Rechts) der Position gehen kann.
	/// </summary>
	/// <param name='pos'>
	/// Die Position die überprüft werden soll
	/// </param>
	/// <param name='invertDirection'>
	/// Ob das Ergebnis invertiert werden soll (z.B. Flee statt Seek)
	/// </param>
	public bool CanMoveTo(Vector3 pos, bool invertDirection = false){
		if(IsRight(pos) ^ invertDirection) //ist das Ziel rechts vom Gegner?
			return CanMoveRight;
		else 
			return CanMoveLeft;
	}
	
	
	
}
