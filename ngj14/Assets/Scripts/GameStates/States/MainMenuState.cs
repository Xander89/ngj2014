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
		Debug.Log("Enter Main Menu State");
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
