using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour {

	public Transform barrel;
	public Transform basis;

	public MovableEntity target;

	private Quaternion startRot;
	private Vector3 barrelAnchor;

	private Tower self;

	// Use this for initialization
	void Start () {
		self = gameObject.GetComponent<Tower>();
		startRot = basis.rotation;
	}


	
	// Update is called once per frame
	void Update () {
		target = self.Target;

		if(target != null && !target.IsDead && target.enabled){

			//Rotation der Basis
			basis.rotation = startRot;
			Vector3 delta = basis.InverseTransformPoint(target.Pos);
			//float height = delta.y;
			delta.y = 0;
			delta = basis.TransformPoint(delta);
			Debug.DrawLine(basis.position, delta, Color.blue);
			basis.LookAt(delta, basis.TransformPoint(Vector3.up));
			basis.Rotate(0,-90,0);

			// Rotation des Geschützes
			// up vector
			Vector3 up = self.Pos;
			// vector to target
			Vector3 toTarget = target.Pos - self.Pos;
			//angle between up and target
			float angle = (90f - Vector3.Angle(up, toTarget));
			Utility.MinMax(ref angle, 0f, 90f);
			Vector3 r = barrel.localEulerAngles;
			r.z = angle;
			barrel.localEulerAngles = r;
		}
	}
}
