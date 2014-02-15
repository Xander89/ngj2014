using UnityEngine;
using System.Collections;

public class SpriteFade : MonoBehaviour {

	private SpriteRenderer rend;
	private float targetAlpha = 1.0f;
	public float speed = 1.0f;

	void Start()
	{
		rend = GetComponentInChildren<SpriteRenderer>();
		targetAlpha = rend.color.a;

	}

	// Use this for initialization
	public void StartFading (float targetAlpha) {
		this.targetAlpha = targetAlpha;
	}
	
	// Update is called once per frame
	void Update () {
		float a = Mathf.Lerp(rend.color.a, targetAlpha, Time.deltaTime * speed);
		rend.color = new Color(rend.color.r,rend.color.g,rend.color.b,a);
	}
}
