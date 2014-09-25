using UnityEngine;
using System.Collections;

public class Pew : MonoBehaviour {

	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += -transform.right *speed;
	}
}
