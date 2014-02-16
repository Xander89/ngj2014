﻿using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public static float _elapsedTime = 0;
	public static int _score;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_elapsedTime += Time.deltaTime;
		_score = (int)(_elapsedTime * 1000 / 64);
		guiText.text = "Score: " + _score;  
	}
}
