using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Zeichnet mehrere Glyphen nebeneinander auf der selben Zeile
/// </summary>
public class GConcat : Glyph {
	
	
	/// <summary>
	/// Alle Sub-Glyphen
	/// </summary>
	private List<Glyph> glyphs = new List<Glyph>();
	


	protected GConcat(params Glyph[] gls){
		foreach(Glyph g in gls)
			this.glyphs.Add(g);
	}
	
	
	
	public void Draw(double size, Vector2 pos){
		//Für alle Sub-Glyphen
		foreach(Glyph g in glyphs){
			//Breite berechnen
			double w = g.Width(size);
			//Zeichne diesen Glyph
			g.Draw(size, pos);
			//Berechne die Position des nächsten Glyphen
			pos = new Vector2((float)(pos.x + w), pos.y);
		}
	}
	
	
	private Dictionary<double, double> widths = new Dictionary<double, double>();
	public double Width(double size){
		//Annahme, dass sich size selten ändert. Deshalb Width cachen.
		if(!widths.ContainsKey(size)){
			//Summiere die Breiten aller Glyphen
			double w = 0.0;
			foreach(Glyph g in glyphs)
				w += g.Width(size);
			widths.Add(size, w);
		}
		return widths[size];
	}
	
	
	private Dictionary<double, double> heights = new Dictionary<double, double>();
	public double Height(double size){
		//Annahme, dass sich size selten ändert. Deshalb Height cachen.
		if(!heights.ContainsKey(size)){
			//Maximale Höhe
			double h = 0.0;
			foreach(Glyph g in glyphs){
				double nh = g.Height(size);
				if(nh > h) h = nh;
			}
			heights.Add(size, h);
		}
		return heights[size];
	}


	
	public static GConcat Concat(params Glyph[] glyphs){
		return new GConcat(glyphs);
	}



}
