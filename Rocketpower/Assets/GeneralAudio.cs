using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// all general audio
public class GeneralAudio : MonoBehaviour
{
    // FMOD events and parameters
    FMOD.Studio.EventInstance OST_Intro;
    FMOD.Studio.ParameterInstance MainGame, IV;

    FMOD.Studio.EventInstance OST_2;
    public FMOD.Studio.ParameterInstance MainGame2, EndGame, GameOver, OV;

    FMOD.Studio.EventInstance FluxEffect;
    FMOD.Studio.ParameterInstance FV;

    FMOD.Studio.EventInstance Checkpoint;
    FMOD.Studio.ParameterInstance HV, HP;

    FMOD.Studio.EventInstance Jump;
    FMOD.Studio.ParameterInstance JV;

    FMOD.Studio.EventInstance Headroll;
    FMOD.Studio.ParameterInstance HRV;

    FMOD.Studio.EventInstance JumpPad;
    FMOD.Studio.ParameterInstance JPV;

    FMOD.Studio.EventInstance Atmosphere;
    FMOD.Studio.ParameterInstance AV;

    // --------------------------------
    public GameObject obj;
    GameObject obj2;

    [HideInInspector] public PlayerFlux p1;
    [HideInInspector] public PlayerFlux p2;

    bool eS = false;
    bool hasGameStarted = false;

    int rCT = 0;
    int p1score, p2score;
    bool wSCT = false;
    bool oldWSCT = false;
    bool gO = false;

    public PlayerFlux fPlayer;
    public int endGameMusicStart = 30;

    void Start()
    {
        obj = GameObject.FindGameObjectWithTag("Elevator");
        obj2 = GameObject.FindGameObjectWithTag("GameModeManager");

        OST_Intro = FMODUnity.RuntimeManager.CreateInstance("event:/OST/OST_Intro");
        OST_Intro.getParameter("MainGame", out MainGame);
        OST_Intro.getParameter("IV", out IV);
        MainGame.setValue(0);
        IV.setValue(0.9f);

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

        FluxEffect = FMODUnity.RuntimeManager.CreateInstance("event:/SD/FluxCarrier");
        FluxEffect.getParameter("FV", out FV);
        FV.setValue(0.8f);

        Checkpoint = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Hexagon");
        Checkpoint.getParameter("HV", out HV);
        Checkpoint.getParameter("HP", out HP);
        HV.setValue(1);
        HP.setValue(0);

        Jump = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Jump");
        Jump.getParameter("JV", out JV);
        JV.setValue(0.9f);

        Headroll = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Headroll");
        Headroll.getParameter("HRV", out HRV);
        HRV.setValue(0.8f);

        JumpPad = FMODUnity.RuntimeManager.CreateInstance("event:/SD/JumpPad");
        JumpPad.getParameter("JPV", out JPV);
        JPV.setValue(0.9f);

        Atmosphere = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Atmosphere");
        Atmosphere.getParameter("AV", out AV);
        AV.setValue(0.7f);

        Atmosphere.start();
    }

    private void StartOst()
    {
        OST_2.start();
    }

    public void CheckPointSound()
    {
        if (fPlayer == p1)
        {
            HP.setValue(p1score);
        }
        else
        {
            HP.setValue(p2score);
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

    public void JumpPadSound()
    {
        JumpPad.start();
    }


    void Update()
    {
        eS = obj.GetComponent<Elevator>().elevatStarted;

        rCT = obj2.GetComponent<FluxManager>().roundCountdownTime;
        wSCT = obj2.GetComponent<FluxManager>().isWinCountDownActive;
        gO = obj2.GetComponent<FluxManager>().gameOver;
        fPlayer = obj2.GetComponent<FluxManager>().fluxPlayer;
        p1score = obj2.GetComponent<FluxManager>().player1score;
        p2score = obj2.GetComponent<FluxManager>().player2score;
        p1 = obj2.GetComponent<FluxManager>().player1;
        p2 = obj2.GetComponent<FluxManager>().player2;

        if (eS == true)
        {
            MainGame.setValue(1);
        }

        if (eS == true && hasGameStarted == false)
        {
            StartOst();
            hasGameStarted = true;
        }

        if (rCT <= endGameMusicStart)
        {
            EndGame.setValue(1);
        }

        if (wSCT == true && oldWSCT != true)
        {
            EndGame.setValue(1);
            oldWSCT = true;
        }
        if (wSCT == false && oldWSCT != false)
        {
            EndGame.setValue(0);
            oldWSCT = false;
        }

        if (gO == true)
        {
            GameOver.setValue(1);
            AV.setValue(0);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            FluxEffect.start();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckPointSound();
        }
    }
	
	void OnDestroy ()
	{
		OST_2.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		Atmosphere.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        OST_Intro.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
	}
}
