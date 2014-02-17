using UnityEngine;
using System.Collections;

public class GameStateManagerController : MonoBehaviour {
	
	public bool clear = false;
	void Start()
	{
		if(clear)
		{
			PlayerPrefs.DeleteAll();
		}
		if (PlayerPrefs.GetInt("showMenu", 1) == 1) {

			GameStateManager.EnterTheFirstState (new MainMenuState ());
		} 
		else
		{
			GameStateManager.EnterTheFirstState (new PlayState ());

		}
			//	PlayerPrefs.SetInt("showMenu", 0);
		//}
		//else
			//GameStateManager.EnterTheFirstState(new PlayState());
	}

	void OnApplicationQuit(){
		
		PlayerPrefs.DeleteAll();
	}
	
	void Update()
	{
		// if (something)
		// GameStateManager.SwitchState(new SomeState());

		// can check active state like this:
		// GameStateManager.ActiveState.StateName.Equals(new SomeState().StateName)
	}
}
