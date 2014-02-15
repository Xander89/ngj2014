using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	[SerializeField]
	private GameObject[] _enemies;

	//[SerializeField]
	//private GameObject _evilSpriteContainer;

	public AnimationCurve difficultyCurve;

	public float spawnRadius = 10;
	public float minRadius = 1;
	public float initTime;

	public float averageInterSpawnTime;
	private float currentAverageTime;

	public float timeToMaximumDifficulty = 30.0f;

	private float timeSinceBeginning = 0.0f;

	private SpriteRenderer[] _evilSprites;



	void Awake()
	{
		//_evilSprites = _evilSpriteContainer.GetComponentsInChildren<SpriteRenderer> ();
	}

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

			int index = Random.Range(0, _enemies.Length);

			GameObject g = (GameObject) Instantiate(_enemies[index], transform.position + spawnDelta, Quaternion.identity);
			/*Debug.Log("evil sprites length.. " + _evilSprites.Length);
			SpriteRenderer randomSprite = _evilSprites[Random.Range(0, _evilSprites.Length)];
			((SpriteRenderer)enemy.renderer).sprite = randomSprite.sprite;
			enemy.renderer.material = randomSprite.material;*/

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
