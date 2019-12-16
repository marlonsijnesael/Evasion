using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
public class FluxManager : MonoBehaviour
{
    #region Public Script References
    [HideInInspector] public GameObject canvas_D1, canvas_D2;
    [HideInInspector] public StateMachine smP1;
    [HideInInspector] public StateMachine smP2;
    [HideInInspector] public PlayerFlux player1;
    [HideInInspector] public PlayerFlux player2;
    [HideInInspector] public VirtualController vControllerP1, vControllerP2;
    #endregion
    
    #region Player Variables
    public Color player1color;
    public Color player2color;
    public int mSpeed_It, mSpeed_notIt;
    #endregion

    [Header("GameMode")]
    #region Flux Player + Score
    public PlayerFlux fluxPlayer;
	public SparkVFX sparkVFX;
    private int player1score;
    private int player2score;
    private int pastPlayer1Score;
    private int pastPlayer2Score;
    [HideInInspector] public Text textFluxP1, textFluxP2;
    #endregion
    #region Flux Capture
    public float fluxCaptureTime;
    [HideInInspector] public bool isFluxPlayerColliderOnCD;
    [HideInInspector] public Slider sliderCaptureTime;
    [HideInInspector] public Image sliderFillImage;
    [HideInInspector] public GameObject sliderCaptureObject;
    #endregion
    #region Pre-Round Variables
    [HideInInspector] public bool readyP1;
    [HideInInspector] public bool readyP2;
    private int startCountdownTime = 5;
    #endregion
    #region Pre-Round Objects
    [HideInInspector] public GameObject startCountdownObjectD1, startCountdownObjectD2;
    [HideInInspector] public Text startCountdownTextD1, startCountdownTextD2;
    [HideInInspector] public GameObject readyChecksD1, readyChecksD2;
    [HideInInspector] public GameObject stasisP1;
    [HideInInspector] public GameObject stasisP2;
    [HideInInspector] public Toggle readyToggleP1;
    [HideInInspector] public Toggle readyToggleP2;
    #endregion
    #region In-Round
    public int roundCountdownTime = 120;
    private bool isStartRoundTimer = true;
    [HideInInspector] public Text roundCountdownText_D1, roundCountdownText_D2;
    [HideInInspector] public GameObject inRoundUI_D1, inRoundUI_D2;
    #endregion

    [Header("WinCondition")]
    #region WinCondition
    private bool stopWinCountDown;
    private IEnumerator coroutineWinTimer;
    [HideInInspector] public bool isWinCountDownActive = true;
    public int winStartCountdownTime = 20;
    private int winCountDownTime;
    [HideInInspector] public GameObject winCountDownD1, winCountDownD2;
    [HideInInspector] public Text winCountDownTextD1, winCountDownTextD2;
    [HideInInspector] public GameObject endScreenD1, endScreenD2;
    [HideInInspector] public Text endTextD1, endTextD2;
    #endregion

    [Header("UI")]
    #region ScoreHexagons
    public int platformsTotal = 3;
    [HideInInspector] public GameObject scoreHexagonPrefab;
    [HideInInspector] public GameObject scoreArrowPrefab;
    [HideInInspector] public Sprite scoreArrowSprite;
    [HideInInspector] public Sprite scoreArrowArrowSprite;
    public int scoreHexagonSize = 75;
    private List<ScoreUI> scoreHexagonList_D1 = new List<ScoreUI>();
    private List<ScoreUI> scoreHexagonList_D2 = new List<ScoreUI>();
    GameObject[] platformArray;
    [HideInInspector] public GameObject scoreArrow_D1, scoreArrow_D2;
    #endregion

