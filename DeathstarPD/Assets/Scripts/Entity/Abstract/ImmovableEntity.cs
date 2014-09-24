using UnityEngine;
using System.Collections;

/// 
/// Abstrakte Oberklasse für unbewegliche Gegner
/// 
public abstract class ImmovableEntity : Entity {
	
	

	/// <summary>
	/// Initializes a new instance of the <see cref="ImmovableEntity`1"/> class.
	/// </summary>
	/// <param name='maxHealth'>
	/// Maximale Trefferpunkte des Gegners. Bei 0 HP stirbt der Gegner.
	/// </param>
	public ImmovableEntity(int maxHealth) : base(maxHealth){
		
	}
	
	
	
}
