using UnityEngine;
using System.Collections;

public class HandCollider : MonoBehaviour {

	[SerializeField]
	private GameObject _gameOver;

	void OnTriggerEnter(Collider other) 
	{
		StartCoroutine(Death());
	}

	IEnumerator Death()
	{
		GameObject g = (GameObject) Instantiate(_gameOver, transform.position - new Vector3(-3.5f, 0.5f, 0f) , Quaternion.identity);
		yield return new WaitForSeconds(0.5f);
		g.GetComponent<SpriteFade>().StartFading(1);
		yield return new WaitForSeconds(3f);
		Application.LoadLevel (0);

	}
}
