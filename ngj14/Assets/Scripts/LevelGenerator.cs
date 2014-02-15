﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject _platform;
	// make singleton for character..
	[SerializeField]
	private GameObject _player;	
	//[SerializeField]
	//private GameObject _platformSpawnPoint;
	[SerializeField] 
	private int _maxSpawnDistance = 10;
	[SerializeField]
	private GameObject _platformStartSpawnPoint;
	[SerializeField]
	private GameObject _platformContainer;

	private Vector3 _distance;

	private Vector3 _lastPosition;

	private GameObject _currPlatform;

	private List<GameObject> _platforms = new List<GameObject>();

	private Mesh   _currentMesh;
	private Bounds _bounds;

	private Mesh _templateMesh;
	private Bounds _templateBounds;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () {
		_templateMesh        = _platform.GetComponent<MeshFilter>().sharedMesh;
		_templateBounds      = _templateMesh.bounds;
		
		Debug.Log ("bounds half size: " + _templateBounds.size.z/2);
		Debug.Log ("platform z scale:   " + _platform.transform.localScale.z);
		/*_platformSpawnPoint.transform.position = 
			new Vector3 (0, 0, _platform.transform.localScale.z);
		_platformSpawnPoint.transform.position = new Vector3 (0, 0, 
		                                                      -_platform.transform.localScale.z);*/
		/*_lastPosition = new Vector3 (_platform.transform.position.x,
		                             _platform.transform.position.y,
		                             _platform.transform.position.z + 
		                             _bounds.size.z/2); 
		Debug.Log ("bounds half size: " + _bounds.size.z/2);
		Debug.Log ("platform z pos:   " + _platform.transform.position.z);*/
	}
	
	// Update is called once per frame
	void Update () {
		//_distance = _platformSpawnPoint.transform.position - _player.transform.position;
		_distance = _lastPosition - _player.transform.position;
		//Debug.Log ("bounds half size: " + _bounds.size.z/2);
		// 30 platforms
		if (_distance.z < _maxSpawnDistance*_templateBounds.size.z*_platform.transform.localScale.z) 
		//if (_distance.z < _maxSpawnDistance)
		{	
			/*_currPlatform = (GameObject) Instantiate (_platform, _lastPosition + 
			             new Vector3(0, 0, _platform.transform.localScale.z + 
			            _platformSpawnPoint.transform.position.z), Quaternion.identity);*/
			_currPlatform = (GameObject) Instantiate (_platform, _lastPosition + 
			                                          new Vector3(0, 0, _bounds.size.z/2*_platform.transform.localScale.z), 
			                                          Quaternion.identity);
			_currPlatform.transform.parent = _platformContainer.transform;

			_currentMesh = _currPlatform.GetComponent<MeshFilter>().sharedMesh;
			_bounds      = _currentMesh.bounds;

			Debug.Log ("bounds half size: " + _bounds.size.z/2);
			Debug.Log ("platform z pos:   " + _currPlatform.transform.position.z);

			_lastPosition = new Vector3(_currPlatform.transform.position.x, _currPlatform.transform.position.y,
			                            _currPlatform.transform.position.z + _bounds.size.z/2*_currPlatform.transform.localScale.z);
			/*_platformSpawnPoint.transform.position = new Vector3(0, 0, 
			                                            _platformSpawnPoint.transform.position.z + 
			                                            _platform.transform.localScale.z);
			*/
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
			   _platform.transform.localScale.z*_templateBounds.size.z)
			{
				//Debug.Log("removing platform...");
				Destroy(_platforms[i]);
				_platforms.RemoveAt(i);
			}
		}
	}
}
