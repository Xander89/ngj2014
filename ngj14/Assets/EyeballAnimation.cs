using UnityEngine;
using System.Collections;

public class EyeballAnimation : MonoBehaviour 
{

	private GameObject hand; 
	private GameObject guiCamera;

	void Start () 
	{
		hand = GameObject.FindGameObjectWithTag("Player");
		guiCamera = GameObject.FindGameObjectWithTag("GUIRCamera");
	}

	void Update () 
	{
		//transform.LookAt(hand.transform);
		Vector3 screen = Camera.main.WorldToScreenPoint(hand.transform.position);
		screen = guiCamera.GetComponent<Camera>().ScreenToWorldPoint(screen);
		Vector3 v = new Vector3(screen.x - transform.position.x, screen.y - transform.position.y, 0f);
		transform.rotation = Quaternion.LookRotation(v, new Vector3(0f, 0f, 1f));
	}
}
