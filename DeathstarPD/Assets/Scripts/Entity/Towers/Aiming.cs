using UnityEngine;
using System.Collections;

public class Aiming : MonoBehaviour {

	public Transform basis;
	public Transform barrelAnchor;
	public bool barrelInverse;

	public MovableEntity target;

	private Quaternion startRot;


	private Tower self;

	// Use this for initialization
	void Start () {
		self = gameObject.GetComponent<Tower>();
		startRot = basis.rotation;
		if(barrelAnchor == null)
			barrelAnchor = transform;
	}


	
	// Update is called once per frame
	void Update () {
		target = self.Target;

		if(target != null && !target.IsDead && target.enabled){

			//Rotation der Basis
			if(basis != null){
				basis.rotation = startRot;
				Vector3 delta = basis.InverseTransformPoint(target.Pos);
				//float height = delta.y;
				delta.y = 0;
				delta = basis.TransformPoint(delta);
				Debug.DrawLine(basis.position, delta, Color.blue);
				basis.LookAt(delta, basis.TransformPoint(Vector3.up));
				basis.Rotate(0,-90,0);
			}
			// Rotation des Geschützes
			// up vector
			if(barrelAnchor != null){
				Vector3 up = barrelAnchor.position;
				// vector to target
				Vector3 toTarget = target.Pos - up;
				//angle between up and target
				float angle = (90f - Vector3.Angle(up, toTarget));
				Utility.MinMax(ref angle, 0f, 90f);
				if(barrelInverse) angle *= -1;
				Vector3 r = barrelAnchor.localEulerAngles;
				r.x = angle;
				barrelAnchor.localEulerAngles = r;
			}
		}
	}
}
