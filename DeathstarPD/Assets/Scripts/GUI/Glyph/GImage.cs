using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Zeichnet eine Textur.
/// </summary>
public class GImage : Glyph {

	private Texture2D img;



	public GImage(Texture2D img){
		this.img = img;
	}



	public void Draw(double size, Vector2 pos){
		//Ausmaße des Rechteckes berechnen
		Rect r = new Rect(pos.x, pos.y, (float)Width(size), (float)Height(size));
		//Bild zeichnen und skalieren
		GUI.DrawTexture(r, img, ScaleMode.ScaleToFit);
	}



	private Dictionary<double, double> widths = new Dictionary<double, double>();
	public double Width(double size){
		//Annahme, dass sich size selten ändert. Deshalb Width cachen.
		if(!widths.ContainsKey(size)){
			double w = ((double)img.width) * size;
			widths.Add(size, w);
		}
		return widths[size];
	}
	
	
	
	private Dictionary<double, double> heights = new Dictionary<double, double>();
	public double Height(double size){
		//Annahme, dass sich size selten ändert. Deshalb Height cachen.
		if(!heights.ContainsKey(size)){
			double h = ((double)img.height) * size;
			heights.Add(size, h);
		}
		return heights[size];
	}

}
