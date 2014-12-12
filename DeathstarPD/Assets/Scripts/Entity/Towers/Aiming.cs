using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour {

	public Transform barrel;
	public Transform basis;

	public MovableEntity target;

	private Quaternion startRot;
	private Vector3 barrelAnchor;

	// Use this for initialization
	void Start () {
		startRot = basis.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		target = gameObject.GetComponent<Tower>().Target;
		if(target){
			basis.rotation = startRot;
			Vector3 delta = basis.InverseTransformPoint(target.Pos);
			float height = delta.y;
			delta.y = 0;
			delta = basis.TransformPoint(delta);

			Debug.DrawLine(basis.position, delta, Color.blue);

			basis.LookAt(delta, basis.TransformPoint(Vector3.up));
			basis.Rotate(0,-90,0);
			//Barrel Ausrichtung
			//float dist = Vector3.Distance(barrel.position, target.Pos);
			//float angle = Mathf.Asin(height/dist);
			//barrel.Rotate(0, 0, angle);
		}
	}
}
