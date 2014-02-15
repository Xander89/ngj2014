using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour {

	public MoveToPoint firstMoving;
	public MoveToPoint secondMoving;
	public SpriteFade bg;
	public SpriteFade girl;
	public SfxManager manager;
	public AudioSource ambient;
	public AudioClip menu_music;
	public AudioClip game_music;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(2.0f);
		firstMoving.StartCoroutine(firstMoving.StartMovement());
		yield return new WaitForSeconds(2.0f);
		secondMoving.StartCoroutine(secondMoving.StartMovement());
		yield return new WaitForSeconds(2.0f);
		bg.StartFading(0.0f);
		girl.StartFading(1.0f);
		yield return new WaitForSeconds(2.0f);
//		Destroy (bg.gameObject);
		manager.PlaySfx("girl_scream");
		yield return new WaitForSeconds(3.0f);
		ambient.clip = menu_music;
		ambient.Play();
	}

	IEnumerator afterPress()
	{
		ambient.clip = menu_music;
		ambient.Play();
		girl.StartFading(0.0f);
		firstMoving.gameObject.GetComponent<SpriteFade>().StartFading(0.0f);
		secondMoving.gameObject.GetComponent<SpriteFade>().StartFading(0.0f);

		yield return null;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(afterPress());
		}
	}
}
