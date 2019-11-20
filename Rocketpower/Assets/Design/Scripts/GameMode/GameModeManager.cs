using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject fluxPlayer;

    public int orangeScore;
    public int blueScore;

    public Text textOrange;
    public Text textBlue;

    GameObject[] platformArray;

    List<GameObject> platformList;

    public Material m_orangePlatform;
    public Material m_bluePlatform;

    void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            fluxPlayer = player1;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            fluxPlayer = player2;
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            updateScore();
        }
    }

    public void updateScore() {
        orangeScore=0;
        blueScore=0;

        for(int i =0; i < platformArray.Length; i++){
            if(platformArray[i].gameObject.GetComponent<Renderer>().material.name == "M_PlatformOrange_v1 (Instance)"){
                orangeScore++;
            }
            if(platformArray[i].gameObject.GetComponent<Renderer>().material.name == "M_PlatformBlue_v1 (Instance)") {
                blueScore++;
            }
            
            Debug.Log(orangeScore);
        }
        textOrange.text = orangeScore.ToString();
        textBlue.text = blueScore.ToString();
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
