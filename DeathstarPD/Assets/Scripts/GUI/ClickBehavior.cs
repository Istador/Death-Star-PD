using UnityEngine;
using System.Collections;

public class ClickBehavior : MonoBehaviour {

	CameraControler cam;

	void Start(){
		cam = GameObject.Find("Main Camera").GetComponent<CameraControler>();
	}

	public void Click() {
		if(Pause.I.Paused || Time.timeScale == 0f) return;

		if(!cam.moving){
			TowerSelect.I.Click();
			TowerBuilding.I.Build();
		}
	}

}
