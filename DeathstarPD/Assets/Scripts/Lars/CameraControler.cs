﻿using UnityEngine;
using System.Collections;

public class CameraControler : MonoBehaviour {

	//The GameObject to place on Click
	public GameObject tower;

	//Delay till a hold mousebutton is aknowledged
	public int delay = 10;

	//Zoomborders
	public float maxRadius = 300f;
	public float minRadius = 110f;
	public float zoomSpeedFactor = 2f;
	private float zoomSpeed;

	//Mouse Controls
	private bool moving = false;
	private Vector3 lastPos;
	private int timer = 0;

	//RotationVariables
	private float rotation = 0;
	public float radius = 450f;
	public float topRotation = 0f;

	// Use this for initialization
	void Start () {
		lastPos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		//Click to place a building
		if(Input.GetMouseButtonUp(0) && moving == false){
			placeBuilding();
		}

		//Click and drag to move Camera
		if(Input.GetMouseButton(0)){
			if(moving == false){
				timer += 1;
			}
			if(timer >= delay){
				Screen.showCursor = false;
				moving = true;
				float dx = lastPos.x - Input.mousePosition.x;
				float dy = lastPos.y - Input.mousePosition.y;
				rotation += dx/100;
				topRotation += dy/100;
			}
		}else{
			moving = false;
			timer = 0;
			Screen.showCursor = true;
		}

		//Zoom out
		if(Input.GetAxis("Mouse ScrollWheel") < 0 && radius < maxRadius){
			radius += zoomSpeed;
			zoomSpeed = radius/100*zoomSpeedFactor;
		}

		//Zoom in
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && radius > minRadius){
			radius -= zoomSpeed;
			zoomSpeed = radius/100*zoomSpeedFactor;
		}

		//Calculate CameraPosition and set Mousposition to determine DeltaX and DeltaY
		calculateRotation();
		lastPos = Input.mousePosition;
	}

	private void calculateRotation(){
		float nradius = Mathf.Cos (topRotation) * radius;
		Vector3 newPos = new Vector3 (Mathf.Cos (rotation) * nradius, Mathf.Sin(topRotation)*radius, Mathf.Sin (rotation) * nradius);
		transform.position = newPos;
		transform.LookAt(new Vector3(0,0,0));
	}

	private void placeBuilding(){
		Debug.Log ("Casting Ray...");
		RaycastHit vHit = new RaycastHit();
		Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(vRay, out vHit, 1000))
		{
			GameObject t = (GameObject) GameObject.Instantiate(tower);
			t.transform.position = vHit.point;
			t.transform.LookAt(new Vector3(0,0,0));
		}
	}
}