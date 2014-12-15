using UnityEngine;
using System.Collections;

public class PanelRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void rotate(float angle){
		transform.Rotate (0f, 0f, angle);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
