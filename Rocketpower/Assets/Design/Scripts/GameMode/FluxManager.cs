using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class FluxManager : MonoBehaviour
{
    #region Public Script References
    [HideInInspector] public GameObject UI;
    [HideInInspector] public GameObject canvas_D1, canvas_D2;
    [HideInInspector] public StateMachine smP1;
    [HideInInspector] public StateMachine smP2;
    [HideInInspector] public PlayerFlux player1;
    [HideInInspector] public PlayerFlux player2;
    [HideInInspector] public GameObject preRoundD1, preRoundD2;
    [HideInInspector] public VirtualController vControllerP1, vControllerP2;
    public Elevator elevatorP1, elevatorP2;
    #endregion

    #region Player Variables
    public Color player1color;
    public Color player2color;
    public float mSpeedDiff = 0.3f;
    public float mSpeedBase = 9.5f;
    #endregion

    [Header("GameMode")]
    #region Flux Player + Score
    public PlayerFlux fluxPlayer;
	private bool sparkIsBeingHeld = false;
    public bool gameOver;
    [HideInInspector] public SparkVFX sparkVFX;
    public int player1score, player2score;
    private int scoreDiff;
    private int pastPlayer1Score, pastPlayer2Score;
    [HideInInspector] public Text textFluxP1, textFluxP2;
    #endregion
    #region Flux Capture
    public GameObject jumpPadOrange, jumpPadBlue;
    public float fluxCaptureTime;
    public float fluxCaptureCD;
    [HideInInspector] public bool isFluxPlayerColliderOnCD;
    [HideInInspector] public Slider sliderCaptureTime;
    [HideInInspector] public Image sliderFillImage;
    [HideInInspector] public GameObject sliderCaptureObject;
    #endregion
    #region Pre-Round Variables
    public bool readyP1, readyP2;
    public bool bothPlayersReady;
    private int startCountdownTime = 3;
    #endregion
    #region Pre-Round Objects
    [HideInInspector] public GameObject startCountdownObjectD1, startCountdownObjectD2;
    [HideInInspector] public Text startCountdownTextD1, startCountdownTextD2;
    public GameObject readyChecksD1, readyChecksD2;
    public GameObject stasisP1, stasisP2;
    public Toggle readyToggleD1_P1, readyToggleD1_P2;
    public Toggle readyToggleD2_P1, readyToggleD2_P2;
    #endregion
    #region In-Round
    public int roundCountdownTime = 120;
    [HideInInspector] public bool isStartRoundTimer = true;
    [HideInInspector] public bool isGameRoundTimerRunning;
    [HideInInspector] public Text roundCountdownText_D1, roundCountdownText_D2;
    [HideInInspector] public GameObject roundCountdownObject_D1, roundCountdownObject_D2;
    [HideInInspector] public GameObject inRoundUI_D1, inRoundUI_D2;
    public GameObject rings;
    #endregion

    [Header("WinCondition")]
    #region WinCondition
    private bool stopWinCountDown;
    private IEnumerator coroutineWinTimer;
    [HideInInspector] public bool isWinCountDownActive;
    public int winStartCountdownTime = 20;
    private int winCountDownTime;
    [HideInInspector] public Text playerWinningTextD1, playerWinningTextD2;
    [HideInInspector] public GameObject playerWinningD1, playerWinningD2;
    [HideInInspector] public GameObject winCountDownD1, winCountDownD2;
    [HideInInspector] public Text winCountDownTextD1, winCountDownTextD2;
    public GameObject overtimeObjD1, overtimeObjD2;
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
    public CanvasGroup cg_PreRoundP1, cg_InRoundP1;
    public CanvasGroup cg_PreRoundP2, cg_InRoundP2;
    #endregion

    private void Awake()
    {
        UI.SetActive(true);
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        //StartCoroutine(StartRoundCountDown());
        Time.timeScale = 1;

        sliderCaptureTime.maxValue = fluxCaptureTime;
        isWinCountDownActive = false;
        smP1.maxSpeed = mSpeedBase;
        smP2.maxSpeed = mSpeedBase;
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
            rt.localPosition = new Vector3(-.5f * platformsTotal * scoreHexagonSize + scoreHexagonSize * (i + .5f) * .85f, 400, 0);
            rt.sizeDelta = new Vector2(scoreHexagonSize, scoreHexagonSize);
            hexagonList.Add(hex.GetComponent<ScoreUI>());
        }
        GameObject newArrow = Instantiate(scoreArrowPrefab);
        newArrow.transform.SetParent(parentCanvas);
        newArrow.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
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
            textFluxP1.text = "Spark: " + fluxPlayer.ToString();
            textFluxP2.text = "Spark: " + fluxPlayer.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            fluxPlayer = player2;
            textFluxP1.text = "Spark: " + fluxPlayer.ToString();
            textFluxP2.text = "Spark: " + fluxPlayer.ToString();
        }
        startRound();

        //Debug.Log(isWinCountDownActive);
    }

    public void SpeedPlayers()
    {
        scoreDiff = player1score - player2score;
        //Debug.Log("P1: " + smP1.maxSpeed);
        //Debug.Log("P2: " + smP2.maxSpeed);

        if (scoreDiff == 0 || scoreDiff == 0)
        {
            smP1.maxSpeed = mSpeedBase;
            smP2.maxSpeed = mSpeedBase;
        }

        if (fluxPlayer == player1)
        {
            if (player1score > player2score)
            {
                if (scoreDiff == 1)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff;
                }
                if (scoreDiff == 2)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 2;
                }
                if (scoreDiff == 3)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 3;
                }
                if (scoreDiff == 4)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 4;
                }
                if (scoreDiff == 5)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 5;
                }
                if (scoreDiff == 6)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 6;
                }
            }
            /*else
            {
                if (scoreDiff > 0 && scoreDiff < 2)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff > 2 && scoreDiff < 4)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 1.2f;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff >= 4)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 1.4f;
                    smP2.maxSpeed = mSpeedBase;
                }
            }*/

        }

        if (fluxPlayer == player2)
        {
            if (player1score < player2score)
            {
                if (scoreDiff == 1)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff == 2)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 2;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff == 3)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 3;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff == 4)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 4;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff == 5)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 5;
                    smP2.maxSpeed = mSpeedBase;
                }
                if (scoreDiff == 6)
                {
                    smP1.maxSpeed = mSpeedBase + mSpeedDiff * 6;
                    smP2.maxSpeed = mSpeedBase;
                }
            }
            /*else
            {
                if (scoreDiff > 0 && scoreDiff < 2)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff;
                }
                if (scoreDiff > 2 && scoreDiff < 4)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 1.2f;
                }
                if (scoreDiff >= 4)
                {
                    smP1.maxSpeed = mSpeedBase;
                    smP2.maxSpeed = mSpeedBase + mSpeedDiff * 1.4f;
                }
            }*/

        }
    }

    public void ColorSwitch()
    {
        //Change Capture Bar Fill Color + Runspeed
        if (fluxPlayer == player1)
        {
            sliderFillImage.color = player2color;
        }

        if (fluxPlayer == player2)
        {
            sliderFillImage.color = player1color;
        }

		if (sparkIsBeingHeld) {
			player1.TurnFlux(fluxPlayer == player1);
			player2.TurnFlux(fluxPlayer == player2);
		}
		else {
			fluxPlayer.TurnFlux(true);
			sparkIsBeingHeld = true;
		}

        if (sparkVFX != null)
        {
            ToggleUI(jumpPadOrange, jumpPadBlue, false);
            Color c;
            if (fluxPlayer == player1)
            {
                c = player1color;
            }
            else
            {
                c = player2color;
            }
            sparkVFX.SetPlayerToFollow(fluxPlayer.transform, c);
        }

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
        if (winCountDownTime >= 0 && isWinCountDownActive)
        {
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            readyP1 = true;
            readyToggleD1_P1.isOn = true;
            readyToggleD2_P1.isOn = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            readyP2 = true;
            readyToggleD1_P2.isOn = true;
            readyToggleD2_P2.isOn = true;
        }
        if (readyP1 && readyP2)
        {
            bothPlayersReady = true;
        }

        if (elevatorP1.isElevatorFinished && elevatorP2.isElevatorFinished && isStartRoundTimer)
        {
            ToggleUI(readyChecksD1, readyChecksD2, false);
            isStartRoundTimer = false;
            StartCoroutine(StartRoundCountdown());
        }
    }

    IEnumerator StartRoundCountdown()
    {
        rings.gameObject.SetActive(true);
        yield return new WaitForSeconds(4.5f);
        ToggleUI(startCountdownObjectD1, startCountdownObjectD2, true);
        ToggleUIText(startCountdownTextD1, startCountdownTextD2, startCountdownTime);

        while (startCountdownTime > 0)
        {
            yield return new WaitForSeconds(0.8f);
            startCountdownTime--;
            ToggleUIText(startCountdownTextD1, startCountdownTextD2, startCountdownTime);

        }
        stasisP1.gameObject.SetActive(false);
        stasisP2.gameObject.SetActive(false);
        startCountdownTextD1.text = "GO!";
        startCountdownTextD2.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        isGameRoundTimerRunning = true;
        ToggleUI(startCountdownObjectD1, startCountdownObjectD2, false);
        ToggleUIText(roundCountdownText_D1, roundCountdownText_D2, roundCountdownTime);
        ToggleUI(inRoundUI_D1, inRoundUI_D2, true);

        StartCoroutine(FadeCanvasGroup(cg_InRoundP1, cg_InRoundP1.alpha, 1, 1.5f));
        StartCoroutine(FadeCanvasGroup(cg_InRoundP2, cg_InRoundP2.alpha, 1, 1.5f));

        StartCoroutine(GameRoundCountdown());
    }

    IEnumerator GameRoundCountdown()
    {
        while (roundCountdownTime > 0 && isGameRoundTimerRunning)
        {
            yield return new WaitForSeconds(1);
            roundCountdownTime--;
            ToggleUIText(roundCountdownText_D1, roundCountdownText_D2, roundCountdownTime);
        }
        if (roundCountdownTime <= 0 && player1score == player2score)
        {
            StartCoroutine(Overtime());
        }
        else if (roundCountdownTime <= 0 && (player1score > player2score || player2score < player1score))
        {
            isGameRoundTimerRunning = false;
            gameOver = true;
            WinScreen();
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator winCountDown()
    {
        if (player1score > player2score)
        {
            playerWinningTextD1.text = "Green Holds All Holofields";
            playerWinningTextD2.text = "Green Holds All Holofields";
        }
        else if (player2score > player1score)
        {
            playerWinningTextD1.text = "Blue Holds All Holofields";
            playerWinningTextD2.text = "Blue Holds All Holofields";
        }

        while (winCountDownTime >= 0 && isWinCountDownActive && !stopWinCountDown)
        {
            yield return new WaitForSeconds(1);
            winCountDownTime--;
            ToggleUIText(winCountDownTextD1, winCountDownTextD2, winCountDownTime);
        }
        if (winCountDownTime < 1 && !stopWinCountDown)
        {
            Time.timeScale = 0;
            gameOver = true;
            WinScreen();
        }
        yield return new WaitForEndOfFrame();
        //yield return new WaitWhile(() => winCountDownTime >= 0);
    }

    IEnumerator Overtime()
    {
        ToggleUI(roundCountdownObject_D1, roundCountdownObject_D2, false);
        ToggleUI(overtimeObjD1, overtimeObjD2, true);
        while (player1score == player2score)
        {
            yield return null;
        }
        Time.timeScale = 0;
        gameOver = true;
        WinScreen();
        yield return new WaitForEndOfFrame();
    }
    public void WinCondition()
    {
        if (player1score == platformsTotal || player2score == platformsTotal && !isWinCountDownActive)
        {
            winCountDownTime = winStartCountdownTime;
            isWinCountDownActive = true;
            stopWinCountDown = false;
            isGameRoundTimerRunning = false;
            StartCoroutine(winCountDown());
            ToggleUIText(winCountDownTextD1, winCountDownTextD2, winStartCountdownTime);
            ToggleUI(winCountDownD1, winCountDownD2, true);
            ToggleUI(playerWinningD1, playerWinningD2, true);
            ToggleUI(roundCountdownObject_D1, roundCountdownObject_D2, false);

        }
        else if (isWinCountDownActive)
        {
            isWinCountDownActive = false;
            isGameRoundTimerRunning = true;
            ToggleUI(winCountDownD1, winCountDownD2, false);
            ToggleUI(playerWinningD1, playerWinningD2, false);
            StartCoroutine(GameRoundCountdown());
            ToggleUI(roundCountdownObject_D1, roundCountdownObject_D2, true);
        }
    }


    public void WinScreen()
    {
        ToggleUI(inRoundUI_D1, inRoundUI_D2, false);
        ToggleUI(endScreenD1, endScreenD2, true);

        if (player1score > player2score)
        {
            endTextD1.text = "You Win";
            endTextD2.text = "You Lose";
        }
        if (player2score > player1score)
        {
            endTextD1.text = "You Lose";
            endTextD2.text = "You Win";
        }
        if (player1score == player2score)
        {
            endTextD1.text = "Draw";
            endTextD2.text = "Draw";
        }
    }

    public IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float lerpTime)
    {
        canvasGroup.alpha = 0;
        float timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentComplete = timeSinceStarted / lerpTime;

        while (true)
        {
            timeSinceStarted = Time.time - timeStartedLerping;
            percentComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentComplete);

            canvasGroup.alpha = currentValue;

            if (percentComplete >= 1)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        print("Done");
    }

    public void ToggleUI(GameObject d1, GameObject d2, bool turnOn)
    {
        d1.SetActive(turnOn);
        d2.SetActive(turnOn);
    }

    private void ToggleUIText(Text d1, Text d2, int integer)
    {
        d1.text = integer.ToString();
        d2.text = integer.ToString();
    }

    IEnumerator FluxColliderSeconds()
    {
        //Flux Capture Cooldown
        isFluxPlayerColliderOnCD = true;
        yield return new WaitForSeconds(fluxCaptureCD);
        isFluxPlayerColliderOnCD = false;
    }
}