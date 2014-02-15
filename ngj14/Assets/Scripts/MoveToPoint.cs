using UnityEngine;
using System.Collections;

public class MoveToPoint : MonoBehaviour {

	public Vector3 offset;
	public float speed;
	public string soundToPlay;
	public float soundDelay;

	private SfxManager manager;
	private Vector3 dest;
	void Awake()
	{
		manager = (SfxManager)FindObjectOfType(typeof(SfxManager));
	}

	// Use this for initialization
	public IEnumerator StartMovement () {
		dest = transform.position + offset;
		StartCoroutine(soundTimer());
		while(true)
		{
			transform.position = Vector3.Lerp(transform.position,dest,Time.deltaTime * speed);
			yield return null;
		}
	}

	IEnumerator soundTimer()
	{
		yield return new WaitForSeconds(soundDelay);
		manager.PlaySfx(soundToPlay);
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(offset + transform.position,1.0f);
	}

}
