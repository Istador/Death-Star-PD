using UnityEngine;
using System.Collections;

public class TargetPractice : MovableEntity {

	protected override void Update () {
		int width = Screen.width / 2;
		Vector3 newPos = new Vector3(0,0,-500);
		if(Input.GetMouseButton(1)){
			newPos.z = -12;
		}
		newPos.x = Input.mousePosition.x / 50 - width / 50;
		newPos.y = Input.mousePosition.y / 50;
		transform.position = newPos;
	}
}
