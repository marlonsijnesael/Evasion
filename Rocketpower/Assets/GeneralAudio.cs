using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneralAudio : MonoBehaviour
{
	[HideInInspector] public PlayerFlux p1;
    [HideInInspector] public PlayerFlux p2;
	
	GameObject obj;
	GameObject obj2;
	
	FMOD.Studio.EventInstance OST_Intro;
	FMOD.Studio.ParameterInstance MainGame, IV;
	
	FMOD.Studio.EventInstance OST_2;
	FMOD.Studio.ParameterInstance MainGame2, EndGame, GameOver, OV;
	
	FMOD.Studio.EventInstance FluxEffect;
	FMOD.Studio.ParameterInstance FV;
	
	FMOD.Studio.EventInstance Checkpoint;
	FMOD.Studio.ParameterInstance HV, HP;
	
	FMOD.Studio.EventInstance Jump;
	FMOD.Studio.ParameterInstance JV;
	
		
	FMOD.Studio.EventInstance Headroll;
	FMOD.Studio.ParameterInstance HRV;
	
	bool eS = false;
	bool hasGameStarted = false;
	
	int rCT = 0;
	bool wSCT = false;
	bool oldWSCT = false;
	bool gO = false;
	public PlayerFlux fPlayer;
	int p1score, p2score;
	public int endGameMusicStart = 30;
	
    void Start()
    {
		obj = GameObject.FindGameObjectWithTag("Elevator");
		obj2 = GameObject.FindGameObjectWithTag("GameModeManager");
		
		OST_Intro = FMODUnity.RuntimeManager.CreateInstance("event:/OST/OST_Intro");
		OST_Intro.getParameter("MainGame", out MainGame);
        OST_Intro.getParameter("IV", out IV);
		MainGame.setValue(0);
		IV.setValue(0.8f);
	
		OST_Intro.start();

		OST_2 = FMODUnity.RuntimeManager.CreateInstance("event:/OST/OST_2");
		OST_2.getParameter("MainGame2", out MainGame2);
        OST_2.getParameter("OV", out OV);
		OST_2.getParameter("EndGame", out EndGame);
        OST_2.getParameter("GameOver", out GameOver);
		MainGame2.setValue(0);
		EndGame.setValue(0);
		GameOver.setValue(0);
		OV.setValue(0.9f);
		
		//OST_2.start();
		
		
		FluxEffect = FMODUnity.RuntimeManager.CreateInstance("event:/SD/FluxCarrier");
		FluxEffect.getParameter("FV", out FV);
		FV.setValue(1);
		
		Checkpoint = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Hexagon");
		Checkpoint.getParameter("HV", out HV);
		Checkpoint.getParameter("HP", out HP);
		HV.setValue(1);
		HP.setValue(0);
		
		Jump = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Jump");
		Jump.getParameter("JV", out JV);
		JV.setValue(1);
		
		Headroll = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Headroll");
		Headroll.getParameter("HRV", out HRV);
		HRV.setValue(1);
    }
	
	void StartOst()
	{
		OST_2.start();
	}
	
	public void CheckPointSound()
	{
		if (fPlayer == p1) {
			print("p1");
			HP.setValue(p1score) ;
		}
		else {
			print("p2");
			HP.setValue(p2score) ;
		}
		Checkpoint.start();
	}
	
	public void SparkSound()
	{
		FluxEffect.start();
	}
	
	public void JumpSound()
	{
		Jump.start();
	}
	
	public void HeadRollSound()
	{
		Headroll.start();
	}
	

    void Update()
    {
		eS = obj.GetComponent<Elevator>().elevatorStarted;
		
		rCT = obj2.GetComponent<FluxManager>().roundCountdownTime;
		wSCT = obj2.GetComponent<FluxManager>().isWinCountDownActive;
		gO = obj2.GetComponent<FluxManager>().gameOver;
		fPlayer = obj2.GetComponent<FluxManager>().fluxPlayer;
		p1score = obj2.GetComponent<FluxManager>().player1score;
		p2score = obj2.GetComponent<FluxManager>().player2score;
		p1 = obj2.GetComponent<FluxManager>().player1;
		p2 = obj2.GetComponent<FluxManager>().player2;

		print("fplayer:");
		print(fPlayer);
		
        if (eS == true) {
			MainGame.setValue(1);
		}
		
		if (eS == true && hasGameStarted == false) {
			StartOst();
			hasGameStarted = true;
		}
		
		if (rCT <= endGameMusicStart) {
			EndGame.setValue(1);
		}
		
		if (wSCT == true && oldWSCT != true) {
			EndGame.setValue(1);
			oldWSCT = true;
		}
		if (wSCT == false && oldWSCT != false) {
			EndGame.setValue(0);
			oldWSCT = false;
		}
		
		if (gO == true) {
			GameOver.setValue(1);
		}
		
		if (Input.GetKeyDown(KeyCode.Return)) {
			FluxEffect.start();
		}
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			CheckPointSound();
		}
    }
}
