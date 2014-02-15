using UnityEngine;
using System.Collections;

public class ScreenHandConverter : MonoBehaviour {

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Camera guiCamera;

	[SerializeField]
	private float radius = 5.0f;

	[SerializeField]
	private GameObject enemySpawn;

	[SerializeField]
	private GameObject colliderGUIPrefab;

	private GameObject collider;

	// Use this for initialization
	void Start () {
		collider = (GameObject)Instantiate(colliderGUIPrefab,new Vector3(1000,1000,1000), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 v = mainCamera.WorldToScreenPoint(transform.position + transform.parent.position);
		Vector3 vx = new Vector3 (v.x, v.y, enemySpawn.transform.position.z - guiCamera.transform.parent.position.z);
		Vector3 screen = guiCamera.ScreenToWorldPoint(vx);
		//Vector3 l = screen - guiCamera.transform.position;
		//Vector3 l0 = guiCamera.transform.position;
		//Vector3 n = new Vector3 (0.0f,0.0f,1.0f);
		//Vector3 p0 = l0 + new Vector3(0.0f,0.0f,-enemySpawn.transform.position.z);
		//float d = Vector3.Dot((p0 - l0),n)/Vector3.Dot(l,n); 
		//collider.transform.position = l0 + d * l;
		collider.transform.position = screen;
	}

	void OnDrawGizmos()
	{
		Vector3 v = mainCamera.WorldToScreenPoint(transform.position);
		Vector3 screen = guiCamera.ScreenToWorldPoint(v);
		Vector3 l = screen - guiCamera.transform.position;
		Vector3 l0 = guiCamera.transform.position;
		Vector3 n = new Vector3 (0.0f,0.0f,1.0f);
		Vector3 p0 = l0 + new Vector3(0.0f,0.0f,-enemySpawn.transform.position.z);
		float d = Vector3.Dot((p0- l0),n)/Vector3.Dot(l,n); 
		Gizmos.DrawRay(new Ray(mainCamera.transform.position,l));
	}
}
