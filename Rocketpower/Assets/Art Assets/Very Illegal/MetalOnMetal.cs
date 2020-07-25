using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class MetalOnMetal : MonoBehaviour
{

	public GameObject metalSparkLeft;
	public GameObject metalSparkRight;

	private StateMachine stateMachine;
	private WallRunState currentState = WallRunState.none;

	private VisualEffect leftEffect;
	private VisualEffect rightEffect;

	private enum WallRunState {
		none,
		left,
		right
	}

	private void Start() {
		stateMachine = gameObject.GetComponent<StateMachine>();
		leftEffect = metalSparkLeft.GetComponent<VisualEffect>();
		rightEffect = metalSparkRight.GetComponent<VisualEffect>();

		leftEffect.enabled = false;
		rightEffect.enabled = false;
	}

	private void FixedUpdate() {
		if (currentState == WallRunState.none) { //if not wallrunning, check if started wallrunning
			//Debug.Log("state " + stateMachine.playerState);
			if (stateMachine.playerState.ToString().Contains("LEFT")) {
				currentState = WallRunState.left;
				leftEffect.enabled = true;
				//Debug.Log("left");
			}
			else if (stateMachine.playerState.ToString().Contains("RIGHT")) {
				currentState = WallRunState.right;
				rightEffect.enabled = true;
				//Debug.Log("right");
			}
		}
		else { //if wallrunning, check if stopped walrunning
			if (!stateMachine.playerState.ToString().Contains("WALLRUN")) {
				currentState = WallRunState.none;
				leftEffect.enabled = false;
				rightEffect.enabled = false;
				Debug.Log(stateMachine.playerState.ToString());
				//Debug.Log("none");
			}
		}
	}

}
