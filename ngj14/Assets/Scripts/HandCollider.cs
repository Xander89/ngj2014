using UnityEngine;
using System.Collections;

public class HandCollider : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		Application.LoadLevel (0);
	}
}
