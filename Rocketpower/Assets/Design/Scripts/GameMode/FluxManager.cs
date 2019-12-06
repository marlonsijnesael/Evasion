using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class FluxManager : MonoBehaviour
{
    #region Public Script References
    [HideInInspector] public GameObject canvas;
    [HideInInspector] public StateMachine smP1;
    [HideInInspector] public StateMachine smP2;
    [HideInInspector] public PlayerFlux player1;
    [HideInInspector] public PlayerFlux player2;
    #endregion
    public Color player1color;
    public Color player2color;

    #region Flux Player + Score
    public PlayerFlux fluxPlayer;
    private int player1score;
    private int player2score;
    [HideInInspector] public Text textP1;
    [HideInInspector] public Text textP2;
    [HideInInspector] public Text textFluxPlayer;
    #endregion

    #region Flux Capture
    public float fluxCaptureTime;
    public bool isFluxPlayerColliderOnCD;
    public Slider sliderCaptureTime;
    [HideInInspector] public Image sliderFillImage;
    [HideInInspector] public GameObject sliderCaptureObject;
    #endregion

    #region Pre-Round Variables
    public bool readyP1;
    public bool readyP2;
    private int startCountdownTime = 5;
    #endregion

    #region Pre-Round Objects
    public GameObject startCountdownObject;
    public Text startCountdownText;
    public GameObject readyChecks;
    public GameObject stasisP1;
    public GameObject stasisP2;
    public Toggle readyToggleP1;
    public Toggle readyToggleP2;
    #endregion

    #region In-Round
    private int roundCountdownTime = 120;
    [HideInInspector] public Text roundCountdownText;
    [HideInInspector] public GameObject inRoundUI;
    #endregion

    GameObject[] platformArray;

    private void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        canvas.gameObject.SetActive(true);
        sliderCaptureTime.maxValue = fluxCaptureTime;
        //StartCoroutine(StartRoundCountDown());
    }

    private void Update()
    {
        //Press 1 or 2 to change flux
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fluxPlayer = player1;
            textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fluxPlayer = player2;
            textFluxPlayer.text = "Flux: " + fluxPlayer.ToString();
        }
        startRound();
    }

    public void ColorandSpeedSwitch()
    {
        //Change Capture Bar Fill Color + Runspeed
        if (fluxPlayer == player1)
        {
            sliderFillImage.color = player2color;
            smP1.maxSpeed = 12;
        }
        else
        {
            smP1.maxSpeed = 14;
        }
        if (fluxPlayer == player2)
        {
            sliderFillImage.color = player1color;
            smP2.maxSpeed = 12;
        }
        else
        {
            smP2.maxSpeed = 14;
        }
    }

    public void updateScore()
    {
        player1score = 0;
        player2score = 0;

        foreach (GameObject platform in platformArray)
        {
            PlatformState state = platform.GetComponent<PlatformState>();
            if (state.GetPlayerID() == 1)
            {
                player1score++;
            }
            else if (state.GetPlayerID() == 2)
            {
                player2score++;
            }
        }
        textP1.text = player1score.ToString();
        textP2.text = player2score.ToString();
    }

    public void startRound()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            readyP1 = true;
            readyToggleP1.isOn = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            readyP2 = true;
            readyToggleP2.isOn = true;
        }

        if (readyToggleP1.isOn && readyToggleP2.isOn)
        {
            startCountdownObject.SetActive(true);
            startCountdownText.text = startCountdownTime.ToString();
            StartCoroutine(StartRoundCountdown());
            readyToggleP1.isOn = false;
            readyToggleP2.isOn = false;
            readyChecks.SetActive(false);
        }
    }

    IEnumerator StartRoundCountdown()
    {
        while (startCountdownTime > 0)
        {
            yield return new WaitForSeconds(1);
            startCountdownTime--;
            startCountdownText.text = startCountdownTime.ToString();
        }
        stasisP1.gameObject.SetActive(false);
        stasisP2.gameObject.SetActive(false);
        startCountdownObject.SetActive(false);
        inRoundUI.SetActive(true);
        roundCountdownText.text = roundCountdownTime.ToString();
        StartCoroutine(GameRoundCountdown());
    }

    IEnumerator GameRoundCountdown()
    {
        while (roundCountdownTime > 0)
        {
            yield return new WaitForSeconds(1);
            roundCountdownTime--;
            roundCountdownText.text = roundCountdownTime.ToString();
        }
        Time.timeScale = 0;
    }

    IEnumerator FluxColliderSeconds()
    {
        //Flux Capture Cooldown
        isFluxPlayerColliderOnCD = true;
        yield return new WaitForSeconds(1.5f);
        isFluxPlayerColliderOnCD = false;
    }
}
