using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    
	public int colorSwitchFrames;
	
	private Image img;
	private float colorSwitchFrac;
	
	private void Start(){
		img = transform.GetComponent<Image>();
		colorSwitchFrac = 1.0f / colorSwitchFrames;
		StartCoroutine(ChangeColor(Color.white, Color.white));
	}
	
	public void StartChangingColor(Color start, Color goal){
		StartCoroutine(ChangeColor(start, goal));
	}
	
	private IEnumerator ChangeColor(Color start, Color goal){
		for (int i = 0; i < colorSwitchFrames; i++){
			Color c = Color.Lerp(start, goal, i * colorSwitchFrac);
			img.color = c;
			yield return null;
		}
		yield return null;
	}
	
	
}
