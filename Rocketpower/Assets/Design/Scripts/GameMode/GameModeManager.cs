using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public int orangeScore;
    public int blueScore;

    GameObject[] platformArray;

    List<GameObject> platformList;

    [HideInInspector] public bool isFluxActive;

    public Material m_orangePlatform;
    public Material m_bluePlatform;

    void Awake()
    {
        platformArray = GameObject.FindGameObjectsWithTag("ColorPlatform");
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F)) {
            updateScore();
        }
    }

    void updateScore() {
        foreach (GameObject platform in platformArray) {
            if(platform.gameObject.GetComponent<Renderer>().material.name == "M_PlatformOrange_v1 (Instance)") {
                List<GameObject> orangeList = new List<GameObject>();
                Debug.Log(platform);
            }
        }

        //List<GameObject> orangeList = new List<GameObject>();

        //for (int i = 0; i < platformArray.Length; i++) {
        //    orangeList.Add(platform);
        //}
        //orangeScore = orangeList.Count;
        //Debug.Log(orangeScore);
    }
}
