using UnityEngine;
using System.Collections;

public class MovePlayer : MonoBehaviour {

	public float RotationAmount    = 1.5f;

	[SerializeField]
	private float movementSpeed = 5;

	private float RotationScaleMultiplier = 1.0f;
	private bool  AllowMouseRotation      = true;
	private float 	YRotation 	 = 0.0f;

	static float sDeltaRotationOld = 0.0f;
	
	private CharacterController _characterController;
	private OVRCameraController CameraController = null;
	
	// Rift detection
	private bool   HMDPresent           = false;
	private bool   SensorPresent        = false;
	private float  RiftPresentTimeout   = 0.0f;
	private string strRiftPresent		= "";

	private Camera CameraLeft, CameraRight, CameraLeftOrtho, CameraRightOrtho;

	[SerializeField]
	private GameObject orthoCamController;
	[SerializeField]
	private GameObject score;

	public static bool riftEnabled = true;

	void Awake()
	{
		_characterController = GetComponent<CharacterController> ();
		Debug.Log (_characterController);
	}

	// Use this for initialization
	void Start () {
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();

		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];

		CheckIfRiftPresent ();
		if (strRiftPresent.Equals ("")) {
						AllowMouseRotation = false;	

				} else {
			// Get the cameras
			Camera[] cameras = gameObject.GetComponentsInChildren<Camera>();
			
			for (int i = 0; i < cameras.Length; i++)
			{
				if(cameras[i].name == "CameraLeft")
					CameraLeft = cameras[i];
				
				if(cameras[i].name == "CameraRight")
					CameraRight = cameras[i];
			}
			
			if((CameraLeft == null) || (CameraRight == null))
				Debug.LogWarning("WARNING: Unity Cameras in OVRCameraController not found!");

			Camera[] orthoCams = orthoCamController.GetComponentsInChildren<Camera>();

			for (int i = 0; i < orthoCams.Length; i++)
			{
				if(orthoCams[i].name == "LeftOrthoCam")
					CameraLeftOrtho = orthoCams[i];
				
				if(orthoCams[i].name == "RightOrthoCam")
					CameraRightOrtho = orthoCams[i];
			}

			// remove left camera
			//CameraController.CameraLeft.enabled = false;
			//CameraController.CameraRight.rect = new Rect(0, 0, 1, 1);
			CameraLeft.enabled = false;
			CameraRight.rect = new Rect(0, 0, 1, 1);
			// remove left ortho camera
			CameraLeftOrtho.enabled = false;
			CameraRightOrtho.rect = new Rect(0, 0, 1, 1);
			CameraRightOrtho.transform.position = new Vector3(0, 0, -17);

			CameraController.LensCorrection = false;
			CameraController.Chromatic      = false;
			CameraController.PredictionOn   = false;
			score.transform.position = new Vector3(3.5f, 3.83f, score.transform.position.z);

			riftEnabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// rotate camera
		if (AllowMouseRotation == true) {
		// compute for key rotation
		float rotateInfluence = Time.deltaTime * RotationAmount * RotationScaleMultiplier;

		float deltaRotation = 0.0f, deltaRotationY = 0.0f;

						deltaRotation = Input.GetAxis ("Mouse X") * rotateInfluence * 3.25f * 10;
						deltaRotationY = Input.GetAxis ("Mouse Y") * rotateInfluence * 3.25f * 10;
		
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		sDeltaRotationOld = filteredDeltaRotation;

		if(CameraController != null)
		{
			Debug.Log("OVR Cam Controller: " + CameraController);
			Debug.Log("YROT: " + YRotation); 
			// Make sure to set the initial direction of the camera 
			// to match the game player direction
			//CameraController.SetOrientationOffset(OrientationOffset);
			CameraController.SetYRotation(-YRotation);
		
			//transform.Rotate(new Vector3(0, YRotation, 0));
		}
		//transform.Rotate(new Vector3(0, YRotation, 0));
		}
		// move player
		_characterController.Move(new Vector3(0, 0, movementSpeed) * Time.deltaTime);
	}

	// CheckIfRiftPresent
	// Checks to see if HMD and / or sensor is available, and displays a 
	// message if it is not
	void CheckIfRiftPresent()
	{
		HMDPresent = OVRDevice.IsHMDPresent();
		SensorPresent = OVRDevice.IsSensorPresent(0); // 0 is the main head sensor
		
		if((HMDPresent == false) || (SensorPresent == false))
		{
			RiftPresentTimeout = 5.0f; // Keep message up for 10 seconds
			
			if((HMDPresent == false) && (SensorPresent == false))
				strRiftPresent = "NO HMD AND SENSOR DETECTED";
			else if (HMDPresent == false)
				strRiftPresent = "NO HMD DETECTED";
			else if (SensorPresent == false)
				strRiftPresent = "NO SENSOR DETECTED";
		}
	}
}
