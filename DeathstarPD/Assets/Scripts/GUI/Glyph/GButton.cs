using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// GUI-Button der einen Glyphen als Text/Icon verwendet.
/// Transparent wenn deaktiviert.
/// Ausgegraut beim mouseOver
/// </summary>
public class GButton : Glyph {


	/// <summary>
	/// Anzahl Buttons zählen, um jedem Button eine eindeutige ID geben zu können.
	/// </summary>
	private static int buttons = 0;

	/// <summary>
	/// Eindeutige ID dieses Buttons
	/// </summary>
	private int id;

	/// <summary>
	/// ID als String für den Mouseover-Effekt
	/// </summary>
	private String name;


	/// <summary>
	/// Die Aktion, die ausgeführt werden soll, wenn der Button gedrückt wird
	/// </summary>
	private Action<GButton> action;

	private GFilled gFill;
	private GBordered gBorder;
	private GLimited gLimit;
	private Glyph gContent;

	/// <summary>
	/// Innenabstand des Randes zum Sub-Glyphen.
	/// </summary>
	public BorderPosition Padding {get{return gBorder.Padding;}}
	/// <summary>
	/// sichtbarer Rand des Buttons.
	/// </summary>
	public BorderPosition Border {get{return gBorder.Border;}}
	/// <summary>
	/// Außenabstand vom Rand zu anderen Glyphen.
	/// </summary>
	/// <value>The margin.</value>
	public BorderPosition Margin {get{return gBorder.Margin;}}

	/// <summary>
	/// farbige Hintergrund Füllung zeichnen oder nicht.
	/// </summary>
	/// <value><c>true</c> if filled; otherwise, <c>false</c>.</value>
	public bool Filled {
		get{return gFill.Enabled;}
		set{gFill.Enabled = value;}
	}

	/// <summary>
	/// Hintergrundfarbe
	/// </summary>
	public Color FillColor {
		get{return gFill.Color;}
		set{gFill.Color = value;}
	}

	/// <summary>
	/// Ob der Button aktiviert ist.
	/// Wenn der Button nicht aktiviert ist, wird er transparent gezeichnet.
	/// </summary>
	public bool Enabled = true;

	/// <summary>
	/// Initializes a new instance of the <see cref="GButton"/> class.
	/// </summary>
	/// <param name="width">Breite..</param>
	/// <param name="height">Höhe.</param>
	/// <param name="g">Sub-Glyph.</param>
	/// <param name="action">Aktion die beim betätigen ausgeführt werden soll.</param>
	public GButton(double width, double height, Glyph g, Action<GButton> action){
		this.action = action;

		id = buttons++;
		name = "GButton"+id;

		gContent = g;
		gLimit = new GLimited(width, height, gContent);
		gBorder = new GBordered(gLimit);
		gFill = new GFilled(Color.white, gBorder);

		gBorder.Enabled = false;
	}

	public void Draw(double size, Vector2 pos){

		//transparenter GUI-Button (zum anklicken)
		Color tmp = GUI.backgroundColor;
		GUI.backgroundColor = Color.clear;
		bool pressed = GUI.Button(
			new Rect(pos.x, pos.y, (float)Width(size), (float)Height(size)),
			new GUIContent("", name)
		);
		GUI.backgroundColor = tmp;

		//Button zeichnen
		if(Enabled && GUI.tooltip == name){ //Mouseover
			//ausgrauen
			Color old = GUI.color;
			GUI.color = new Color(old.r * 0.5f, old.g * 0.5f, old.b * 0.5f);
			//Rand zeichnen
			gBorder.Enabled = true;
			//Zeichnen
			gFill.Draw(size, pos);
			//Rand zukünftig nicht zeichnen
			gBorder.Enabled = false;
			//Ausgrauen deaktivieren
			GUI.color = old;
		} else if(Enabled){ //Aktiviert
			gFill.Draw(size, pos);
		} else { //Deaktiviert
			//Transparent zeichnen
			Color old = GUI.color;
			GUI.color = new Color(old.r, old.g, old.b, 0.5f);
			gFill.Draw(size, pos);
			GUI.color = old;
		}

		//Aktion ausführen beim Click
		if(Enabled && pressed && action != null){
		//	Enabled = false;
			action(this);
		}
	}

	public double Width(double size){ return gFill.Width(size); }

	public double Height(double size){ return gFill.Height(size); }

}
