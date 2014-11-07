using UnityEngine;
using System.Collections;

public class Field {
	private Color color;
	private Vector3 position;
	private float size;
	private bool[] faces = new bool[5];

	public Field(Vector3 pos, float s, string c){
		if(c.StartsWith("\"#")){
			this.color = ColorX.HexToRGB(c.Substring(2, c.Length-3));
		}else{
			this.color = ColorX.HexToRGB(c);
		}
		this.position = pos;
		this.size = s;
	}

	public Color Color {
		get {
			return color;
		}
		set {
			color = value;
		}
	}

	public Vector3 Position {
		get {
			return position;
		}
		set {
			position = value;
		}
	}

	public float Size {
		get {
			return size;
		}
		set {
			size = value;
		}
	}

	public void setFaceActive(int i, bool v){
		faces[i] = v;
	}

	public bool getFaceActive(int i){
		return faces[i];
	}

	public void setFaceArray(bool[] ar){
		faces = ar;
	}

	public bool[] getFaceArray(){
		return faces;
	}
}
