using UnityEngine;

public class TowerBuilding : GeneralObject {

	/// <summary>
	/// Minimale Distanz zum nächsten Turm um bauen zu können
	/// </summary>
	private float MinimumDistanceToNextTower = 8f;

	/// <summary>
	/// wie weit das Objekt in den Planeten hineinragt
	/// </summary>
	private float Offset = 2.0f;

	/// <summary>
	/// Container in den alle Prototyp-Türme kommen
	/// </summary>
	private Transform Container;
	private GameObject RangeSphere;
	private Vector3 sphereScale = new Vector3(2f, 2f, 2f);

	public int SelectedIndex { get; private set; }
	public Tower Selected { get { return (SelectedIndex == -1) ? null : Prototypes[SelectedIndex] ; } }
	public Tower[] Prototypes { get; private set; }

	/// <summary>
	/// gespeicherte Original Materials
	/// </summary>
	private Material[][] materials;

	/// <summary>
	/// Höhe des Objektes, um es nicht halb im Planeten anzuzeigen
	/// </summary>
	private float height = 0f;

	protected override void Start() {
		SelectedIndex = -1;

		//Container holen
		Container = Towers.Container.transform.FindChild("TowerPrototypes");
		RangeSphere = Container.FindChild("RangeSphere").gameObject;

		//Prototypen erzeugen
		Prototypes = new Tower[]{
			  Instantiate("Towers/MGTower").GetComponent<Tower>()
			, Instantiate("Towers/LaserTower").GetComponent<Tower>()
			, Instantiate("Towers/LightningTower").GetComponent<Tower>()
			, Instantiate("Towers/RocketTower").GetComponent<Tower>()

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
		//nicht während das Spiel pausiert ist
		if(Pause.I.Paused || Time.timeScale == 0f) return;

		//Wenn ein Turm ausgewählt ist
		if(SelectedIndex != -1){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Wenn die Maus über dem Planeten ist
			if(Physics.Raycast(ray, out hit, 1000f, (int)Layer.Level)){
				//Position, wo der gezeichnet werden soll
				Selected.transform.position = hit.point.normalized * (
					hit.point.magnitude
					+ height * 0.5f
					- Offset
				);
				Selected.transform.LookAt(Vector3.zero); //zum Kugel-Mittelpunkt ausrichten
				Selected.transform.Rotate(0, -90, 90); //Richtig drehen
				Selected.Visible = true; //Sichtbar

				//farbliche rot/grün Markierung (kann bauen / kann nicht bauen)
				RangeSphere.renderer.materials = Resource.UsableMaterial(CanBuild ? "CanBuild" : "CantBuild");
				RangeSphere.transform.position = Selected.transform.position;
				RangeSphere.renderer.enabled = true;

			} else {
				//wenn die Maus über dem All ist
				Selected.Visible = false; //Unsichtbar
				RangeSphere.renderer.enabled = false;
			}
		}
	}



	/// <summary>
	/// Ob der Abstand zum nächsten Turm eingehalten wird
	/// </summary>
	public bool CanBuild { get {
		return Physics.OverlapSphere(Selected.Pos, MinimumDistanceToNextTower, (int)Layer.Building | (int)Layer.Tower).Length == 0;
	}}



	public void Select(int index){
		if(index < 0 || index >= Prototypes.Length){
			Debug.LogError("Trying to select non-existant tower "+index+".");
			return;
		}

		//nicht während das Spiel pausiert ist
		if(Pause.I.Paused || Time.timeScale == 0f) return;

		//nicht genug Geld zum bauen
		if(!GameResources.I.EnoughMoney(Prototypes[index].MoneyBuildCost) || !GameResources.I.EnoughCookies(Prototypes[index].CookieBuildCost)){
			//TODO GUI: Fehlermeldung ausgeben, weil nicht genug Geld zum bauen des Turms vorhanden ist
			return;
		}

			
		//Wenn sich die Auswahl ändert
		if(SelectedIndex != index){

			Deselect(); //Wenn vorher schon einer ausgewählt war, ihn erst deselektieren

			SelectedIndex = index; //Auswahl ändern

			Selected.Active = true; //gameObject aktiviere

			//RangeSphere skalieren
			RangeSphere.transform.localScale = sphereScale * Selected.Range;

			//Höhe berechnen
			height = Utility.HeightYByCollider(Selected.gameObject);
			//Debug.Log("Height: "+height);
		}
	}

	public void Deselect(){
		//wenn ein Turm ausgewählt ist
		if(SelectedIndex != -1){
			Selected.Active = false; //gameObjekt deaktivieren
			Selected.Visible = false; //unsichtbar

			//RangeSphere ausblenden
			RangeSphere.renderer.enabled = false;

			SelectedIndex = -1;
		}
	}

	public void Build(){
		//Nur wenn ein Turm zum bauen ausgewählt ist, dieser gezeichnet wird, und bauen möglich ist
		if(SelectedIndex != -1 && Selected.Visible && CanBuild){
			//Dem Spieler Geld für den Turmbau abziehen
			GameResources.I.Money -= Selected.MoneyBuildCost;
			GameResources.I.Cookies -= Selected.CookieBuildCost;
			
			//Turm erzeugen
			Tower t = Selected.Instantiate(Selected.gameObject).GetComponent<Tower>();
			t.transform.parent = Towers.Container.transform; //in Turm-Container
			t.transform.LookAt(Vector3.zero); //zum Kugel-Mittelpunkt ausrichten
			t.transform.Rotate(0, -90, 90); //Richtig drehen
			t.collider.enabled = true; //Collider einschalten (ist beim Prototypen ausgeschaltet)
			t.enabled = true; //Tower Skript aktivieren (ist beim Prototypen ausgeschaltet)

			//Verlasse den Baumodus
			Deselect();
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
