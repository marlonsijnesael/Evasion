using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class FluxManager : MonoBehaviour
{
    [HideInInspector] public StateMachine smP1;
    [HideInInspector] public StateMachine smP2;
	public Color player1color;
	public Color player2color;
	[HideInInspector] public PlayerFlux player1;
    [HideInInspector] public PlayerFlux player2;
    public float fluxCaptureTime;
    public PlayerFlux fluxPlayer;
    private int player1score;
    private int player2score;

    [HideInInspector] public Text textP1;
    [HideInInspector] public Text textP2;
    [HideInInspector] public Text textFluxPlayer;
    [HideInInspector] public Slider sliderCaptureTime;
    [HideInInspector] public Image sliderFillImage;
    [HideInInspector] public GameObject sliderCaptureObject;
    [HideInInspector] public GameObject canvas;

    GameObject[] platformArray;

    public bool isFluxPlayerColliderOnCD;

    private void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        canvas.gameObject.SetActive(true);
        sliderCaptureTime.maxValue = fluxCaptureTime;
    }

    private void Update() {
        //Press 1 or 2 to change flux
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            fluxPlayer = player1;
            textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            fluxPlayer = player2;
            textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
        }
    }

    public void ColorandSpeedSwitch(){
        //Change Capture Bar Fill Color + Runspeed
        if(fluxPlayer == player1){
            sliderFillImage.color = player2color;
            smP1.maxSpeed = 12;
        }
        else{
            smP1.maxSpeed = 14;
        }
        if(fluxPlayer == player2){
            sliderFillImage.color = player1color;
            smP2.maxSpeed = 12;
        }
        else{
            smP2.maxSpeed = 14;
        }
    }

    public void updateScore() {
        player1score=0;
        player2score=0;

		foreach (GameObject platform in platformArray){
			PlatformState state = platform.GetComponent<PlatformState>();
			if (state.GetPlayerID() == 1){
				player1score++;
			}
			else if (state.GetPlayerID() == 2){
				player2score++;
			}
		}
        textP1.text = player1score.ToString();
        textP2.text = player2score.ToString();
    }

    public void startGameRound(){

    }

    IEnumerator StartRoundCountDown(){
        int countDown = 5;
        while (countDown > 0){
            yield return new WaitForSeconds(5);
            countDown--;
        }
    }

    IEnumerator FluxColliderSeconds(){
        //Flux Capture Cooldown
        isFluxPlayerColliderOnCD = true;
		yield return new WaitForSeconds(1.5f);
        isFluxPlayerColliderOnCD = false;
	}
}
