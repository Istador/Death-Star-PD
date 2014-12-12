using UnityEngine;
using System.Collections;

public class ShowRoomControl : MonoBehaviour {

	public GameObject[] towers;

	private int current = 0;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.A)){
			PreviousModel();
		}
		if(Input.GetKeyUp(KeyCode.D)){
			NextModel();
		}
	}

	public void NextModel(){
		current += 1;
		if(current >= towers.Length){
			current = 0;
		}
		SetModel(current);
	}

	public void PreviousModel(){
		current -= 1;
		if(current < 0){
			current = towers.Length - 1;
		}
		SetModel(current);
	}

	public void SetModel(int id){
		for(int i = 0; i < towers.Length; i++){
			if(i == id){
				towers[i].SetActive(true);
			}else{
				towers[i].SetActive(false);
			}
		}
	}

	public void BackToMainMenu(){
		Game.LoadLevel(0);
	}
}
