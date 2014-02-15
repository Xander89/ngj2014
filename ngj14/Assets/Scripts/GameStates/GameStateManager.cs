using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {

	private static GameState _activeState;
	public static GameState ActiveState
	{
		get
		{
			return _activeState;
		}
	}
	
	void Update()
	{
		if (_activeState != null)
		{
			_activeState.StateUpdate();
		}
	}
	
	public static void SwitchState(GameState newState)
	{
		_activeState.ExitState();
		_activeState = newState;
		_activeState.EnterState();
	}
	
	public static void EnterTheFirstState(GameState firstState)
	{
		_activeState = firstState;
		_activeState.EnterState();
	}
	
	void Init()
	{
		
	}
	
}
