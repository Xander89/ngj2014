using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour 
{
	[SerializeField]
	private SfxManager sfx_manager;

	[SerializeField]
	private GameObject[] _enemies;

	[SerializeField]
	private string[] _enemySounds;

	//[SerializeField]
	//private GameObject _evilSpriteContainer;

	public AnimationCurve difficultyCurve;
	public AnimationCurve enemyMultiplicityChanceCurve;

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
			for(int i = 0; i < GetMultiplicity(); i++) 
			{
				int index = Random.Range(0, _enemies.Length);
				int sound_index = Random.Range(0, _enemySounds.Length);
				Vector2 direction = GetRandomVector();
				Vector3 spawnDelta = new Vector3(direction.x, direction.y, 0.0f) * spawnRadius;
				Vector3 minDelta = new Vector3(direction.x, direction.y, 0.0f) * minRadius;
				Vector3 s = new Vector3(0.0f,0.0f,1.0f);
				GameObject g = (GameObject) Instantiate(_enemies[index], transform.position + spawnDelta, Quaternion.LookRotation(s,-direction));
				g.transform.parent = transform;
				Enemy e = g.GetComponent<Enemy>();
				e.StartMovement(transform.position + minDelta);
				Debug.Log ("_enemySounds.Length " + _enemySounds.Length);
				Debug.Log ("STRING " +  _enemySounds[sound_index].ToString());
				sfx_manager.PlaySfx(_enemySounds[sound_index].ToString());
			}
			/*Debug.Log("evil sprites length.. " + _evilSprites.Length);
			SpriteRenderer randomSprite = _evilSprites[Random.Range(0, _evilSprites.Length)];
			((SpriteRenderer)enemy.renderer).sprite = randomSprite.sprite;
			enemy.renderer.material = randomSprite.material;*/


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

	private int GetMultiplicity()
	{
		float n = Random.Range(0.0f,1.0f);
		float m = enemyMultiplicityChanceCurve.Evaluate(Mathf.Min(timeSinceBeginning / timeToMaximumDifficulty,1.0f)); 
		if(n < m)
		{
			return 2;
		}
		return 1;
	}


/*	IEnumerator Spawn()
	{

	}*/
}
