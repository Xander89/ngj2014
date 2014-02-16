using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {


	public Renderer _renderer;
	public SpriteFade red;
	private GUIText _message;
	private bool _restart = false;
	private float time = 0;

	void Start()
	{
		_message = red.transform.FindChild("message").GetComponent<GUIText>();
	}

	void Update()
	{
		time += Time.deltaTime;

		if (time < 3)
		{
			if (_renderer.isVisible)
			{
				_restart = false;
				_message.enabled = false;
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
			Fade.FadeOut ();
			return;
		}
			
		else
		{
			Application.LoadLevel(0);
		}
	}

	IEnumerator BlinkRed()
	{
		red.enabled = true;
		_message.enabled = true;
		Debug.Log ("DUAHDUAW");
		red.StartFading(0.8f);
		yield return new WaitForSeconds(0.8f);
		red.StartFading(0f);
		yield return new WaitForSeconds(0.8f);
		red.StartFading(0.8f);
		yield return new WaitForSeconds(0.8f);
		red.StartFading(0f);
		Debug.Log ("DUAHDUAW2");
		_message.enabled = false;
	}

}
