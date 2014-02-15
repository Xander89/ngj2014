using UnityEngine;
using System.Collections;

public class EyeballAnimation : MonoBehaviour 
{

	private GameObject hand; 
	private GameObject guiCamera;

	void Start () 
	{
		hand = GameObject.FindGameObjectWithTag("Player");
	}

	void Update () 
	{
		//transform.LookAt(hand.transform);
		
		if (transform.eulerAngles.x > 90)
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90f, 90f);
		else
			transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90f, -90f);
	}
}
