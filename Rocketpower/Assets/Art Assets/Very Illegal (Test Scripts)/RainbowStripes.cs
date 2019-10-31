using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RainbowStripes : MonoBehaviour {

	private static bool hasBeenInitialised = false;

	public Material rainbowMaterial;
	public Material officialYellowMaterial;

	private Color buildColor = Color.yellow;
	private Color randomColor = new Color(0, 0, 0, 1);

	private void Awake() {
		if (Application.isEditor) {
			//Debug.Log("We are running this from inside of the editor!");
			RandomColor();
		}
		else {
			//Debug.Log("this is a build");
			SetYellow();
		}
	}

	private void Start() {
		if (Application.isEditor) {
			//Debug.Log("We are running this from inside of the editor!");
			RandomColor();
		}
		else {
			//Debug.Log("this is a build");

		}
	}

	private void RandomColor() {
		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		int unixTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
		Debug.Log(unixTime);

		//make the number somewhat smaller for the sine to work with (the factor is 1/3600 so that it changes approximately 120 degrees over the colour wheel per hour)
		float t = (unixTime % 1000000) * 0.00027777777f;
		Debug.Log(t);

		randomColor.r = Mathf.Sin(t) * .5f + .5f;
		randomColor.g = Mathf.Sin(t + 1) * .5f + .5f;
		randomColor.b = Mathf.Sin(t + 2) * .5f + .5f;

		rainbowMaterial.SetColor("_BaseColor", randomColor);
		rainbowMaterial.SetColor("_EmissiveColor", randomColor);
		rainbowMaterial.SetColor("_EmissiveColorLDR", randomColor);
		Debug.Log("set M_YellowLights colour to " + randomColor);
	}

	private void SetYellow() {
		Color baseColor = officialYellowMaterial.GetColor("_BaseColor");
		Color emissiveColor = officialYellowMaterial.GetColor("_EmissiveColor");
		Color emissiveColorLDR = officialYellowMaterial.GetColor("_EmissiveColorLDR");

		rainbowMaterial.SetColor("_BaseColor", baseColor);
		rainbowMaterial.SetColor("_EmissiveColor", emissiveColor);
		rainbowMaterial.SetColor("_EmissiveColorLDR", emissiveColorLDR);
	}

}
