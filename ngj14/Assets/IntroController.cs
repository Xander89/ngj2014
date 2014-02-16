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
	public Hand hand;
	public SpriteRenderer hint;
//	public AudioClip game_music;

	// Use this for initialization
	public IEnumerator StartIntro () {
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
		hand.CanBeKilled = true;
		GameStateManager.SwitchState(new PlayState());
		StartCoroutine(ShowHint());
		girl.StartFading(0.0f);
		firstMoving.gameObject.GetComponent<SpriteFade>().StartFading(0.0f);
		secondMoving.gameObject.GetComponent<SpriteFade>().StartFading(0.0f);

		yield return null;

	}

	IEnumerator ShowHint()
	{
		SpriteFade fade = hint.GetComponent<SpriteFade>();
		fade.StartFading(1f);
		yield return new WaitForSeconds(3f);
		fade.StartFading(0f);

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
		{

			StartCoroutine(afterPress());
		}
	}
}
