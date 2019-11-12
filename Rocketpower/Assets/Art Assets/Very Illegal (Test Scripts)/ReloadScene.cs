using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
	
	public KeyCode key = KeyCode.Return;
	
	private Scene scene;
	
	private void Start(){
		scene = SceneManager.GetActiveScene();
	}
	
	private void Update(){
		if (Input.GetKeyDown(key)){
			SceneManager.LoadScene(scene.name);
		}
	}
}
