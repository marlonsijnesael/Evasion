using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class FluxManager : MonoBehaviour
{
    
	public Color player1color;
	public Color player2color;
	[HideInInspector] public PlayerFlux player1;
    [HideInInspector] public PlayerFlux player2;
    public int fluxCaptureTime;
    public PlayerFlux fluxPlayer;
    private int player1score;
    private int player2score;

    public Text textP1;
    public Text textP2;
    public Text textFluxPlayer;
    public Slider sliderCaptureTime;
    public Image sliderFillImage;
    public GameObject sliderCaptureObject;
    public GameObject canvas;

    GameObject[] platformArray;

    public bool isFluxPlayerColliderOnCD;

    private void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        canvas.gameObject.SetActive(true);
        sliderCaptureTime.maxValue = fluxCaptureTime;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
			ChangeFluxplayer(player1);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
			ChangeFluxplayer(player2);
        }
    }
	
	public void ChangeFluxplayer(PlayerFlux newFluxPlayer){
		if (fluxPlayer){
			fluxPlayer.TurnFlux(false); //turn off flux for previous flux player, if any
		}
		fluxPlayer = newFluxPlayer;
		fluxPlayer.TurnFlux(true); //turn on flux for new flux player
		textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
		
		if(fluxPlayer == player1){
            sliderFillImage.color = player2color;
        }
        if(fluxPlayer == player2){
            sliderFillImage.color = player1color;
        }
		textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
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

    IEnumerator FluxColliderSeconds(){
		Debug.Log("Coroutine Start");
        isFluxPlayerColliderOnCD = true;
		yield return new WaitForSeconds(1.5f);
        isFluxPlayerColliderOnCD = false;
		Debug.Log("Coroutine Finish");
	}
}
