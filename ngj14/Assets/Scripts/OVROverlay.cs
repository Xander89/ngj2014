using UnityEngine;
using System.Collections;

//-------------------------------------------------------------------------------------
// ***** OVROverlay
//
// OVRMainMenu is used to control the loading of different scenes. It also renders out 
// a menu that allows a user to modify various Rift settings, and allow for storing 
// these settings for recall later.
// 
// A user of this component can add as many scenes that they would like to be able to 
// have access to.
//
// OVRMainMenu is currently attached to the OVRPlayerController prefab for convenience, 
// but can safely removed from it and added to another GameObject that is used for general 
// Unity logic.
//
public class OVROverlay : MonoBehaviour
{
	private OVRPresetManager	PresetManager 	= new OVRPresetManager();
	
	public float 	FadeInTime    		= 2.0f;
	public Texture 	FadeInTexture 		= null;
	
	public Font 	FontReplace			= null;
	
	// Scenes to show onscreen
	public string [] SceneNames;
	public string [] Scenes;
	
	private bool ScenesVisible   	= false;
	
	// Spacing for scenes menu
	private int    	StartX			= 490;
	private int    	StartY			= 300;
	private int    	WidthX			= 300;
	private int    	WidthY			= 23;
	
	// Spacing for variables that users can change
	private int    	VRVarsSX		= 553;
	private int		VRVarsSY		= 350;
	private int    	VRVarsWidthX 	= 175;
	private int    	VRVarsWidthY 	= 23;
	
	private int    	StepY			= 25;
	
	// Handle to OVRCameraController
	private OVRCameraController CameraController = null;
	
	// Handle to OVRPlayerController
	private OVRPlayerController PlayerController = null;
	
	// Controller buttons
	private bool  PrevStartDown;
	private bool  PrevHatDown;
	private bool  PrevHatUp;
	
	private bool  ShowVRVars;
	
	private bool  OldSpaceHit;
	
	// FPS 
	private float  UpdateInterval 	= 0.5f;
	private float  Accum   			= 0; 	
	private int    Frames  			= 0; 	
	private float  TimeLeft			= 0; 				
	private string strFPS			= "FPS: 0";
	
	// IPD shift from physical IPD
	public float   IPDIncrement		= 0.0025f;
	private string strIPD 			= "IPD: 0.000";	
	
	// Prediction (in ms)
	public float   PredictionIncrement = 0.001f; // 1 ms
	private string strPrediction       = "Pred: OFF";	
	
	// FOV Variables
	public float   FOVIncrement		= 0.2f;
	private string strFOV     		= "FOV: 0.0f";
	
	// Distortion Variables
	public float   DistKIncrement   = 0.001f;
	//private string strDistortion 	= "Dist k1: 0.00f k2 0.00f";
	
	// Height adjustment
	public float   HeightIncrement   = 0.01f;
	private string strHeight     	 = "Height: 0.0f";
	
	// Speed and rotation adjustment
	public float   SpeedRotationIncrement   	= 0.05f;
	private string strSpeedRotationMultipler    = "Spd. X: 0.0f Rot. X: 0.0f";
	
	private bool   LoadingLevel 	= false;	
	private float  AlphaFadeValue	= 1.0f;
	private int    CurrentLevel		= 0;
	
	// Rift detection
	private bool   HMDPresent           = false;
	private bool   SensorPresent        = false;
	private float  RiftPresentTimeout   = 0.0f;
	private string strRiftPresent		= "";
	
	// Device attach / detach
	public enum Device {HMDSensor, HMD, LatencyTester}
	private float  DeviceDetectionTimeout 	= 0.0f;
	private string strDeviceDetection 		= "";
	
	// Mag yaw-drift correction
	private OVRMagCalibration   MagCal     = new OVRMagCalibration();
	
	// Replace the GUI with our own texture and 3D plane that
	// is attached to the rendder camera for true 3D placement
	private OVRGUI  		GuiHelper 		 = new OVRGUI();
	private GameObject      GUIRenderObject  = null;
	private RenderTexture	GUIRenderTexture = null;
	
	// Crosshair system, rendered onto 3D plane
	public Texture  CrosshairImage 			= null;
	private OVRCrosshair Crosshair        	= new OVRCrosshair();
	
	// Create a delegate for update functions
	private delegate void updateFunctions();
	private updateFunctions UpdateFunctions;
	
	
	
	// * * * * * * * * * * * * *
	
