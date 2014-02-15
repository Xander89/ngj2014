using UnityEngine;
using System.Collections;

public abstract class GameState : MonoBehaviour {

	private string stateName;

	public abstract string StateName
	{
		get;
	}

	public abstract void EnterState();
	public abstract void StateUpdate();
	public abstract void ExitState();
}
