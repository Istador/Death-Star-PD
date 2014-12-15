using UnityEngine;
using System.Collections;

public abstract class Tower : ImmovableEntity {

	/// <summary>
	/// wie oft geupdatet werden soll.
	/// default: 7 (jeden 7. Frame)
	/// </summary>
	public static readonly int update_each_x_frames = 7;
	/// <summary>
	/// Dieser spezifische Tower updatet bei Frame Nr. x
	/// </summary>
	private readonly int update_on_frame = Utility.Rnd.Next(update_each_x_frames);
	/// <summary>
	/// Die aktuelle Framenummer
	/// </summary>
	private static int frame_nr = 0;
	/// <summary>
	/// Wann der letzte Frame war. (um zu erkennen wann ein neuer Frame ist)
	/// </summary>
	private static float last_frame_time = 0f;



	protected override void Start() {
		//Setze Level auf 1 (berechnet Werte)
		Level = 1;
		isAttackCooldownActive = false;
		Buffs = 0;

		//Start in Superklasse
		base.Start();

		//Füge zur Towers-Collection hinzu
		Towers.I.Add(this);
	}



	protected override void Update() {
		//nur berechnen der neuen Framenummer
		if(last_frame_time < Time.time){
			frame_nr = (frame_nr + 1) % update_each_x_frames;
			last_frame_time = Time.time;
		}

		//nur alle x Frames
		if(frame_nr == update_on_frame){

			//Target lebt nicht mehr?
			if(AttackPattern == EAttackPattern.SingleTarget && (Target == null || !Target.enabled || Target.IsDead)){
				Target = null;

				//finde neues Target
				Collider[] hits = Physics.OverlapSphere(Pos, Range, (int)Layer.EnemyFighter);
				if(hits.Length > 0){

					//neues Target das am nächsten ist finden
					float nearestdistance = float.PositiveInfinity;
					foreach(Collider hit in hits){
						MovableEntity current = hit.gameObject.GetComponent<MovableEntity>();
						//ob es ein valides Ziel
						if(current == null || !current.enabled || current.IsDead) continue;
						//Enfernung berechnen (im Squared-Distance-Space)
						float distance = DistanceSqTo(current);
						//neues nahestes Ziel
						if(Target == null || distance < nearestdistance){
							nearestdistance = distance;
							Target = current;
						}
					}

					//Wenn der Cooldown noch nicht läuft
					if(Target != null && !isAttackCooldownActive){
						//Greife das Ziel jetzt an
						DoAttack(Target);
						//Nachricht an selbst für später
						MessageDispatcher.I.Dispatch(this, "AttackCooldownReached", AttackCooldown);
						//merke das der Cooldown läuft
						isAttackCooldownActive = true;
					}
				}
			}
			else if(AttackPattern == EAttackPattern.MultiTarget && !isAttackCooldownActive){
				AttackAllInRange();
			}
		}

		base.Update();
	}



	public override bool HandleMessage(Telegram msg){
		if(this == null || IsDead || !enabled) return true;

		switch(msg.message){
		case "LevelUp":
			LevelUp();
			return true;
		case "AttackCooldownReached":
			//Ziel existiert noch und ist in Reichweite 
			if(Target != null && Target.enabled && !Target.IsDead && DistanceTo(Target) <= Range){
				//Greife das Ziel jetzt an
				DoAttack(Target);
				//Nachricht an selbst für später
				MessageDispatcher.I.Dispatch(this, "AttackCooldownReached", AttackCooldown);
			}
			//Ziel existiert nicht mehr oder ist nicht mehr in Reichweite
			else {
				Target = null;
				isAttackCooldownActive = false;
			}
			return true;
		case "MultiTargetCooldownReached":
			AttackAllInRange();
			return true;
		default:
			return base.HandleMessage(msg);
		}
	}



	public override void Death(){
		//Entferne den Turm von der Towers-Collection
		Towers.I.Remove(this);

		//Death von Superklasse
		base.Death();
	}



	public void Sell(){
		//Entferne den Turm von der Towers-Collection
		Towers.I.Remove(this);

		//Death ohne DeathEffekt
		IsDead = true;
		Destroy(gameObject);
	}



	/// <summary>
	/// Das Level dieses Turmes
	/// </summary>
	public int Level {
		get { return _Level; }
		private set {
			//Setze Instanzvariable
			_Level = value;
			//neuen Wert setzen (+50% pro Buff)
			MaxHealth = Mathf.RoundToInt(((float)MaxHealthTable[System.Math.Max(0, Level - 1)]) * (1f + (float)Buffs * 0.5f));
			//Namen setzen
			name = GetType()+" (Level "+Level+")";
		}
	}
	private int _Level; //Instanzvariable



