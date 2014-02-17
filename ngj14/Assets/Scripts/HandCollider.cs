using UnityEngine;
using System.Collections;

public class HandCollider : MonoBehaviour {

	[SerializeField]
	private GameObject _gameOver;

	private GameObject _score;
	private SfxManager manager;

	private bool end = false;
	private bool died = false;

	private Vector3 nonORPos   = new Vector3 (0, 0.0f, -16.5f);
	private Vector3 nonORScale = new Vector3 (3.2f, 1.3f, 1f);
	private Vector3 ORPos      = new Vector3 (0, 1.0f, -16.5f);
	private Vector3 ORScale    = new Vector3 (1f, 0.8f, 1f);

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
			//_score.GetComponent<GUIText> ().text = "Score: " + Score._score;
			_score.GetComponent<TextMesh>().text = "Score: " + Score._score;
			_score.GetComponent<Score> ().enabled = false;

			GameObject g;

			if(MovePlayer.riftEnabled)
			{
				g = (GameObject) Instantiate(_gameOver, ORPos  , Quaternion.identity);
				g.transform.localScale = ORScale;
				_score.transform.position = new Vector3(_score.transform.position.x,
				                                        _score.transform.position.y - 2.5f,
				                                        _score.transform.position.z);

			}
			else
			{
				g = (GameObject) Instantiate(_gameOver, nonORPos  , Quaternion.identity);
				g.transform.localScale = nonORScale;
			}
			//g.transform.localScale = new Vector3();
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
