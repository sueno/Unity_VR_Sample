using UnityEngine;
using System.Collections;

public class MotionGUI : MonoBehaviour {
	
	private static string message = "";
	private static bool rightAnchor = false;
	private static bool leftAnchor = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    	guiText.text = "L : "+(leftAnchor?"T":"F")+"  R : "+(rightAnchor?"T":"F")+"\n"+message;
	
	}
	
	public static void setRightAnchor(bool state) {
		rightAnchor = state;
	}

	public static void setLeftAnchor(bool state) {
		leftAnchor = state;
	}
	
	public static void setMessage(string state) {
		message = state;
	}
}
