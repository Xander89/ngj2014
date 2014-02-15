using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	[SerializeField]
	private float movementSpeed = 5;
	
	private CharacterController _characterController;

	void Awake()
	{
		_characterController = GetComponent<CharacterController> ();
		Debug.Log (_characterController);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.position = new Vector3 (transform.position.x, transform.position.y, 
		  //                                transform.position.z + movementSpeed);
		_characterController.Move(new Vector3(0, 0, movementSpeed) * Time.deltaTime);
	}
}
