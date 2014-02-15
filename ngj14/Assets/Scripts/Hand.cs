using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour {


	private Renderer[] _renderers;
	private bool _restart = false;
	private float time = 0;

	void Awake()
	{
		_renderers = gameObject.GetComponentsInChildren<Renderer> ();
	}

	void Update()
	{
		time += Time.deltaTime;
		if (time < 2)
			return;

		if (!_restart) {
			foreach (Renderer r in _renderers) {
				if (!r.isVisible) {
						Debug.Log ("fade it");
						Fade.FadeOut ();
						_restart = true;
						time = 0;
						break;
				}
			}
		}
		else
		{
			Application.LoadLevel(0);
		}
	}

}
