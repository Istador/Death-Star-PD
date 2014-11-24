using UnityEngine;

public class TowerSelect : GeneralObject {



	public Tower Selected { get; private set; }



	/// <summary>
	/// Einen Turm auswählen
	/// </summary>
	/// <param name="t">T.</param>
	public void Select(Tower t){
		if(t != null){
			Selected = t;
			//Positioniere Panel über den Turm
			panel.position = Camera.main.WorldToScreenPoint(t.Pos);

			//TODO: fülle SelectPanel mit den Turmdaten des ausgewählten Turms

			//zeige das Panel an
			panel.gameObject.SetActive(true);
		} else {
			Unselect();
		}
	}



	/// <summary>
	/// Auswahl aufheben
	/// </summary>
	public void Unselect(){
		Selected = null;
		panel.gameObject.SetActive(false);
	}



	/// <summary>
	/// aufrufen, wenn die Maus auf das Spielfeld klickt. Überprüfe ob ein Turm ausgewählt wird.
	/// </summary>
	public void Click(){
		//nicht wenn im Baumodus
		if(TowerBuilding.I.SelectedIndex != -1) return;

		//schaue ob die Maus über einen Turm liegt
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//Wenn die Maus einen Turm selektiert
		if(Physics.Raycast(ray, out hit, 1000f, (int)Layer.Tower)){
			Tower t = hit.collider.gameObject.GetComponent<Tower>();
			Select(t);
		} else {
			Unselect();
		}
	}



	/// <summary>
	/// Panel, dass über dem Turm angezeigt wird
	/// </summary>
	RectTransform panel;



	protected override void Start(){
		//Vorsicht: Find findet nur aktivierte Objekte
		panel = GameObject.Find("SelectionPanel").GetComponent<RectTransform>();
		panel.gameObject.SetActive(false);
	}



	protected override void Update(){
		//Panel positionieren, sofern auswahl vorhanden
		if(Selected != null){
			panel.position = Camera.main.WorldToScreenPoint(Selected.Pos);
		}
	}



	/**
	 * Singleton Reference (not Object :( )
	*/
	private static TowerSelect instance;
	public TowerSelect(){instance = this;}
	public static TowerSelect Instance{get{return instance;}}
	public static TowerSelect I{get{return Instance;}}
}
