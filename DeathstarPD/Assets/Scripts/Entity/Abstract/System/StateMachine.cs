/*
 * Zustandsautomat
 * 
 * Funktionalität:
 * - globaler Zustand
 * - aktueller Zustand
 * - voriger Zustand
 * - Zustandsübergänge
 * - Nachrichtensystem
 * 
 * Quelle:
 * Mat Buckland - Programming Game AI by Example
*/
public class StateMachine<T> : MessageReceiver {

	
	
	/// <summary>
	/// Besitzer dieses Zustandsautomatens
	/// </summary>
	private T owner;
	
	/// <summary>
	/// Der globale Zustand des Automatens
	/// </summary>
	public State<T> GlobalState {get; set;}

	/// <summary>
	/// Der aktuelle Zustand des Automatens
	/// </summary>
	public State<T> CurrentState {get; set;}
	
	/// <summary>
	/// Der vorherige Zustand (vor dem aktuellen Zustand)
	/// </summary>
	public State<T> PreviousState {get; set;}
	
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="StateMachine`1"/> class.
	/// </summary>
	/// <param name='owner'>
	/// Besitzer dieses Zustandsautomatens
	/// </param>
	public StateMachine(T owner){
		this.owner = owner;
	}
	
	
	
	/// <summary>
	/// Ändert den aktuellen Zustand des Automatens.
	/// ruft Exit() des alten und Enter() des neuen Zustands auf.
	/// </summary>
	/// <param name='state'>
	/// der neue Zustand zu dem gewechselt werden soll.
	/// </param>
	public void ChangeState<X>(State<X> state) where X: T {
		//wenn ein voriger Zustand existiert verlasse ihn
		if(CurrentState!=null) CurrentState.Exit(owner);
		
		//merke aktuellen Zustand als alten Zustand
		PreviousState = CurrentState;
		//ersetze aktuellen Zustand durch den neuen Zustand
		CurrentState = state;
		
		//betrete den neuen Zustand
		if(CurrentState!=null) CurrentState.Enter(owner);
	}
	
	
	
	/// <summary>
	/// Ändert den globalen Zustand des Automatens.
	/// ruft Exit() des alten und Enter() des neuen Zustands auf.
	/// </summary>
	/// <param name='state'>
	/// der neue Zustand zu dem gewechselt werden soll.
	/// </param>
	public void ChangeGlobalState<X>(State<T> state) where X: T {
		//wenn ein voriger Zustand existiert verlasse ihn
		if(GlobalState!=null) GlobalState.Exit(owner);
		//ersetze aktuellen Zustand durch den neuen Zustand
		GlobalState = state;
		//betrete den neuen Zustand
		if(GlobalState!=null) GlobalState.Enter(owner);
	}
	
	
	
	/// <summary>
	/// Kehrt zum vorigem Zustand zurück.
	/// ruft Exit() des aktuellen und Enter() des vorigen Zustandes auf.
	/// </summary>
	public void RevertToPreviousState(){
		ChangeState(PreviousState);
	}
	
	
	
	/// <summary>
	/// Methode um die Zustandsmaschine zu "starten".
	/// Ruft die Enter-Methode des aktuellen Zustandes auf.
	/// </summary>
	public void Start(){
		if(GlobalState!=null) GlobalState.Enter(owner);
		if(CurrentState!=null) CurrentState.Enter(owner);
	}
	
	
	
	/// <summary>
	/// Update Methode die bei jedem Frame aufgerufen wird.
	/// Deligiert an die jeweiligen Zustände.
	/// </summary>
	public void Update(){
		//zuerst der globale Zustand
		if(GlobalState!=null) GlobalState.Execute(owner);
		//und dann der normale
		if(CurrentState!=null) CurrentState.Execute(owner);
	}
	
	
	
	/// <summary>
	/// Ist der Zustandsautomat in dem angefragten Zustand?
	/// </summary>
	/// <returns>
	/// ob der Zustandsautomat in den angefragten Zustand ist.
	/// </returns>
	/// <param name='state'>
	/// Der Zustand der überprüft werden soll.
	/// </param>
	public bool IsInState(State<T> state){
		return CurrentState == state || (CurrentState!=null && CurrentState.Equals(state));
	}
	
	
	
	/// <summary>
	/// Nachrichten an den aktuellen Zustand weitergeben.
	/// Gibt die Nachricht an den globalen Zustand, wenn der aktuelle Zustand
	/// die Nachricht nicht verarbeiten kann.
	/// </summary>
	/// <returns>
	/// ob die Nachricht verarbeitete werden konnte.
	/// </returns>
	/// <param name='msg'>
	/// Die Nachricht die verarbeitet werden soll.
	/// </param>
	public bool HandleMessage(Telegram msg){
		if(owner == null) return true;
		//wenn der aktuelle Zustand die Nachricht verarbeiten kann
		if(CurrentState!=null && CurrentState.OnMessage(owner, msg))
			return true;
		//falls aktueller zustand nicht kann, dann globaler
		else if(GlobalState!=null && GlobalState.OnMessage(owner, msg))
			return true;
		//Nachricht unverarbeitet
		return false;
	}
	
	
	
}
