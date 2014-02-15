using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	Vector3 source;
	Vector3 sink;
	public float duration = 5.0f;
	public float exclamationDuration = 1.0f;

	void Start()
	{
		source = transform.position;
	}

	void Update()
	{

	}

	public void StartMovement(Vector3 destination)
	{
		sink = destination;
		StartCoroutine(Move());
	}

	IEnumerator Move()
	{
		float time = 0;

		//Move forward
		while (time < duration)
		{
			transform.position = Vector3.Lerp(transform.position, sink, time / duration / 10f);
			time += Time.deltaTime;
			yield return null;
		}

		//Reset
		time = 0;

		//Move backward
		while (time < duration)
		{
			transform.position = Vector3.Lerp(transform.position, source, time / duration  / 10f);
			time += Time.deltaTime;
			yield return null;
		}

		GameObject.Destroy(this.gameObject);
		yield return null;

	}
}
