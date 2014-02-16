using UnityEngine;
using System.Collections;

public class IntroController : MonoBehaviour {

	public MoveToPoint firstMoving;
	public MoveToPoint secondMoving;
	public SpriteFade bg;
	public SpriteFade girl;
	public SpriteFade bgNarrow;
	public SpriteFade peepingEyes;
	public SpriteFade pressSpace;
	public SfxManager manager;
	public AudioSource ambient;
	public AudioClip menu_music;
	public Hand hand;
	public SpriteRenderer hint;
//	public AudioClip game_music;
	private bool ready_To_Start = false;

	// Use this for initialization
	public IEnumerator StartIntro () {
		yield return new WaitForSeconds(2.0f);
		firstMoving.StartCoroutine(firstMoving.StartMovement());
		yield return new WaitForSeconds(2.0f);
		girl.StartFading(0.0f);
		yield return new WaitForSeconds(2.0f);
		secondMoving.StartCoroutine(secondMoving.StartMovement());
		yield return new WaitForSeconds(2.0f);
		bg.StartFading(0.0f);
		peepingEyes.StartFading(1f);
		yield return new WaitForSeconds(2.0f);
//		Destroy (bg.gameObject);
		pressSpace.StartFading(1f);
		manager.PlaySfx("girl_scream");

		yield return new WaitForSeconds(3.0f);
			ambient.clip = menu_music;
		ambient.Play();
		ready_To_Start = true;
	}

	IEnumerator afterPress()
	{
		hand.CanBeKilled = true;
		GameStateManager.SwitchState(new PlayState());
		StartCoroutine(ShowHint());
		girl.StartFading(0.0f);
		pressSpace.StartFading(0f);
		peepingEyes.StartFading(0f);
		bgNarrow.StartFading(0f);
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
		if(ready_To_Start && Input.GetKeyDown(KeyCode.Space))
		{
			ready_To_Start = false;
			StartCoroutine(afterPress());
		}
	}
}
