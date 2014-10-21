using UnityEngine;
using System;

/// <summary>
/// Aktionsbutton, der Rohstoffe kostet (auskommentiert), eine Effektdauer & Cooldown besitzt und per Tastatur aktiviert werden kann.
/// </summary>
public class GActionButton : Glyph, MessageReceiver {

	//visueller Button
	private GButton but;
	public void Draw(double size, Vector2 pos){ but.Draw(size, pos); }
	public double Width(double size){ return but.Width(size); }
	public double Height(double size){ return but.Height(size); }

	//ob der Cooldown abgelaufen ist
	private bool cooldownReady = true;

	//ob genug Gehirne zur Verfügung stehen, um die Aktion auszuführen
	/*
	private bool enoughBrains = false;
	*/

	//Setzen ob dieser Button aktiv ist oder nicht
	private bool _enabled = true;
	public bool Enabled {
		get{return _enabled;}
		set{
			_enabled = value;
			but.Enabled = Enabled && cooldownReady /* && enoughBrains */;
		}
	}

	//Kosten um die Aktion auszuführen
	/*
	private int cost;
	*/

	/// <summary>
	/// Aktion die beim Click oder Tastendruck ausgeführt werden soll
	/// true: Wurde gerade gedrückt
	/// false: die effect-Duration ist abgelaufen
	/// </summary>
	private Action<bool> action;


	//Konstruktor
	public GActionButton(string buttonName, string icon, Action<bool> action = null, /* int cost = 0,*/ float effectDuration = 0.0f, float cooldown = 0.0f){
		/* this.cost = cost; */
		this.action = action;

		Action<GButton> a = (GButton b) => {
			but.Enabled = false;
			cooldownReady = false;

			//wenn kosten existieren, verringere Gehirnanzahl
			/* 
			if(this.cost > 0)
				PlayerObject.I.Brains -= this.cost;
			*/

			//Aktion ausführen
			if(action != null) action(true); //beginn
			
			//Nachricht an selbst, wann der Effekt abgelaufen ist
			MessageDispatcher.I.Dispatch(this, this, "end of effect", effectDuration);
			//Nachricht an selbst, wann der Cooldown abgelaufen ist
			MessageDispatcher.I.Dispatch(this, this, "cooldown ready", cooldown);
		};

		//visueller Button
		but = new GButton(200, 200, new GImage(Resource.Texture[icon]), a);
		but.Enabled = false;
		but.Filled = false;

		//über Ressourcenänderungen informiert werden
		/*
		object brains = Observer.I.Add("Brains", this);
		if(brains != null)
			ObserveUpdate("Brains", brains);
		*/

		//Keyevent registrieren
		Inputs.I.Register(buttonName, (down) => {
			if(down && Enabled && cooldownReady /* && enoughBrains */)
				a(but);
		});
	}



	//Destruktor
	~GActionButton(){
		/*
		Observer.I.Remove("Brains", this);
		*/
	}



	public bool HandleMessage(Telegram msg){
		switch(msg.message){
		case "end of effect":
			//Aktion ausführen
			if(action != null) action(false); //ende
			return true;
		case "cooldown ready":
			cooldownReady = true;
			but.Enabled = Enabled /* && enoughBrains */;
			return true;
		/*
		case "Brains":
			enoughBrains = (cost <= 0 || ((int)brains) >= cost);
			but.Enabled = Enabled && enoughBrains && cooldownReady;
		*/
		default:
			return false;
		}
	}

	

}
