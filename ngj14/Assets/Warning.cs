using UnityEngine;
using System.Collections;

public class Warning : MonoBehaviour 
{
	public float exclamationMarkWaitTime = 1f;
	public const string EXCLAMATION_MARK = "exclamationmark";

	void Start () 
	{
		
	}

	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.GetComponent<Enemy>())
		{
			SpriteRenderer sprite = col.transform.FindChild(EXCLAMATION_MARK).GetComponent<SpriteRenderer>();
			sprite.enabled = false;
			StartCoroutine(SpawnMark(sprite));
		}
	}

	IEnumerator SpawnMark(SpriteRenderer sprite)
	{
		sprite.enabled = true;
		yield return new WaitForSeconds(exclamationMarkWaitTime);
		sprite.enabled = false;
	}
}
