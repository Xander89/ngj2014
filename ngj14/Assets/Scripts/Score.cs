﻿using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public static float _elapsedTime = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		_elapsedTime += Time.deltaTime;
		guiText.text = "Score: " + (int)(_elapsedTime*1000/64);  
	}
}
