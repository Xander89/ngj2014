using UnityEngine;
using System.Collections;

public class MainMenuState : GameState {

	private GameObject _score;

	public override string StateName
	{
		get
		{
			Debug.Log ("asdsad" + this.GetType().ToString());
			return this.GetType().ToString();
		}
	}
	
	public override void EnterState()
	{
		IntroController c = ((GameStateManager)FindObjectOfType(typeof(GameStateManager))).intro.GetComponent<IntroController>();
		c.gameObject.SetActive(true);
		c.StartCoroutine(c.StartIntro());
		Debug.Log("Enter Main Menu State");
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
		Debug.Log("Exit Main Menu State");
	}
}
