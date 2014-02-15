using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject _platform;
	// make singleton for character..
	[SerializeField]
	private GameObject _player;	
	[SerializeField]
	private GameObject _platformSpawnPoint;
	[SerializeField] 
	private int _maxSpawnDistance = 30;
	[SerializeField]
	private GameObject _platformStartSpawnPoint;
	[SerializeField]
	private GameObject _platformContainer;

	private Vector3 _distance;

	private Vector3 _lastPosition;

	private GameObject _currPlatform;

	private List<GameObject> _platforms = new List<GameObject>();

	// Use this for initialization
	void Start () {
		_platformSpawnPoint.transform.position = 
			new Vector3 (0, 0, _platform.transform.localScale.z);
		_platformSpawnPoint.transform.position = new Vector3 (0, 0, 
		                                                      -_platform.transform.localScale.z);
	}
	
	// Update is called once per frame
	void Update () {
		_distance = _platformSpawnPoint.transform.position - _player.transform.position;

		if (_distance.z < _maxSpawnDistance) 
		{	
			_currPlatform = (GameObject) Instantiate (_platform, _lastPosition + 
			             new Vector3(0, 0, _platform.transform.localScale.z + 
			            _platformSpawnPoint.transform.position.z), Quaternion.identity);
			_currPlatform.transform.parent = _platformContainer.transform;
			_platformSpawnPoint.transform.position = new Vector3(0, 0, 
			                                            _platformSpawnPoint.transform.position.z + 
			                                            _platform.transform.localScale.z);
			_platforms.Add(_currPlatform);
		}
		PlatformCleanup ();
		//_distance = _player.transform.position - 
	}

	void PlatformCleanup()
	{
		for (int i = 0; i < _platforms.Count; i++) 
		{
			if(_player.transform.position.z - _platforms[i].transform.position.z >=
			   _platform.transform.localScale.z*2)
			{
				Debug.Log("removing platform...");
				Destroy(_platforms[i]);
				_platforms.RemoveAt(i);
			}
		}
	}
}
