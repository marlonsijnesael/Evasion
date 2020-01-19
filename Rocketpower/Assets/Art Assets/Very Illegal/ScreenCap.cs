using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCap : MonoBehaviour {

	public KeyCode screenCapKey = KeyCode.Return;
	public int captureSize = 1;

	// Use this for initialization
	void Start() {
		Debug.Log(Application.persistentDataPath);
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(screenCapKey)) {
			string date = System.DateTime.Now.ToString();
			date = date.Replace("/", "-");
			date = date.Replace(" ", "_");
			date = date.Replace(":", "-");
			ScreenCapture.CaptureScreenshot("ScreenCap_" + date + ".png", captureSize);
			Debug.Log("screenshot captured, named ScreenCap_" + date + ".png");
		}
	}
}