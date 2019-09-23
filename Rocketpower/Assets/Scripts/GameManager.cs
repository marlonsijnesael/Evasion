using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool gamePauzed = false;

    [Header("UI holders")]
    [SerializeField] private GameObject uI_Disconnect;
    [SerializeField] private GameObject uI_Pauze;

    private GameObject activeUI, previousActiveUI;

    private void OnEnable()
    {
       MonitorControllers.OnConnect += PauzeGame;
    }

    private void OnDisable()
    {
        MonitorControllers.OnConnect -= PauzeGame;
    }


    public void PauzeGame(string _popUp, bool _setActive, float _timeScale)
    {
        Time.timeScale = _timeScale;
        switch (_popUp)
        {
            case "connect":
                uI_Disconnect.SetActive(_setActive);
                break;


            case "regularPauze":
               //open pauze menu
                break;
        }
    }

}
