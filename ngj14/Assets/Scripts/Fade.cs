using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {

	private bool _start = true;
	// FadeInOut
	//
	//--------------------------------------------------------------------
	//                        Public parameters
	//--------------------------------------------------------------------
	
	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.33f;
	
	int drawDepth = -1000;
	
	//--------------------------------------------------------------------
	//                       Private variables
	//--------------------------------------------------------------------
	
	private float alpha = 1.0f; 
	
	private static int fadeDir = -1;
	
	//--------------------------------------------------------------------
	//                       Runtime functions
	//--------------------------------------------------------------------
	
	//--------------------------------------------------------------------
	
	void OnGUI(){
		
		alpha += fadeDir * fadeSpeed * Time.deltaTime;	
		alpha = Mathf.Clamp01(alpha);	

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		//GUI.color.a = alpha;
		
		GUI.depth = drawDepth;


		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}
	
	//--------------------------------------------------------------------
	
	public static void FadeIn(){
		fadeDir = -1;	
	}
	
	//--------------------------------------------------------------------
	
	public static void FadeOut(){
		fadeDir = 1;	
	}
	
	void Start(){
		alpha=1;
		FadeIn();
	}

}
