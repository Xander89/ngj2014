﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject _platform;

	[SerializeField]
	private GameObject[] _corridors;

	// make singleton for character..
	[SerializeField]
	private GameObject _player;	
	//[SerializeField]
	//private GameObject _platformSpawnPoint;
	[SerializeField] 
	private int _maxSpawnDistance = 1;
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

	private string _corridor1MeshParentName = "polySurface73";
	private string _corridor2MeshParentName = "polySurface74";
	private string _corridor3MeshParentName = "polySurface75";

	int  _randomOrientation = 0;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () {

		/*GameObject meshParent = _corridors [0].transform.FindChild (_corridor1MeshParentName).gameObject;
		Debug.Log(meshParent.GetComponent<MeshFilter>());

		_templateMesh        = meshParent.GetComponent<MeshFilter>().sharedMesh;
*/
		_templateMesh = _corridors [0].GetComponent<MeshFilter> ().sharedMesh;
		_templateBounds      = _templateMesh.bounds;
		
		Debug.Log ("bounds half size: " + _templateBounds.size.z/2);
		Debug.Log ("platform z scale:   " + _corridors[0].transform.localScale.z);
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
		if (_distance.z < _maxSpawnDistance*_templateBounds.size.z*_corridors[0].transform.localScale.z) 
		//if (_distance.z < _maxSpawnDistance)
		{	
			/*_currPlatform = (GameObject) Instantiate (_platform, _lastPosition + 
			             new Vector3(0, 0, _platform.transform.localScale.z + 
			            _platformSpawnPoint.transform.position.z), Quaternion.identity);*/
			int index = Random.Range(0, _corridors.Length);

			_randomOrientation = Random.Range(0, 100);

			if(_randomOrientation < 50)
			{
				_currPlatform = (GameObject) Instantiate (_corridors[index], _lastPosition + 
				                                          new Vector3(0, 0, _bounds.size.z/2*_corridors[0].transform.localScale.z), 
				                                          Quaternion.Euler(new Vector3(0, 90, 0)));
			}
			else
			{
				_currPlatform = (GameObject) Instantiate (_corridors[index], _lastPosition + 
				                                          new Vector3(0, 0, _bounds.size.z/2*_corridors[0].transform.localScale.z), 
				                                          Quaternion.Euler(new Vector3(0, -90, 0)));
			}

			_currPlatform.transform.parent = _platformContainer.transform;

			//_currentMesh = _currPlatform.GetComponentInChildren<MeshFilter>().sharedMesh;
			_currentMesh = _currPlatform.GetComponent<MeshFilter>().sharedMesh;
			_bounds      = _currentMesh.bounds;

			//Debug.Log ("bounds half size: " + _bounds.size.z/2);
			//Debug.Log ("platform z pos:   " + _currPlatform.transform.position.z);

			_lastPosition = new Vector3(_currPlatform.transform.position.x, _currPlatform.transform.position.y,
			                            _currPlatform.transform.position.z + _bounds.size.z*_currPlatform.transform.localScale.z);
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
			   _corridors[0].transform.localScale.z*_templateBounds.size.z)
			{
				//Debug.Log("removing platform...");
				Destroy(_platforms[i]);
				_platforms.RemoveAt(i);
			}
		}
	}
}
