using UnityEngine;
using System.Collections;

public class PlayState : GameState {

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
		Debug.Log("Enter Play State");
		_enemies = GameObject.Find ("Enemies");
		EnemySpawn es = _enemies.GetComponent<EnemySpawn> ();
		es.StartCoroutine(es.Engage());
		SfxManager sfx = (SfxManager)FindObjectOfType(typeof(SfxManager));
		sfx.PlayAmbientGame();
		Score._elapsedTime = 0;
		_score = GameObject.Find ("Score");
		_score.GetComponent<Score> ().enabled = true;
		_score.GetComponent<MeshRenderer> ().enabled = true;
		//_score.GetComponent<GUIText> ().enabled = true;
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
