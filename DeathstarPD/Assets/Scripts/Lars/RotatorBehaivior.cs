using UnityEngine;
using System.Collections;

public class RotatorBehaivior : MonoBehaviour {

	public float speed = 2;
	public bool rotate { get; set; }

	private Vector2 mousePos;


	// Use this for initialization
	void Start () {
		rotate = true;
		mousePos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(rotate){
			transform.Rotate(0, speed * Time.deltaTime, 0);
		}else{
			if(Input.GetMouseButton(0)){
				float delta = mousePos.x - Input.mousePosition.x;
				transform.Rotate(0, delta, 0);
			}
		}
		mousePos = Input.mousePosition;
	}

	void ToggleRotation(){
		if(rotate){
			rotate = false;
		}else{
			rotate = true;
		}
	}
}
