using UnityEngine;
using System.Collections;

public class GameStateManagerController : MonoBehaviour {

	private static bool _showMenu = true;

	void Start()
	{
		//if (PlayerPrefs.GetInt("showMenu", 1) == 1) {

						GameStateManager.EnterTheFirstState (new MainMenuState ());
						_showMenu = false;
		//	PlayerPrefs.SetInt("showMenu", 0);
		//}
		//else
			//GameStateManager.EnterTheFirstState(new PlayState());
	}
	
	void Update()
	{
		// if (something)
		// GameStateManager.SwitchState(new SomeState());

		// can check active state like this:
		// GameStateManager.ActiveState.StateName.Equals(new SomeState().StateName)
	}
}
