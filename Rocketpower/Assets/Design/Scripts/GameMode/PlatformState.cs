using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformState : MonoBehaviour
{
	
	public int colorChangeTime;

	private int ownedByPlayer;
	private Color currentColor;
	private bool isChanging;

	private Material mat;
	
	private void Awake() {
		mat = transform.GetComponent<Renderer>().material;
	}
	
	public int GetPlayerID() {
		int playerID = ownedByPlayer;
		return playerID;
	}
	
	public bool GetIsChanging(){
		return isChanging;
	}

	public void ChangeColorTo(int playerID, Color c) {
		if (!isChanging) {
			ownedByPlayer = playerID;
			StartCoroutine(ChangeColor(c));
			isChanging = true;
		}
	}

	private IEnumerator ChangeColor(Color c) {

		mat.SetColor("_GoalColor", c);

		float timeFrac = 1.0f / (colorChangeTime * 1.0f);
		for (int i = 0; i <= colorChangeTime; i++) {
			mat.SetFloat("_State", i * timeFrac);
			yield return null;
		}

		mat.SetColor("_StartColor", c);

		currentColor = c;
		isChanging = false;
		yield return null;
	}
	
	
	
	
	/*legacy functions
    void Update() {
        platformColorCheck();
    }

    void platformColorCheck() {
        if (gameObject.GetComponent<Renderer>().material.name == "M_PlatformOrange_v1 (Instance)") {
            isPlatformOrange = !isPlatformBlue;
        }

        if (gameObject.GetComponent<Renderer>().material.name == "M_PlatformColorBlue_v1") {
            isPlatformBlue = !isPlatformOrange;
        }
    }
	//*/
}