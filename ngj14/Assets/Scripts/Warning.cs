using UnityEngine;
using System.Collections;

public class Warning : MonoBehaviour 
{
	public float exclamationMarkWaitTime = 1f;
	public const string EXCLAMATION_MARK = "exclamationmark";

	public float borderX = -80f;
	public float borderY = -160f;

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
		sprite.transform.parent = null;
		float positionZ = sprite.transform.position.z;
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(sprite.transform.position);
		screenPosition = new Vector3(Mathf.Clamp(screenPosition.x, 0 + borderX, Screen.width - borderX), Mathf.Clamp (screenPosition.y, 0 + borderY, Screen.height - borderY), screenPosition.z);
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
		worldPosition = new Vector3(worldPosition.x, worldPosition.y, positionZ);
		sprite.transform.position = worldPosition;
		sprite.enabled = true;
		yield return new WaitForSeconds(exclamationMarkWaitTime);
		sprite.enabled = false;
	}
}
