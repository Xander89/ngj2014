﻿using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text = "Score: " + (int)(Time.time*1000/64);  
	}
}
