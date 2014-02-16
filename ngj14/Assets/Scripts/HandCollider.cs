using UnityEngine;
using System.Collections;

public class HandCollider : MonoBehaviour {

	[SerializeField]
	private GameObject _gameOver;

	private GameObject _score;
	private SfxManager manager;

	private bool end = false;
	private bool died = false;

	void Start()
	{
		manager = (SfxManager)FindObjectOfType(typeof(SfxManager));
		_score = GameObject.Find ("Score");
	}

	void OnTriggerEnter(Collider other) 
	{
		if(other != null && other.transform != null && other.transform.GetComponent<Enemy>() != null)
		{


			StartCoroutine(Death());
		}
	}

	IEnumerator Death()
	{	
		if(died) 
		{
			yield return null;
		} 
		else
		{
			died = true;
			_score.GetComponent<Score> ().enabled = false;
			_score.GetComponent<GUIText> ().enabled = false;
			GameObject g = (GameObject) Instantiate(_gameOver, transform.position  , Quaternion.identity);
			yield return new WaitForSeconds(0.5f);
			manager.PlaySfx("GameOver");

			g.GetComponent<SpriteFade>().StartFading(1);
			yield return new WaitForSeconds(3f);
			//GameStateManager.SwitchState(new DeathState());
			end = true;
		}
	}

	void Update()
	{
		if(end && Input.GetKeyDown(KeyCode.Space))
		{
			PlayerPrefs.SetInt("showMenu", 0);
			Application.LoadLevel (0);
			end = false;
		}
	}
}
