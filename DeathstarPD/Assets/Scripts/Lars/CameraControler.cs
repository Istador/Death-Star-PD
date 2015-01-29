using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControler : MonoBehaviour {

	public static readonly float twopi = Mathf.PI * 2f;
	public static readonly float pihalf = Mathf.PI / 2f;
	public static readonly float threepihalf = pihalf * 3f;

	//back Camera
	public Transform backCamera;

	//Zoomborders
	public static readonly float maxRadius = 350f;
	public static readonly float minRadius = 150f;
	public static readonly float zoomSpeedFactor = 2f;


	//Mouse Controls
	public bool moving { get; private set; }
	private Vector3 lastPos;


	//RotationVariables
	private float rotation = 0;
	public static readonly float startRadius = 250f;
	private float radius = startRadius;
	private float topRotation = 0f;
	private Vector3 up = Vector3.up;


	// Use this for initialization
	void Start() {
		lastPos = Input.mousePosition;
		moving = false;
		calculateRotation();
	}


	private void calculateRotation(){
		//Main Camera
		float nradius = Mathf.Cos(topRotation) * radius;
		transform.position = new Vector3(
			Mathf.Cos(rotation) * nradius,
			Mathf.Sin(topRotation) * radius,
			Mathf.Sin(rotation) * nradius
			);
		transform.LookAt(Vector3.zero, up);

		//Back Camera
		nradius = Mathf.Cos(topRotation) * startRadius;
		backCamera.position = new Vector3(
			Mathf.Cos(rotation - Mathf.PI) * nradius,
			Mathf.Sin(topRotation) * startRadius,
			Mathf.Sin(rotation - Mathf.PI) * nradius
			);
		backCamera.LookAt(Vector3.zero, up);
	}


	public void BeginDrag(){
		if(Pause.I.Paused || Time.timeScale == 0f) return;
		//Debug.Log("BeginDrag");

		Screen.showCursor = false;
		moving = true;

		lastPos = Input.mousePosition;
	}


	public void Drag(){
		if(Pause.I.Paused || Time.timeScale == 0f) return;
		//Debug.Log("Drag");

		//top rotation
		float dy = lastPos.y - Input.mousePosition.y;
		topRotation += dy / 100f;
		topRotation %= twopi;
		if(topRotation < 0f) topRotation += twopi;

		up = topRotation > pihalf && topRotation <= threepihalf ? Vector3.down : Vector3.up;

		//side rotation
		float dx = lastPos.x - Input.mousePosition.x;
		rotation += dx * up.y / 100f;
		rotation %= twopi;
		if(rotation < 0f) rotation += twopi;

		//Debug.Log(rotation+", "+topRotation);

		//change camera
		calculateRotation();

		//set Mousposition to determine DeltaX and DeltaY
		lastPos = Input.mousePosition;
	}


	public void EndDrag(){
		//Debug.Log("EndDrag");
		moving = false;
		Screen.showCursor = true;
	}


	public void Scroll(BaseEventData data){
		PointerEventData ped = data as PointerEventData;
		if(ped != null){
			//how much rows the scroll wheel has changed since the last Scrtoll-Event
			float scrollSpeed = -ped.scrollDelta.y;

			//calculate speed
			float zoomSpeed = radius / 100f * zoomSpeedFactor * scrollSpeed;

			//zoom
			radius += zoomSpeed;

			//satisfy zoom constraints
			Utility.MinMax(ref radius, minRadius, maxRadius);

			//change camera
			calculateRotation();
		}
	}

	
}
