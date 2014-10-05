using UnityEngine;
using System.Collections;

public abstract class Tower : ImmovableEntity {

	/// <summary>
	/// wie oft geupdatet werden soll.
	/// default: 3 (jeden 3. Frame)
	/// </summary>
	public static readonly int update_each_x_frames = 3;
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

		//Start in Superklasse
		base.Start();

		//Füge zur Towers-Collection hinzu
		Towers.I.Add(this);
	}



	protected override void Update() {
		if(last_frame_time < Time.time){
			frame_nr = (frame_nr + 1) % update_each_x_frames;
			last_frame_time = Time.time;
		}

		//nur alle x Frames
		if(frame_nr == update_on_frame){

			//Target lebt nicht mehr?
			if(Target == null || !Target.enabled || Target.IsDead){
				Target = null;

				//finde neues Target
				RaycastHit hit;
				//TODO: testen ob der SphereCast funktioniert :)
				if(Physics.SphereCast(Pos, Range, Vector3.zero, out hit, 0f, (int)Layer.EnemyFighter)){
					//neues Target setzen
					Target = hit.collider.gameObject.GetComponent<MovableEntity>();

					//Wenn der Cooldown noch nicht läuft
					if(!isAttackCooldownActive){
						//Greife das Ziel jetzt an
						DoAttack();
						//Nachricht an selbst für später
						MessageDispatcher.I.Dispatch(this, "AttackCooldownReached", AttackCooldown);
						//merke das der Cooldown läuft
						isAttackCooldownActive = true;
					}
				}
			}
		}

		base.Update();
	}



	public override bool HandleMessage(Telegram msg){
		switch(msg.message){
		case "LevelUp":
			LevelUp();
			return true;
		case "AttackCooldownReached":
			//Ziel existiert noch und ist in Reichweite 
			if(Target != null && Target.enabled && !Target.IsDead && DistanceTo(Target) <= Range){
				//Greife das Ziel jetzt an
				DoAttack();
				//Nachricht an selbst für später
				MessageDispatcher.I.Dispatch(this, "AttackCooldownReached", AttackCooldown);
			}
			//Ziel existiert nicht mehr oder ist nicht mehr in Reichweite
			else {
				Target = null;
				isAttackCooldownActive = false;
			}
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



	/// <summary>
	/// Das Level dieses Turmes
	/// </summary>
	public int Level {
		get { return _Level; }
		private set {
			//Setze Instanzvariable
			_Level = value;
			//Berechne Werte neu
			MaxHealth = CalcMaxHealth(Level);
			Damage = CalcDamage(Level);
			Range = CalcRange(Level);
			AttackCooldown = CalcAttackCooldown(Level);
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
	/// Der Angriffscooldown ist abgelaufen und das Target existiert nocht
	/// </summary>
	protected abstract void DoAttack();



	/// <summary>
	/// Berechnet die macimalen Trefferpunkte abhängig vom Level des Turmes
	/// </summary>
	public abstract int CalcMaxHealth(int level);



	/// <summary>
	/// momentaner Schaden pro Treffer
	/// </summary>
	public int Damage { get; private set; }
	/// <summary>
	/// Berechnet den verursachten Schaden abhängig vom Level des Turmes
	/// </summary>
	public abstract int CalcDamage(int level);



	/// <summary>
	/// momentane Angriffsreichweite
	/// </summary>
	public float Range { get; private set; }
	/// <summary>
	/// Berechnet die Angriffsreichtweite abhängig vom Level des Turmes
	/// </summary>
	public abstract float CalcRange(int level);



	/// <summary>
	/// Abklingzeit zwischen Angriffen in Sekunden
	/// </summary>
	public float AttackCooldown { get; private set; }
	/// <summary>
	/// Berechnet die Abklingzeit zwischen den Angriffen abhängig vom Level des Turmes
	/// </summary>
	public abstract float CalcAttackCooldown(int level);
	/// <summary>
	/// Ob der Cooldown Aktiv ist
	/// </summary>
	private bool isAttackCooldownActive = false;



	/// <summary>
	/// Das momentan anvisierte Ziel dieses Turmes
	/// </summary>
	public MovableEntity Target { get; private set; }



}
