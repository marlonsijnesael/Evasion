﻿using System.Collections;
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
    public PlayerFlux fluxPlayer;

    private int player1score;
    private int player2score;

    public Text textP1;
    public Text textP2;
    public Text textFluxPlayer;
    public GameObject canvas;

    GameObject[] platformArray;

    private void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        canvas.gameObject.SetActive(true);

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            fluxPlayer = player1;
            textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            fluxPlayer = player2;
            textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
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

    private void switchColor(){
        if(fluxPlayer == player1){
            fluxPlayer = player2;

        }
        else {
            fluxPlayer = player1;
        }
    }
}
