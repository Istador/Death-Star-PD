using UnityEngine;

public interface Glyph {
	/// <summary>
	/// Zeichnet einen Glyphen bei einer bestimmten Position mit einem bestimmten Skalierungsfaktor.
	/// </summary>
	/// <param name="size">Skalierungsfaktor.</param>
	/// <param name="pos">Position.</param>
	void Draw(double size, Vector2 pos);

	/// <summary>
	/// Breite des Glyphens, bei dem übergebenen Skalierungsfaktor.
	/// </summary>
	/// <param name="size">Skalierungsfaktor.</param>
	double Width(double size);

	/// <summary>
	/// Höhe des Glyphens, bei dem übergebenen Skalierungsfaktor.
	/// </summary>
	/// <param name="size">Skalierungsfaktor.</param>
	double Height(double size);
}

public class GEmpty : Glyph {
	public void Draw(double size, Vector2 pos){}
	public double Width(double size){return 0.0;}
	public double Height(double size){return 0.0;}

	/*
	* Singleton
	*/
	private static GEmpty instance;
	private GEmpty(){}
	public static GEmpty Instance{get{
			if(instance==null) instance = new GEmpty();
			return instance;
		}}
	public static GEmpty I{get{return Instance;}}
}