	// Awake
	void Awake()
	{
		// Find camera controller
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVROverlay: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVROverlay: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];
		
		// Find player controller
		OVRPlayerController[] PlayerControllers;
		PlayerControllers = gameObject.GetComponentsInChildren<OVRPlayerController>();
		
		if(PlayerControllers.Length == 0)
			Debug.LogWarning("OVROverlay: No OVRPlayerController attached.");
		else if (PlayerControllers.Length > 1)
			Debug.LogWarning("OVROverlay: More then 1 OVRPlayerController attached.");
		else
			PlayerController = PlayerControllers[0];
		
	}
	
	// Start
	void Start()
	{
		AlphaFadeValue = 1.0f;	
		CurrentLevel   = 0;
		PrevStartDown  = false;
		PrevHatDown    = false;
		PrevHatUp      = false;
		ShowVRVars	   = false;
		OldSpaceHit    = false;
		strFPS         = "FPS: 0";
		LoadingLevel   = false;	
		
		ScenesVisible    = false;
		
		// Ensure that camera controller variables have been properly
		// initialized before we start reading them
		if(CameraController != null)
		{
			CameraController.InitCameraControllerVariables();
			GuiHelper.SetCameraController(ref CameraController);
		}
		
		// Set the GUI target 
		GUIRenderObject = GameObject.Instantiate(Resources.Load("OVRGUIObjectMain")) as GameObject;
		
		if(GUIRenderObject != null)
		{
			if(GUIRenderTexture == null)
			{
				int w = Screen.width;
				int h = Screen.height;
				
				if(CameraController.PortraitMode == true)
				{
					int t = h;
					h = w;
					w = t;
				}
				
				// We don't need a depth buffer on this texture
				GUIRenderTexture = new RenderTexture(w, h, 0);	
				GuiHelper.SetPixelResolution(w, h);
				// NOTE: All GUI elements are being written with pixel values based
				// from DK1 (1280x800). These should change to normalized locations so 
				// that we can scale more cleanly with varying resolutions
				//GuiHelper.SetDisplayResolution(OVRDevice.HResolution, 
				//								 OVRDevice.VResolution);
				GuiHelper.SetDisplayResolution(1280.0f, 800.0f);
			}
		}
		
		// Attach GUI texture to GUI object and GUI object to Camera
		if(GUIRenderTexture != null && GUIRenderObject != null)
		{
			GUIRenderObject.renderer.material.mainTexture = GUIRenderTexture;
			
			if(CameraController != null)
			{
				// Grab transform of GUI object
				Transform t = GUIRenderObject.transform;
				// Attach the GUI object to the camera
				CameraController.AttachGameObjectToCamera(ref GUIRenderObject);
				// Reset the transform values (we will be maintaining state of the GUI object
				// in local state)
				OVRUtils.SetLocalTransform(ref GUIRenderObject, ref t);
				// Deactivate object until we have completed the fade-in
				// Also, we may want to deactive the render object if there is nothing being rendered
				// into the UI
				// we will move the position of everything over to the left, so get
				// IPD / 2 and position camera towards negative X
				Vector3 lp = GUIRenderObject.transform.localPosition;
				float ipd = 0.0f;
				CameraController.GetIPD(ref ipd);
				lp.x -= ipd * 0.5f;
				GUIRenderObject.transform.localPosition = lp;
				
				GUIRenderObject.SetActive(false);
			}
		}
		
		// Make sure to hide cursor 
		if(Application.isEditor == false)
		{
			Screen.showCursor = false; 
			Screen.lockCursor = true;
		}
		
		// CameraController updates
		if(CameraController != null)
		{
			UpdateFunctions += UpdateDistortionCoefs;

		}
		
		// PlayerController updates
		if(PlayerController != null)
		{
			UpdateFunctions += UpdatePlayerControllerMovement;
		}
		
		// Device updates
		UpdateFunctions += UpdateDeviceDetection;
		UpdateFunctions += UpdateResetOrientation;
		OVRMessenger.AddListener<Device, bool>("Sensor_Attached", UpdateDeviceDetectionMsgCallback);
		
		// Mag Yaw-Drift correction
		// We will test to see if we are already calibrated by the
		// Calibration tool
		MagCal.SetInitialCalibarationState(); 
		UpdateFunctions += MagCal.UpdateMagYawDriftCorrection;
		MagCal.SetOVRCameraController(ref CameraController);
		
		// Crosshair functionality
		Crosshair.Init();
		Crosshair.SetCrosshairTexture(ref CrosshairImage);
		Crosshair.SetOVRCameraController (ref CameraController);
		Crosshair.SetOVRPlayerController(ref PlayerController);
		UpdateFunctions += Crosshair.UpdateCrosshair;
		
		// Check for HMD and sensor
		CheckIfRiftPresent();
		
		// Init static members
		ScenesVisible = false;
	}
	
	// Update
	void Update()
	{		
		if(LoadingLevel == true)
			return;
		
		// Update specific delegate variables that are not passed through
		// the delegate master function (may change UpdateFunctions to take
		// a data ptr or certain variables)
		// MagCal.MagAutoCalibrate = MagAutoCalibrate;
		
		UpdateFunctions();
		
		// Toggle Fullscreen
		if(Input.GetKeyDown(KeyCode.F11))
			Screen.fullScreen = !Screen.fullScreen;
		
		// Escape Application
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
	
	// UpdateDistortionCoefs
	void UpdateDistortionCoefs()
	{
		float Dk0 = 0.0f;
		float Dk1 = 0.0f;
		float Dk2 = 0.0f;
		float Dk3 = 0.0f;
		
		// * * * * * * * * *
		// Get the distortion coefficients to apply to shader
		CameraController.GetDistortionCoefs(ref Dk0, ref Dk1, ref Dk2, ref Dk3);
		
		if(Input.GetKeyDown(KeyCode.Alpha1))
			Dk1 -= DistKIncrement;
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			Dk1 += DistKIncrement;
		
		if(Input.GetKeyDown(KeyCode.Alpha3))
			Dk2 -= DistKIncrement;
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			Dk2 += DistKIncrement;
		
		CameraController.SetDistortionCoefs(Dk0, Dk1, Dk2, Dk3);
		
		//if(ShowVRVars == true)// limit gc
		//	strDistortion = 
		//	System.String.Format ("DST k1: {0:F3} k2 {1:F3}", Dk1, Dk2);
	}
	

	// UpdatePlayerControllerMovement
	void UpdatePlayerControllerMovement()
	{
		if(PlayerController != null)
			PlayerController.SetHaltUpdateMovement(ScenesVisible);
	}

	// GUI
	
	// * * * * * * * * * * * * * * * * *
	// OnGUI
	void OnGUI()
	{	
		// Important to keep from skipping render events
		if (Event.current.type != EventType.Repaint)
			return;
		
		// Fade in screen
		if(AlphaFadeValue > 0.0f)
		{
			AlphaFadeValue -= Mathf.Clamp01(Time.deltaTime / FadeInTime);
			if(AlphaFadeValue < 0.0f)
			{
				AlphaFadeValue = 0.0f;	
			}
			else
			{
				GUI.color = new Color(0, 0, 0, AlphaFadeValue);
				GUI.DrawTexture( new Rect(0, 0, Screen.width, Screen.height ), FadeInTexture ); 
				return;
			}
		}
		
		// We can turn on the render object so we can render the on-screen menu
		if(GUIRenderObject != null)
		{
			if (ScenesVisible || ShowVRVars || Crosshair.IsCrosshairVisible() || 
			    RiftPresentTimeout > 0.0f || DeviceDetectionTimeout > 0.0f )
				GUIRenderObject.SetActive(true);
			else
				GUIRenderObject.SetActive(false);
		}
		
		//***
		// Set the GUI matrix to deal with portrait mode
		Vector3 scale = Vector3.one;
		if(CameraController.PortraitMode == true)
		{
			float h = OVRDevice.HResolution;
			float v = OVRDevice.VResolution;
			scale.x = v / h; 					// calculate hor scale
			scale.y = h / v; 					// calculate vert scale
		}
		Matrix4x4 svMat = GUI.matrix; // save current matrix
		// substitute matrix - only scale is altered from standard
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
		
		// Cache current active render texture
		RenderTexture previousActive = RenderTexture.active;
		
		// if set, we will render to this texture
		if(GUIRenderTexture != null)
		{
			RenderTexture.active = GUIRenderTexture;
			GL.Clear (false, true, new Color (0.0f, 0.0f, 0.0f, 0.0f));
		}
		
		// Update OVRGUI functions (will be deprecated eventually when 2D renderingc
		// is removed from GUI)
		GuiHelper.SetFontReplace(FontReplace);
		
		// If true, we are displaying information about the Rift not being detected
		// So do not show anything else
		if(GUIShowRiftDetected() != true)
		{	
			GUIShowVRVariables();
		}
		
		Crosshair.OnGUICrosshair();
		
		// Restore active render texture
		RenderTexture.active = previousActive;
		
		// ***
		// Restore previous GUI matrix
		GUI.matrix = svMat;
	}
	
	// GUIShowVRVariables
	void GUIShowVRVariables()
	{
		bool SpaceHit = Input.GetKey("space");
		if ((OldSpaceHit == false) && (SpaceHit == true))
		{
			if(ShowVRVars == true) 
				ShowVRVars = false;
			else 
				ShowVRVars = true;
		}
		
		OldSpaceHit = SpaceHit;
		
		int y   = VRVarsSY;
		
		// Print out auto mag correction state
		MagCal.GUIMagYawDriftCorrection(VRVarsSX, y, 
		                                VRVarsWidthX, VRVarsWidthY,
		                                ref GuiHelper);
		
		// Draw FPS
		GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
		                     ref strFPS, Color.green);
		
		// Don't draw these vars if CameraController is not present
		if(CameraController != null)
		{
			GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
			                     ref strPrediction, Color.white);		
			GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
			                     ref strIPD, Color.yellow);
			GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
			                     ref strFOV, Color.white);
		}
		
		// Don't draw these vars if PlayerController is not present
		if(PlayerController != null)
		{
			GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
			                     ref strHeight, Color.yellow);
			GuiHelper.StereoBox (VRVarsSX, y += StepY, VRVarsWidthX, VRVarsWidthY, 
			                     ref strSpeedRotationMultipler, Color.white);
		}
		
		// Eventually remove distortion from being changed
		/*
		// Don't draw if CameraController is not present
		if(CameraController != null)
		{
			// Distortion k values
			y += StepY;
			GUIStereoBox (VRVarsSX, y, VRVarsWidthX, VRVarsWidthY, 
							 ref strDistortion, 
							 Color.red);
		}
		*/
		
	}
	
	// RIFT DETECTION
	
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
	
	// GUIShowRiftDetected
	bool GUIShowRiftDetected()
	{
		if(RiftPresentTimeout > 0.0f)
		{
			GuiHelper.StereoBox (StartX, StartY, WidthX, WidthY, 
			                     ref strRiftPresent, Color.white);
			
			return true;
		}
		else if(DeviceDetectionTimeout > 0.0f)
		{
			GuiHelper.StereoBox (StartX, StartY, WidthX, WidthY, 
			                     ref strDeviceDetection, Color.white);
			
			return true;
		}
		
		return false;
	}
	
	// UpdateDeviceDetection
	void UpdateDeviceDetection()
	{
		if(RiftPresentTimeout > 0.0f)
			RiftPresentTimeout -= Time.deltaTime;
		
		if(DeviceDetectionTimeout > 0.0f)
			DeviceDetectionTimeout -= Time.deltaTime;
	}
	
	// UpdateDeviceDetectionMsgCallback
	void UpdateDeviceDetectionMsgCallback(Device device, bool attached)
	{
		if(attached == true)
		{
			switch(device)
			{
			case(Device.HMDSensor):
				strDeviceDetection = "HMD SENSOR ATTACHED";
				break;
				
			case(Device.HMD):
				strDeviceDetection = "HMD ATTACHED";
				break;
				
			case(Device.LatencyTester):
				strDeviceDetection = "LATENCY SENSOR ATTACHED";
				break;
			}
		}
		else
		{
			switch(device)
			{
			case(Device.HMDSensor):
				strDeviceDetection = "HMD SENSOR DETACHED";
				break;
				
			case(Device.HMD):
				strDeviceDetection = "HMD DETACHED";
				break;
				
			case(Device.LatencyTester):
				strDeviceDetection = "LATENCY SENSOR DETACHED";
				break;
			}
		}
		
		// Do not show on startup of level, since we will allow the
		// other method of detecting the rift to show, and not allow
		// this method to create a false positive detection at the start
		if(AlphaFadeValue == 0.0f)
			DeviceDetectionTimeout = 3.0f;
	}
	
	// RIFT RESET ORIENTATION
	
	// UpdateResetOrientation
	void UpdateResetOrientation()
	{
		if( ((ScenesVisible == false) && 
		     (OVRGamepadController.GPC_GetButton((int)OVRGamepadController.Button.Down) == true)) ||
		   (Input.GetKeyDown(KeyCode.B) == true) )
		{
			OVRDevice.ResetOrientation(0);
		}
	}
	
}
