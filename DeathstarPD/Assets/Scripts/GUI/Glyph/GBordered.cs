using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Klasse für die Breite von Rand-Positionen.
/// </summary>
public class BorderPosition {
	public double top, bottom, left, right;
	
	public BorderPosition(double forall){
		all = forall;
	}
	
	public double all {set{
			top = value;
			bottom = value;
			left = value;
			right = value;
		}}
	
	public double Width { get{return left+right;} }
	public double Height { get{return top+bottom;} }
}

/// <summary>
/// Zeichnet einen Rand um einen Glyphen.
/// </summary>
public class GBordered : Glyph {
	private Glyph g;

	public GBordered(Glyph g){
		this.g = g;
	}


	/// <summary>
	/// Außenabstand vom Rand zu anderen Glyphen
	/// </summary>
	public readonly BorderPosition Margin = new BorderPosition(0.0);
	/// <summary>
	/// Breite des Randes
	/// </summary>
	public readonly BorderPosition Border = new BorderPosition(1.0); //in pixeln
	/// <summary>
	/// Innenabstand vom Rand zum Sub-Glyphen
	/// </summary>
	public readonly BorderPosition Padding = new BorderPosition(1.0);

	/// <summary>
	/// Randfarbe
	/// </summary>
	public Color BorderColor = Color.black;

	/// <summary>
	/// Ob der Rand gezeichnet werden soll.
	/// Der Sub-Glyph wird immer gezeichnet.
	/// </summary>
	public bool Enabled = true;


	private Dictionary<double, Rect[]> borderCache = new Dictionary<double, Rect[]>();

	/// <summary>
	/// double to float
	/// </summary>
	private Rect RectFromDouble(double left, double top, double width, double height){
		return new Rect((float)left, (float)top, (float)width, (float)height);
	}

	/// <summary>
	/// Bewegt das vorberechnete Rechteck um den aktuellen Positionsvektor
	/// </summary>
	/// <param name="r">Rechteck.</param>
	/// <param name="v">Positionsvektor.</param>
	private Rect Move(Rect r, Vector2 v){
		return new Rect(
			Mathf.Floor(r.x + v.x),
		    Mathf.Floor(r.y + v.y),
			r.width,
			r.height
		);
	}

	/// <summary>
	/// Zeichnet den Rand, und berechnet, falls noch nicht geschehen, dessen Größen.
	/// </summary>
	/// <param name="size">Skalierungsfaktor.</param>
	/// <param name="pos">Position.</param>
	private void drawBorder(double size, Vector2 pos){
		//nur einmalig unabhängig von der Position berechnen
		if(!borderCache.ContainsKey(size)){
			double w = g.Width(size);
			double h = g.Height(size);

			double width = ((Border.left + Padding.left + Padding.right + Border.right)*size + w);
			double height = ((Border.top + Padding.top + Padding.bottom + Border.bottom)*size + h);

			double top = (Margin.top * size);
			double bottom = ((Margin.top + Border.top + Padding.top + Padding.bottom)*size + h);
			double left = (Margin.left * size);
			double right = ((Margin.left + Border.left + Padding.left + Padding.right)*size + w);

			Rect rTop = RectFromDouble(left, top, width, Border.top*size);
			Rect rBottom = RectFromDouble(left, bottom, width, Border.bottom*size);
			Rect rLeft = RectFromDouble(left, top, Border.left*size, height);
			Rect rRight = RectFromDouble(right, top, Border.right*size, height);

			borderCache.Add(size, new Rect[]{rTop, rBottom, rLeft, rRight});
		}

		//vier Linien zeichnen, um ein Rechteck um den Glyphen zu erzeugen
		foreach(Rect r in borderCache[size])
			Utility.DrawRectangle(Move(r,pos), BorderColor);
	}



	private Dictionary<double, Vector2> innerCache = new Dictionary<double, Vector2>();

	private Vector2 innerPos(double size){
		if(!innerCache.ContainsKey(size)){
			Vector2 pos = new Vector2(
				(float)(size * (Margin.left + Border.left + Padding.left)),
				(float)(size * (Margin.top + Border.top + Padding.top))
			);
			innerCache.Add(size, pos);
		}
		return innerCache[size];
	}



	public void Draw(double size, Vector2 pos){
		g.Draw(size, pos + innerPos(size));
		if(Enabled) drawBorder(size, pos);
	}



	private Dictionary<double, double> widths = new Dictionary<double, double>();
	public double Width(double size){
		if(!widths.ContainsKey(size)){
			double w = g.Width(size) + (Margin.Width + Border.Width + Padding.Width) * size;
			widths.Add(size, w);
		}
		return widths[size];
	}



	private Dictionary<double, double> heights = new Dictionary<double, double>();
	public double Height(double size){
		if(!heights.ContainsKey(size)){
			double h = g.Height(size) + (Margin.Height + Border.Height + Padding.Height) * size;
			heights.Add(size, h);
		}
		return heights[size];
	}



}