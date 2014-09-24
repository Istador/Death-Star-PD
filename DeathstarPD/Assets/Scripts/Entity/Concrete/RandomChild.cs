using UnityEngine;
using System.Collections;

public class RandomChild : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Wieviele Kinder es gibts
		int n = transform.childCount;
		
		//ein zufälliges Kind anhand des Indexes auswählen
		int r = Utility.Rnd.Next(n);
		
		GameObject choosen = null;
		
		//für alle Kinder
		for(int i=0; i<n; i++){
			GameObject child = transform.GetChild(i).gameObject;
			//zufälllig ausgewähltes Kind
			if(i==r){
				//Merken des ausgewählten Kindes
				choosen = child;
			}
			//nicht ausgewählte Kinder
			else {
				//Kind zerstören
				child.SetActive(false);
				Destroy(child);
			}
		}
		
		//Kindelement eine Ebene höher
		if(choosen != null)
			choosen.transform.parent = transform.parent;
		
		//dieses Objekt zerstören
		gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
