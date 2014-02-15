using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	[SerializeField]
	GameObject enemy;

	public AnimationCurve difficultyCurve;

	public float spawnRadius = 10;
	public float minRadius = 1;
	public float initTime;

	public float averageInterSpawnTime;
	private float currentAverageTime;

	public float timeToMaximumDifficulty = 30.0f;

	private float timeSinceBeginning = 0.0f;

	IEnumerator Start()
	{

		yield return new WaitForSeconds(initTime);
		timeSinceBeginning = 0.0f;
		StartCoroutine(updateTime());
		//Spawn

		currentAverageTime = averageInterSpawnTime;
		while(true)
		{
			Vector2 direction = GetRandomVector();
			Vector3 spawnDelta = new Vector3(direction.x, direction.y, 0.0f) * spawnRadius;
			Vector3 minDelta = new Vector3(direction.x, direction.y, 0.0f) * minRadius;

			GameObject g = (GameObject) Instantiate(enemy, transform.position + spawnDelta, Quaternion.identity);
			g.transform.parent = transform;
			Enemy e = g.GetComponent<Enemy>();
			e.StartMovement(transform.position + minDelta);

			yield return new WaitForSeconds(currentAverageTime);
			currentAverageTime = averageInterSpawnTime*difficultyCurve.Evaluate(Mathf.Min(timeSinceBeginning / timeToMaximumDifficulty,1.0f)); 
		}
	}

	IEnumerator updateTime()
	{
		while(true)
		{
			timeSinceBeginning += Time.deltaTime;
			yield return null;
		}
	}

	void Update()
	{

	}

	Vector2 GetRandomVector()
	{
		Vector2 vec  = new Vector2(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f));
		vec = vec.normalized;
		return vec;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position,1.0f);
	}

/*	IEnumerator Spawn()
	{

	}*/
}
