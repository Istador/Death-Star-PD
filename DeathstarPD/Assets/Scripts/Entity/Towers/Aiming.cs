using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour {

	public Transform barrel;
	public Transform basis;

	public MovableEntity target;

	private float lastAngle = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		target = gameObject.GetComponent<Tower>().Target;
		if(target){
			Vector3 t = new Vector3(target.Pos.x , transform.position.y, target.Pos.z);
			basis.LookAt(t);
			basis.Rotate(new Vector3( 0, -90, 0));

			//#TODO Lars: Barrel Ausrichtung der Tower implemntieren
//			float x = Vector3.Distance(transform.position, new Vector3(target.Pos.x, transform.position.y, target.Pos.z));
//			float y = Vector3.Distance(transform.position, new Vector3(transform.position.x, target.Pos.y, transform.position.z));
//			float angle = Mathf.Atan(y / x);
//
//			barrel.RotateAround(transform.parent.transform.position, new Vector3(0,0,1), lastAngle - angle);
//			lastAngle = angle;
		}
	}
}
