using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {


	public Renderer _renderer;
	private bool _restart = false;
	private float time = 0;


	void Update()
	{
		time += Time.deltaTime;
		if (time < 2)
			return;

		if (!_restart) {
			if (!_renderer.isVisible) {
					Debug.Log ("fade it");
				Fade.FadeOut ();
				_restart = true;
				time = 0;
				
			}
		}
		else
		{
			Application.LoadLevel(0);
		}
	}

}
