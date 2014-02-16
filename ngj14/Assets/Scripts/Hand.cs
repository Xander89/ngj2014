using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {


	public Renderer _renderer;
	public SpriteFade red;
	private bool _restart = false;
	private float time = 0;
	public bool CanBeKilled = false;
	public SpriteRenderer hint;
	private SpriteFade fade;

	void Start()
	{
		fade = hint.GetComponent<SpriteFade>();
	}

	void Update()
	{
		time += Time.deltaTime;

		if (time < 3)
		{
			if (_renderer.isVisible)
			{
				_restart = false;
				fade.StartFading(0f);
				SpriteRenderer redflash = red.GetComponent<SpriteRenderer>();
				redflash.color = new Color(redflash.color.r, redflash.color.g, redflash.color.b, 0f);
				red.enabled = false;
			}
			return;
		}

		if (!_restart) {
			if (!_renderer.isVisible) {
				StartCoroutine(BlinkRed());
				_restart = true;
				time = 0;
				
			}
		}



		else if (_restart && time > 3 && time < 5)
		{
			Debug.Log ("fade it");
			if (CanBeKilled)
				Fade.FadeOut ();
			return;
		}
			
		else
		{
			if (CanBeKilled)
			{
				PlayerPrefs.SetInt("showMenu", 0);
				Application.LoadLevel(0);
			}
		}
	}

	IEnumerator BlinkRed()
	{
		red.enabled = true;
		fade.StartFading(1f);
		Debug.Log ("DUAHDUAW");
		red.StartFading(0.8f);
		yield return new WaitForSeconds(0.8f);
		red.StartFading(0f);
		yield return new WaitForSeconds(0.8f);
		red.StartFading(0.8f);
		yield return new WaitForSeconds(0.8f);
		red.StartFading(0f);
		Debug.Log ("DUAHDUAW2");
		fade.StartFading(0f);
	}

}