    private void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        canvas_D1.gameObject.SetActive(true);
        canvas_D2.gameObject.SetActive(true);
        sliderCaptureTime.maxValue = fluxCaptureTime;
        //StartCoroutine(StartRoundCountDown());
    }

    private void Start()
    {
        //hex.transform.SetParent(canvas.transform);
        CleanDualScreenTest(canvas_D1.transform, scoreHexagonList_D1, 1);
        CleanDualScreenTest(canvas_D2.transform, scoreHexagonList_D2, 2);
    }

    private void CleanDualScreenTest(Transform parentCanvas, List<ScoreUI> hexagonList, int display)
    {
        for (int i = 0; i < platformsTotal; i++)
        {
            GameObject hex = Instantiate(scoreHexagonPrefab);
            hex.transform.SetParent(parentCanvas);
            hex.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            RectTransform rt = hex.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(-.5f * platformsTotal * scoreHexagonSize + scoreHexagonSize * (i + .5f), 400, 0);
            rt.sizeDelta = new Vector2(scoreHexagonSize, scoreHexagonSize);
            hexagonList.Add(hex.GetComponent<ScoreUI>());
        }
        GameObject newArrow = Instantiate(scoreArrowPrefab);
        newArrow.transform.SetParent(parentCanvas);
        newArrow.transform.localScale = new Vector3(1, 1, 1);
        newArrow.gameObject.SetActive(false);
        newArrow.name = newArrow.name;

        if (display == 1)
            scoreArrow_D1 = newArrow;
        else
            scoreArrow_D2 = newArrow;
    }

    private void Update()
    {
        //Press 1 or 2 to change flux
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            fluxPlayer = player1;
            textFluxP1.text = "Flux: " + fluxPlayer.ToString();
            textFluxP2.text = "Flux: " + fluxPlayer.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fluxPlayer = player2;
            textFluxP1.text = "Flux: " + fluxPlayer.ToString();
            textFluxP2.text = "Flux: " + fluxPlayer.ToString();
        }
        startRound();
    }

    public void ColorandSpeedSwitch()
    {
        //Change Capture Bar Fill Color + Runspeed
        if (fluxPlayer == player1)
        {
            sliderFillImage.color = player2color;
            smP1.maxSpeed = mSpeed_It;
        }
        else
        {
            smP1.maxSpeed = mSpeed_notIt;
        }

        if (fluxPlayer == player2)
        {
            sliderFillImage.color = player1color;
            smP2.maxSpeed = mSpeed_It;
        }
        else
        {
            smP2.maxSpeed = mSpeed_notIt;
        }

        player1.TurnFlux(fluxPlayer == player1);
        player2.TurnFlux(fluxPlayer == player2);
		
		sparkVFX.SetPlayerToFollow(fluxPlayer.transform);

        //set scorearrow position and color
        scoreArrow_D1.SetActive(true);
        scoreArrow_D2.SetActive(true);
        if (fluxPlayer == player1)
        {
            scoreArrow_D1.GetComponent<Image>().color = player1color;
            scoreArrow_D1.transform.localRotation = Quaternion.Euler(0, 0, 0);
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D1[Mathf.Clamp(player1score - 1, 0, platformsTotal - 1)].transform.localPosition;
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D2[Mathf.Clamp(player1score - 1, 0, platformsTotal - 1)].transform.localPosition;

            scoreArrow_D1.GetComponent<Image>().sprite = scoreArrowSprite;
            if (player1score == 0)
            {
                scoreArrow_D1.transform.localPosition = scoreHexagonList_D1[Mathf.Clamp(player1score - 1, 0, platformsTotal - 1)].transform.localPosition - new Vector3(scoreHexagonSize, 0, 0);
                scoreArrow_D1.transform.localPosition = scoreHexagonList_D2[Mathf.Clamp(player1score - 1, 0, platformsTotal - 1)].transform.localPosition - new Vector3(scoreHexagonSize, 0, 0);

                scoreArrow_D1.GetComponent<Image>().sprite = scoreArrowArrowSprite;
            }
        }
        else
        {
            scoreArrow_D1.GetComponent<Image>().color = player2color;
            scoreArrow_D1.transform.localRotation = Quaternion.Euler(0, 0, 180);
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D1[Mathf.Clamp(platformsTotal - player2score, 0, platformsTotal - 1)].transform.localPosition;
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D2[Mathf.Clamp(platformsTotal - player2score, 0, platformsTotal - 1)].transform.localPosition;

            scoreArrow_D1.GetComponent<Image>().sprite = scoreArrowSprite;
            if (player2score == 0)
            {
                scoreArrow_D1.transform.localPosition = scoreHexagonList_D1[Mathf.Clamp(platformsTotal - player2score, 0, platformsTotal - 1)].transform.localPosition + new Vector3(scoreHexagonSize, 0, 0);
                scoreArrow_D1.transform.localPosition = scoreHexagonList_D2[Mathf.Clamp(platformsTotal - player2score, 0, platformsTotal - 1)].transform.localPosition + new Vector3(scoreHexagonSize, 0, 0);

                scoreArrow_D1.GetComponent<Image>().sprite = scoreArrowArrowSprite;
            }
        }
    }

    public void updateScore()
    {
        if(winCountDownTime >= 0 && !isWinCountDownActive){
            stopWinCountDown = true;
        }
        pastPlayer1Score = player1score;
        pastPlayer2Score = player2score;
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
        WinCondition();

        //change score UI
        if (pastPlayer1Score < player1score && pastPlayer2Score == player2score)
        {
            //change white to player 1
            scoreHexagonList_D1[player1score - 1].StartChangingColor(Color.white, player1color);
            scoreHexagonList_D2[player1score - 1].StartChangingColor(Color.white, player1color);

        }
        else if (pastPlayer1Score == player1score && pastPlayer2Score < player2score)
        {
            // change white to player 2
            scoreHexagonList_D1[platformsTotal - player2score].StartChangingColor(Color.white, player2color);
            scoreHexagonList_D2[platformsTotal - player2score].StartChangingColor(Color.white, player2color);

        }
        else if (pastPlayer1Score < player1score && pastPlayer2Score > player2score)
        {
            if (player1score + player2score == platformsTotal)
            {
                //change one from player 2 to player 1
                scoreHexagonList_D1[player1score - 1].StartChangingColor(player2color, player1color);
                scoreHexagonList_D2[player1score - 1].StartChangingColor(player2color, player1color);

            }
            else
            {
                //change a white one to player 1 and a player 2 one to white
                scoreHexagonList_D1[player1score - 1].StartChangingColor(Color.white, player1color);
                scoreHexagonList_D1[platformsTotal - player2score - 1].StartChangingColor(player2color, Color.white);
                scoreHexagonList_D2[player1score - 1].StartChangingColor(Color.white, player1color);
                scoreHexagonList_D2[platformsTotal - player2score - 1].StartChangingColor(player2color, Color.white);
            }
        }
        else if (pastPlayer1Score > player1score && pastPlayer2Score < player2score)
        {
            if (player1score + player2score == platformsTotal)
            {
                //change one from player 1 to player 2
                scoreHexagonList_D1[platformsTotal - player2score].StartChangingColor(player1color, player2color);
                scoreHexagonList_D2[platformsTotal - player2score].StartChangingColor(player1color, player2color);

            }
            else
            {
                //change a white one to player 2 and a player 1 one to white
                scoreHexagonList_D1[platformsTotal - player2score].StartChangingColor(Color.white, player2color);
                scoreHexagonList_D1[player1score].StartChangingColor(player1color, Color.white);
                scoreHexagonList_D2[platformsTotal - player2score].StartChangingColor(Color.white, player2color);
                scoreHexagonList_D2[player1score].StartChangingColor(player1color, Color.white);
            }
        }

        //move the arrow
        if (fluxPlayer == player1)
        {
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D1[Mathf.Clamp(player1score - 1, 0, platformsTotal - 1)].transform.localPosition;
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D2[Mathf.Clamp(player1score - 1, 0, platformsTotal - 1)].transform.localPosition;

            scoreArrow_D1.GetComponent<Image>().sprite = scoreArrowSprite;
        }
        else
        {
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D1[Mathf.Clamp(platformsTotal - player2score, 0, platformsTotal - 1)].transform.localPosition;
            scoreArrow_D1.transform.localPosition = scoreHexagonList_D2[Mathf.Clamp(platformsTotal - player2score, 0, platformsTotal - 1)].transform.localPosition;

            scoreArrow_D1.GetComponent<Image>().sprite = scoreArrowSprite;
        }
    }

    public void startRound()
    {
        if (vControllerP1.JumpButtonPressedThisFrame || Input.GetKeyDown(KeyCode.Q))
        {
            readyP1 = true;
            readyToggleP1.isOn = true;
        }
        if (vControllerP2.JumpButtonPressedThisFrame || Input.GetKeyDown(KeyCode.W))
        {
            readyP2 = true;
            readyToggleP2.isOn = true;
        }
        if (readyToggleP1.isOn && readyToggleP2.isOn && isStartRoundTimer)
        {
            ToggleUI(startCountdownObjectD1, startCountdownObjectD2, true);
            ToggleUI(readyChecksD1, readyChecksD2, false);
            ToggleUIText(startCountdownTextD1, startCountdownTextD2, startCountdownTime);
            isStartRoundTimer = false;

            StartCoroutine(StartRoundCountdown());
        }
    }

    IEnumerator StartRoundCountdown()
    {
        while (startCountdownTime > 0)
        {
            yield return new WaitForSeconds(1);
            startCountdownTime--;
            ToggleUIText(startCountdownTextD1, startCountdownTextD2, startCountdownTime);

        }
        stasisP1.gameObject.SetActive(false);
        stasisP2.gameObject.SetActive(false);
        ToggleUI(startCountdownObjectD1, startCountdownObjectD2, false);
        ToggleUI(inRoundUI_D1, inRoundUI_D2, true);
        ToggleUIText(roundCountdownText_D1, roundCountdownText_D2, roundCountdownTime);

        StartCoroutine(GameRoundCountdown());
    }

    IEnumerator GameRoundCountdown()
    {
        while (roundCountdownTime > 0)
        {
            yield return new WaitForSeconds(1);
            roundCountdownTime--;
            ToggleUIText(roundCountdownText_D1, roundCountdownText_D2, roundCountdownTime);
        }
        Time.timeScale = 0;
        WinScreen();
    }

