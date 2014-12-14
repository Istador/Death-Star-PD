using UnityEngine;


/// <summary>
/// Diese Klasse erzeugt Primärgebäude auf der Planetenoberfläche und verteilt sie zufällig mit einem Mindestabstand.
/// </summary>
public class SpawnBuildings : GeneralObject {

	//Radius des Planetens
	private static float radius = 100f;

	//Mindestabstand
	private static float min_dist_sq = 80f*80f;
	
	/// <summary>
	/// wie weit das Objekt in den Planeten hineinragt
	/// </summary>
	private float Offset = 2.0f;

	//Namen der Prefabs die erzeugt werden sollen
	//Warnung: bei 80*80 nicht mehr als 14 Gebäude, sonst Endlosschleife!
	private static string[] names = { "CommandCenter", "Hangar", "Hangar", "Hole", "Hole", "Hole", "Hole" };


	private static Building[] buildings = new Building[names.Length];

	protected override void Start () {
		base.Start();

		Spawn();

		//Inputs.I.Register("Fire2", ()=>{Spawn();});

		//Deaktiviere und Entferne dieses Skript nach einmaliger Ausführung.
		enabled = false;
		Destroy(this);
	}

	private void Spawn(){
		//Entferne bereits erzeugte Gebäude
		foreach(Building b in buildings){
			if(b != null) Destroy(b.gameObject);
		}

		int index = 0;
		
		foreach(string name in names){
			
			Vector3 pos = RandomPoint();

			//neues Gebäude erzeugen
			Building b = Instantiate("Buildings/"+name, pos).GetComponent<Building>();
					
			//Höhe berechnen
			float height = Utility.HeightYByCollider(b.gameObject);
					
			b.transform.position = pos.normalized * (
				pos.magnitude
				+ height * 0.5f
				- Offset
			);
					
			b.transform.parent = Buildings.Container.transform;
			b.transform.LookAt(Vector3.zero);
			b.transform.Rotate(0, -90, 90); //Richtig drehen
			buildings[index++] = b;
		} // end foreach
	} // end Spawn()

	private static bool IsDistanceOk(Vector3 pos){
		//für alle bisherigen Gebäude den Abstand prüfen
		for(int i = 0; i < buildings.Length; i++){
			Building b = buildings[i];
			if(b == null) return true; //Ende des Arrays
			else if( b.DistanceSqTo(pos) < min_dist_sq ) return false; //Mindestabstand wird nicht eingehalten
		}
		//alle erfolgreich
		return true;
	}

	public static Vector3 RandomPoint(){
		Vector3 pos = Vector3.zero;

		for(int max=500; max > 0; max--) { // Azahl begrenzen, damit keine Endlosschleife
			//zwei zufällige Winkel
			float alpha = Utility.NextFloat(0f, Mathf.PI);
			float beta = Utility.NextFloat(0f, 2f * Mathf.PI);

			//Berechne den Punkt auf der Kugel
			pos = new Vector3(
				radius * Mathf.Sin(alpha) * Mathf.Cos(beta)
				, radius * Mathf.Sin(alpha) * Mathf.Sin(beta)
				, radius * Mathf.Cos(alpha)
				);
			
			// Wenn alle anderen Gebäude weit genug weg sind
			if(IsDistanceOk(pos))
				break; //Verlasse die Schleife
		}

		return pos;
	}

}
