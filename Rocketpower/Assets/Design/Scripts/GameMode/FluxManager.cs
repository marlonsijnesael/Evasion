using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class FluxManager : MonoBehaviour
{
    
	[Header("Players")]
	public Color player1color;
	public Color player2color;
	[HideInInspector] public PlayerFlux player1;
    [HideInInspector] public PlayerFlux player2;
    public int fluxCaptureTime;
    public PlayerFlux fluxPlayer;
	
	[Header("UI")]
	public int platformsTotal = 3;
	public GameObject scoreHexagonPrefab;
	public GameObject scoreArrowPrefab;
	public int scoreHexagonSize = 75;
    public Text textP1;
    public Text textP2;
    public Text textFluxPlayer;
    public Slider sliderCaptureTime;
    public Image sliderFillImage;
    public GameObject sliderCaptureObject;
    public GameObject canvas;

    GameObject[] platformArray;

    public bool isFluxPlayerColliderOnCD;
	
    private int player1score;
    private int player2score;
	private int pastPlayer1Score;
	private int pastPlayer2Score;
	
	private List<ScoreUI> scoreHexagonList = new List<ScoreUI>();
	private Transform scoreArrow;

    private void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
        canvas.gameObject.SetActive(true);
        sliderCaptureTime.maxValue = fluxCaptureTime;
    }
	
	private void Start(){
		for (int i = 0; i < platformsTotal; i++){
			GameObject hex = Instantiate(scoreHexagonPrefab);
			hex.transform.SetParent(canvas.transform);
			hex.GetComponent<RectTransform>().localPosition = new Vector3(-400 + 800 / (platformsTotal - 1) * i, 400, 0);
			scoreHexagonList.Add(hex.GetComponent<ScoreUI>());
		}
		scoreArrow = Instantiate(scoreArrowPrefab).transform;
		scoreArrow.transform.SetParent(canvas.transform);
		scoreArrow.gameObject.SetActive(false);
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
		
		//set scorearrow position and color
		scoreArrow.gameObject.SetActive(true);
		if (fluxPlayer == player1){
			scoreArrow.GetComponent<Image>().color = player1color;
			scoreArrow.localPosition = scoreHexagonList[Mathf.Clamp(player1score, 0, platformsTotal - 1)].transform.localPosition - new Vector3(800 / (platformsTotal - 1), 0, 0);
		}
		else{
			scoreArrow.GetComponent<Image>().color = player2color;
			scoreArrow.localPosition = scoreHexagonList[Mathf.Clamp(platformsTotal - player2score - 1, 0, platformsTotal - 1)].transform.localPosition + new Vector3(800 / (platformsTotal - 1), 0, 0);
		}
		
	}

    public void updateScore() {
		pastPlayer1Score = player1score;
		pastPlayer2Score = player2score;
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
		
		if (pastPlayer1Score < player1score && pastPlayer2Score == player2score){
			//change white to player 1
			scoreHexagonList[player1score - 1].StartChangingColor(Color.white, player1color);
		}
		else if (pastPlayer1Score == player1score && pastPlayer2Score < player2score){
			// change white to player 2
			scoreHexagonList[platformsTotal - player2score].StartChangingColor(Color.white, player2color);
		}
		else if (pastPlayer1Score < player1score && pastPlayer2Score > player2score){
			if (player1score + player2score == platformsTotal){
				//change one from player 2 to player 1
				scoreHexagonList[player1score - 1].StartChangingColor(player2color, player1color);
			}
			else{
				//change a white one to player 1 and a player 2 one to white
				scoreHexagonList[player1score - 1].StartChangingColor(Color.white, player1color);
				scoreHexagonList[platformsTotal - player2score - 1].StartChangingColor(player2color, Color.white);
			}
		}
		else if (pastPlayer1Score > player1score && pastPlayer2Score < player2score){
			if (player1score + player2score == platformsTotal){
				//change one from player 1 to player 2
				scoreHexagonList[platformsTotal - player2score].StartChangingColor(player1color, player2color);
			}
			else{
				//change a white one to player 2 and a player 1 one to white
				scoreHexagonList[platformsTotal - player2score].StartChangingColor(Color.white, player2color);
				scoreHexagonList[player1score].StartChangingColor(player1color, Color.white);
			}
		}
		
		//move the arrow
		if (fluxPlayer == player1){
			scoreArrow.GetComponent<Image>().color = player1color;
			scoreArrow.localPosition = scoreHexagonList[Mathf.Clamp(player1score, 0, platformsTotal - 1)].transform.localPosition - new Vector3(800 / (platformsTotal - 1), 0, 0);
		}
		else{
			scoreArrow.GetComponent<Image>().color = player2color;
			scoreArrow.localPosition = scoreHexagonList[Mathf.Clamp(platformsTotal - player2score - 1, 0, platformsTotal - 1)].transform.localPosition + new Vector3(800 / (platformsTotal - 1), 0, 0);
		}
		
		
		
		if (player1score == platformsTotal) {
			//player 1 wins
			Debug.Log("player 1 wins");
		}
		else if (player2score == platformsTotal){
			//player 2 wins
			Debug.Log("player 2 wins");
		}
		
    }

    IEnumerator FluxColliderSeconds(){
		Debug.Log("Coroutine Start");
        isFluxPlayerColliderOnCD = true;
		yield return new WaitForSeconds(1.5f);
        isFluxPlayerColliderOnCD = false;
		Debug.Log("Coroutine Finish");
	}
}
