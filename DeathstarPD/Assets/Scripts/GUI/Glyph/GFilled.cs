using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Zeichnet hinter einem transparenten Glyphen ein farbiges Rechteck.
/// </summary>
public class GFilled : Glyph {

	/// <summary>
	/// Ob der Hintergrund gezeichnet werden soll.
	/// Der Sub-Glyph wird auch bei false gezeichnet.
	/// </summary>
	public bool Enabled = true;


	/// <summary>
	/// Farbe des Hintergrund-Rechteckes
	/// </summary>
	public Color Color;


	/// <summary>
	/// Sub-Glyph, der über dem farbigen Hintergrund gezeichnet wird.
	/// </summary>
	private Glyph g;


	/// <summary>
	/// Initializes a new instance of the <see cref="GFilled"/> class.
	/// </summary>
	/// <param name="c">Hintergrundfarbe.</param>
	/// <param name="g">Sub-Glyph.</param>
	public GFilled(Color c, Glyph g){
		this.Color = c;
		this.g = g;
	}



	public void Draw(double size, Vector2 pos){
		if(Enabled){
			//Rechteck-Ausmaße berechnen
			Rect r = new Rect(pos.x, pos.y, (float)Width(size), (float)Height(size));
			//farbiges Rechteck zeichnen
			Utility.DrawRectangle(r, Color);
		}
		//Glyph über dem Rechteck zeichnen
		g.Draw(size, pos);
	}


	
	public double Width(double size){ return g.Width(size); }
	public double Height(double size){ return g.Height(size); }


}
