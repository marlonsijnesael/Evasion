using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class MetalOnMetal : MonoBehaviour
{

	[HideInInspector]public GameObject metalSparkLeft;
	[HideInInspector]public GameObject metalSparkRight;
	public ParticleSystem metalSystemLeft;
	public ParticleSystem metalSystemRight;

	public int emissionRate = 20;

	private StateMachine stateMachine;
	private WallRunState currentState = WallRunState.none;

	//private VisualEffect leftEffect;
	//private VisualEffect rightEffect;

	private enum WallRunState {
		none,
		left,
		right
	}

	private void Start() {
		stateMachine = gameObject.GetComponent<StateMachine>();
		metalSystemLeft.emissionRate = 0;
		metalSystemRight.emissionRate = 0;

		//leftEffect = metalSparkLeft.GetComponent<VisualEffect>();
		//rightEffect = metalSparkRight.GetComponent<VisualEffect>();

		//leftEffect.enabled = false;
		//rightEffect.enabled = false;
	}

	private void FixedUpdate() {
		if (currentState == WallRunState.none) { //if not wallrunning, check if started wallrunning
			if (stateMachine.playerState.ToString().Contains("LEFT")) {
				currentState = WallRunState.left;
				metalSystemLeft.emissionRate = emissionRate;

				//leftEffect.enabled = true;
			}
			else if (stateMachine.playerState.ToString().Contains("RIGHT")) {
				currentState = WallRunState.right;
				metalSystemRight.emissionRate = emissionRate;

				//rightEffect.enabled = true;
			}
		}
		else { //if wallrunning, check if stopped walrunning
			if (!stateMachine.playerState.ToString().Contains("WALLRUN")) {
				currentState = WallRunState.none;
				metalSystemLeft.emissionRate = 0;
				metalSystemRight.emissionRate = 0;

				//leftEffect.enabled = false;
				//rightEffect.enabled = false;
			}
		}
	}

}
