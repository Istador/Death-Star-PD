using UnityEngine;
using System.Collections;

public class Field {

	public Field(Vector3 pos, float s, string c){
		if(c.StartsWith("\"#")){
			Color = ColorX.HexToRGB(c.Substring(2, c.Length-3));
		}else{
			Color = ColorX.HexToRGB(c);
		}
		Position = pos;
		Size = s;
		Faces = new bool[5];
	}

	public Color Color { get; set; }
	public Vector3 Position { get; set; }
	public float Size { get; set; }
	public bool[] Faces { get; set; }

}
