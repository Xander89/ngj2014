using UnityEngine;
using System.Collections;

public class PlayState : GameState {

	private GameObject _enemies;

	public override string StateName
	{
		get
		{
			return this.GetType().ToString();
		}
	}
	
	public override void EnterState()
	{
		Debug.Log("Enter Play State");
		_enemies = GameObject.Find ("Enemies");
		EnemySpawn es = _enemies.GetComponent<EnemySpawn> ();
		es.StartCoroutine(es.Engage());

	}
	
	public override void StateUpdate()
	{
		// do stuff here
	}
	public override void ExitState()
	{

		Debug.Log("Exit Play State");
	}
}