public void WinCondition(){
        coroutineWinTimer = winCountDown();
        if(player1score == platformsTotal || player2score == platformsTotal && isWinCountDownActive){
            winCountDownTime = winStartCountdownTime;
            isWinCountDownActive = false;
            stopWinCountDown = false;
            StartCoroutine(winCountDown());
            ToggleUIText(winCountDownTextD1, winCountDownTextD2, winStartCountdownTime);
            ToggleUI(winCountDownD1, winCountDownD2, true);
        }
        else if(!isWinCountDownActive){
            isWinCountDownActive = true;
            ToggleUI(winCountDownD1, winCountDownD2, false);
        }
    }

    IEnumerator winCountDown(){
        while(winCountDownTime >= 0 && !isWinCountDownActive && !stopWinCountDown){
            yield return new WaitForSeconds(1);
            winCountDownTime--;
            ToggleUIText(winCountDownTextD1, winCountDownTextD2, winCountDownTime);
        }
        if(winCountDownTime <= 0 && !stopWinCountDown){
            Debug.Log("End Me");
            Time.timeScale = 0;
            WinScreen();
        }
        yield return new WaitForEndOfFrame();
        //yield return new WaitWhile(() => winCountDownTime >= 0);
    }

    public void WinScreen(){
        ToggleUI(inRoundUI_D1, inRoundUI_D2, false);
        ToggleUI(endScreenD1, endScreenD2, true);

        if(player1score > player2score){
            endTextD1.text = "Winner";
            endTextD2.text = "Loser";
        }
        if(player2score > player1score){
            endTextD1.text = "Loser";
            endTextD2.text = "Winner";
        }
        if(player1score == player2score){
            endTextD1.text = "Draw";
            endTextD2.text = "Draw";
        }
    }

    private void ToggleUI(GameObject d1, GameObject d2, bool turnOn){
        d1.SetActive(turnOn);
        d2.SetActive(turnOn);
    }

    private void ToggleUIText(Text d1, Text d2, int integer){
        d1.text = integer.ToString();
        d2.text = integer.ToString();
    }

    IEnumerator FluxColliderSeconds()
    {
        //Flux Capture Cooldown
        isFluxPlayerColliderOnCD = true;
        yield return new WaitForSeconds(1.5f);
        isFluxPlayerColliderOnCD = false;
    }
}

