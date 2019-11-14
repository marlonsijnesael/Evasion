using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{

    public GameObject player1Current;
    public GameObject player2Current;
    public GameObject player1Base;
    public GameObject player2Base;
    public GameObject fluxPlayer;

    public int orangeScore;
    public int blueScore;

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
            player1Current = fluxPlayer;
            player2Current = player2Base;
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            player2Current = fluxPlayer;
            player1Current = player1Base;
        }
    }

    public void updateScore() {
        foreach (GameObject platform in platformArray) {
            orangeScore=0;
            blueScore=0;
            if(platform.gameObject.GetComponent<Renderer>().material.name == "M_PlatformOrange_v1 (Instance)") {
                Debug.Log(platform);
                List<GameObject> orangeList = new List<GameObject>();
                orangeList.Add(platform);
                for(int i = 0; i < orangeList.Count; i++){
                    orangeScore++;
                }
                Debug.Log(orangeScore);
            }
            if(platform.gameObject.GetComponent<Renderer>().material.name == "M_PlatformBlue_v1 (Instance)") {
                Debug.Log(platform);
                List<GameObject> blueList = new List<GameObject>();
            }
        }

        //List<GameObject> orangeList = new List<GameObject>();

        //for (int i = 0; i < platformArray.Length; i++) {
        //    orangeList.Add(platform);
        //}
        //orangeScore = orangeList.Count;
        //Debug.Log(orangeScore);
    }

    private void switchColor(){
        if(player1Current == fluxPlayer){
            player2Current = fluxPlayer;
            player1Current = player1Base;

        }
        if(player2Current == fluxPlayer){
            player1Current = fluxPlayer;
            player2Current = player2Base;
        }
    }
}
