using UnityEngine;
using System.Collections;

public class MainMenuState : GameState {

	private GameObject _enemies;
	private GameObject _score;

	public override string StateName
	{
		get
		{
			return this.GetType().ToString();
		}
	}
	
	public override void EnterState()
	{
		Debug.Log("Enter Main Menu State");
		_enemies = GameObject.Find ("Enemies");
		_score = GameObject.Find ("Score");
		_score.GetComponent<Score> ().enabled = false;
		_score.GetComponent<GUIText> ().enabled = false;
		//_enemies.SetActive(false);
	}
	
	public override void StateUpdate()
	{
		// do stuff here
	}
	public override void ExitState()
	{
		//_enemies.SetActive(true);
		Debug.Log ("enemy spawner.. " + _enemies.GetComponent<EnemySpawn>());

		Debug.Log("Exit Main Menu State");
	}
}
