using UnityEngine;
using System.Collections;

/// 
/// Dieses Objekt wird nur zufällig erstellt.
/// 
public class RandomObject : GeneralObject {
	
	
	
	/// <summary>
	/// Wahrscheinlichkeit, dass das Objekt erstellt wird
	/// 
	/// Wertebereich: 0.0 bis 1.0
	/// Default: 0.5 (=> 50 % objekt vorhanden, 50 % nicht vorhanden)
	/// </summary>
	protected float f_RandomObjectProbability = 0.5f;
	
	
	
	protected override void Start () {
		if(rnd.NextDouble() > f_RandomObjectProbability){
			//zerstöre das Objekt
			gameObject.SetActive(false);
			Destroy(gameObject);
		} else {
			base.Start();
		}
	}
	
	
	
}
