using UnityEngine;
using System.Collections;

public class MainMenuState : GameState {

	public override string StateName
	{
		get
		{
			return this.GetType().ToString();
		}
	}
	
	public override void EnterState()
	{
		IntroController c = ((GameStateManager)FindObjectOfType(typeof(GameStateManager))).intro.GetComponent<IntroController>();
		c.gameObject.SetActive(true);
		c.StartCoroutine(c.StartIntro());
		//_enemies.SetActive(false);
	}
	
	public override void StateUpdate()
	{
		// do stuff here
	}
	public override void ExitState()
	{
		//_enemies.SetActive(true);
	}
}
