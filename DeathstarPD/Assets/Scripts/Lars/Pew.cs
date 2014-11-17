using UnityEngine;
using System.Collections;

public class Pew : MonoBehaviour {

	public float speed;
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position += -transform.right * speed * Time.fixedDeltaTime * 50f;
	}
}
