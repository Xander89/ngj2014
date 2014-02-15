using UnityEngine;
using System.Collections;

public class GameStateManagerController : MonoBehaviour {

	void Start()
	{
	
		GameStateManager.EnterTheFirstState(new MainMenuState());

	}
	
	void Update()
	{
		// if (something)
		// GameStateManager.SwitchState(new SomeState());

		// can check active state like this:
		// GameStateManager.ActiveState.StateName.Equals(new SomeState().StateName)
	}
}