	/// <summary>
	/// erhöht das Level um 1.
	/// </summary>
	public void LevelUp(){
		Level++;
	}



	/// <summary>
	/// Der Angriffscooldown ist abgelaufen und das Target existiert noch
	/// </summary>
	protected abstract void DoAttack(MovableEntity target);



	private void AttackAllInRange(){
		if(AttackPattern != EAttackPattern.MultiTarget) return;

		//Alle Gegner in Reichweite finden
		Collider[] hits = Physics.OverlapSphere(Pos, Range, (int)Layer.EnemyFighter);
		//bei mindestens einem Gegner
		if(hits.Length > 0){
			//Alle Gegner
			foreach(Collider hit in hits){
				MovableEntity e = hit.gameObject.GetComponent<MovableEntity>();
				//Greife das Ziel an
				if(e != null) DoAttack(e);
			}
			isAttackCooldownActive = true;
			MessageDispatcher.I.Dispatch(this, "MultiTargetCooldownReached", AttackCooldown);
		}
		//bei keinen Gegnern in Reichweite
		else {
			isAttackCooldownActive = false;
		}
	}
	
	
	/// <summary>
	/// maximale Trefferpunkte abhängig vom Level des Turmes
	/// </summary>
	public abstract int[] MaxHealthTable { get; }



	/// <summary>
	/// momentaner Schaden pro Treffer, abhängig vom aktuellem Level und Anzahl Buffs (je +50%).
	/// </summary>
	public int Damage { get{
		return Mathf.RoundToInt(((float)DamageTable[System.Math.Max(0, Level - 1)]) * (1f + (float)Buffs * 0.5f));
	} }
	/// <summary>
	/// verursachter Schaden abhängig vom Level des Turmes
	/// </summary>
	public abstract int[] DamageTable { get; }



	/// <summary>
	/// momentane Angriffsreichweite, abhängig vom aktuellem Level un Anzahl Buffs (je 2.5f)
	/// </summary>
	public float Range { get{
		return RangeTable[System.Math.Max(0, Level - 1)] + ((float)Buffs * 2.5f) ;
	} }
	/// <summary>
	/// Angriffsreichtweite abhängig vom Level des Turmes
	/// </summary>
	public abstract float[] RangeTable { get; }



	/// <summary>
	/// wieviele Kekse es kostet Level 1 zu bauen
	/// </summary>
	public int CookieBuildCost { get{return CookieTable[0];} }
	/// <summary>
	/// wieviele Kekse es kostet upzugraden
	/// </summary>
	public int CookieUpgradeCost { get{return CookieTable[Level];} }
	/// <summary>
	/// Tabelle mit den Kekskosten
	/// </summary>
	public abstract int[] CookieTable { get; }



	/// <summary>
	/// wieviel Geld es kostet Level 1 zu bauen
	/// </summary>
	public int MoneyBuildCost { get{return MoneyTable[0];} }
	/// <summary>
	/// wieviel Geld es kostet upzugraden
	/// </summary>
	public int MoneyUpgradeCost { get{return MoneyTable[Level];} }
	/// <summary>
	/// Tabelle mit den Geldkosten
	/// </summary>
	public abstract int[] MoneyTable { get; }



	/// <summary>
	/// Abklingzeit zwischen Angriffen in Sekunden, abhängig vom aktuellem Level.
	/// </summary>
	public float AttackCooldown { get{return AttackCooldownTable[System.Math.Max(0, Level - 1)];} }
	/// <summary>
	/// Abklingzeit zwischen den Angriffen abhängig vom Level des Turmes
	/// </summary>
	public abstract float[] AttackCooldownTable  { get; }
	/// <summary>
	/// Ob der Cooldown Aktiv ist
	/// </summary>
	private bool isAttackCooldownActive = false;


	public int Buffs {
		get {
			return _buffs;
		}
		set {
			_buffs = System.Math.Max(0, value);
			//neuen Wert setzen (+50% pro Buff)
			MaxHealth = Mathf.RoundToInt(((float)MaxHealthTable[System.Math.Max(0, Level - 1)]) * (1f + (float)Buffs * 0.5f));
		}
	}
	private int _buffs = 0;

	/// <summary>
	/// Angriffsmuster des Turmes
	/// Keine Angriffe, Einzelnes Ziel oder alle Ziele in Reichweite.
	/// </summary>
	public abstract EAttackPattern AttackPattern { get; }
	public enum EAttackPattern { None, SingleTarget, MultiTarget }



	/// <summary>
	/// Das momentan anvisierte Ziel dieses Turmes.
	/// </summary>
	public MovableEntity Target { get; private set; }



}
