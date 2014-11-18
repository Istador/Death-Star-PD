using UnityEngine;
using System.Collections;

public class TowerBuilding : GeneralObject {

	/// <summary>
	/// Minimale Distanz zum nächsten Turm um bauen zu können
	/// </summary>
	//private float MinimumDistanceToNextTower = 5f;

	/// <summary>
	/// wie weit das Objekt in den Planeten hineinragt
	/// </summary>
	private float Offset = 1.5f;

	/// <summary>
	/// Container in den alle Prototyp-Türme kommen
	/// </summary>
	private Transform Container;

	public int SelectedIndex { get; private set; }
	public Tower Selected { get { return (SelectedIndex == -1) ? null : Prototypes[SelectedIndex] ; } }
	public Tower[] Prototypes { get; private set; }

	protected override void Start() {
		SelectedIndex = -1;

		//Container holen
		Container = Towers.Container.transform.FindChild("TowerPrototypes");

		//Prototypen erzeugen
		Prototypes = new Tower[]{
			  Instantiate("LaserTower").GetComponent<Tower>()
			, Instantiate("RocketTower").GetComponent<Tower>()
		};

		//für jeden Prototyp
		for(int i = 0; i < Prototypes.Length; i++){
			Tower t = Prototypes[i];
			//In den Container
			t.transform.parent = Container;
			//kein Collider
			t.collider.enabled = false;
			//Unischtbar
			t.Visible = false;
			//Tower Skript deaktivieren
			t.enabled = false;
			//Game ObjectDeaktivieren
			t.Active = false;
			//Entferne aus Towers-Collection
			Towers.I.Remove(t);
			//Namen ändern
			t.name = t.GetType().ToString()+" (Prototype)";
			//auswählbar über Tastendruck
			int index = i; //Closure Variable
			Inputs.I.Register("Tower"+(i+1).ToString(), () => Select(index));
		}

	}

	protected override void Update(){
		//Wenn ein Turm ausgewählt ist
		if(SelectedIndex != -1){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Wenn die Maus über dem Planeten ist
			if(Physics.Raycast(ray, out hit, 1000f, (int)Layer.Level)){
				//Position, wo der gezeichnet werden soll
				Selected.transform.position = hit.point.normalized * (
					hit.point.magnitude
					+ Selected.transform.localScale.z * 0.5f
					- Offset
				);
				Selected.transform.LookAt(Vector3.zero); //zum Kugel-Mittelpunkt ausrichten
				Selected.transform.eulerAngles += new Vector3(0,-90,90); //Richtig drehen
				Selected.Visible = true; //Sichtbar
				//TODO Lars: farbliche rot/grün Markierung (kann bauen / kann nicht bauen)
				// if(CanBuild); //Grün
				// else ; //Rot
			} else {
				//wenn die Maus über dem All ist
				Selected.Visible = false; //Unsichtbar
			}
		}
	}

	public bool CanBuild { get {
		//TODO Robin: Funktion um Abstand zu nächstem Turm einzuhalten
		return true;
	}}

	public void Select(int index){
		if(index < 0 || index >= Prototypes.Length){
			Debug.Log("Error: Trying to select non-existant tower "+index+".");
			return;
		}
		//Wenn sich die Auswahl ändert
		if(SelectedIndex != index){
			//Wenn vorher schon einer ausgewählt war, ihn deselecten
			Deselect();
			//Auswahl ändern
			SelectedIndex = index;
			//gameObject aktivieren
			Selected.Active = true;

			//Debug.Log("Info: "+Selected.GetType()+" ausgewählt.");
		}
	}

	public void Deselect(){
		//wenn ein Turm ausgewählt ist
		if(SelectedIndex != -1){
			//Debug.Log("Info: "+Selected.GetType()+" nicht mehr ausgewählt.");
			//gameObjekt deaktivieren
			Selected.Active = false;
			//unsichtbar
			Selected.Visible = false;
			SelectedIndex = -1;
		}
	}

	public void Build(){
		//Nur wenn ein Turm zum bauen ausgewählt ist, dieser gezeichnet wird, und bauen möglich ist
		if(SelectedIndex != -1 && Selected.Visible && CanBuild){
			//Turm erzeugen
			Tower t = Selected.Instantiate(Selected.gameObject).GetComponent<Tower>();
			//in Turm-Container
			t.transform.parent = Towers.Container.transform;
			//zum Kugel-Mittelpunkt ausrichten
			t.transform.LookAt(Vector3.zero);
			t.transform.eulerAngles += new Vector3(0,-90,90); //Richtig drehen
			//Collider einschalten (ist beim Prototypen ausgeschaltet)
			t.collider.enabled = true;
			//Tower Skript aktivieren (ist beim Prototypen ausgeschaltet)
			t.enabled = true;
			//Verlasse den Baumodus
			Deselect();

			//Debug.Log("Info: "+t.GetType()+" gebaut.");
		}
	}

	
	/**
	 * Singleton Reference (not Object :( )
	*/
	private static TowerBuilding instance;
	public TowerBuilding(){instance = this;}
	public static TowerBuilding Instance{get{return instance;}}
	public static TowerBuilding I{get{return Instance;}}
}
