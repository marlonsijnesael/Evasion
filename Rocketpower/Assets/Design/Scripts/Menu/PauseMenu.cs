using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public FluxManager gm;

    public VirtualController vCP1, vCP2;

    public GameObject pauseMenuD1, pauseMenuD2, pauseMenuD3;

    public bool isPauseMenuActive;

    public EventSystem ES;
    private GameObject StoreSelected;

    GameObject audioObj;

    void Start()
    {
        audioObj = GameObject.FindGameObjectWithTag("audio");
    }

    void Update()
    {
        if (ES.currentSelectedGameObject != StoreSelected)
        {
            if (ES.currentSelectedGameObject == null)
            {
                ES.SetSelectedGameObject(StoreSelected);
            }
            else
            {
                StoreSelected = ES.currentSelectedGameObject;
            }
        }

        if (Input.GetKeyDown(KeyCode.O) || vCP1.StartPressed || vCP2.StartPressed)
        {
            isPauseMenuActive = !isPauseMenuActive;
            if (isPauseMenuActive)
            {
                PauseGame();
            }
            else if (!isPauseMenuActive)
            {
                ResumeGame();
                print("resumed");

            }
        }
    }

    public void PauseGame()
    {
        if (gm.isStartRoundTimer)
        {
            gm.ToggleUI(gm.preRoundD1, gm.preRoundD2, false);
            gm.ToggleUI3(pauseMenuD1, pauseMenuD2, pauseMenuD3, true);
            Time.timeScale = 0;
            StoreSelected = ES.firstSelectedGameObject;
        }
        else if (!gm.isStartRoundTimer)
        {
            gm.ToggleUI(gm.inRoundUI_D1, gm.inRoundUI_D2, false);
            gm.ToggleUI3(pauseMenuD1, pauseMenuD2, pauseMenuD3, true);
            gm.isGameRoundTimerRunning = false;
            Time.timeScale = 0;
            StoreSelected = ES.firstSelectedGameObject;
        }
        audioObj.GetComponent<GeneralAudio>().OV.setValue(0.6f);

    }

    public void ResumeGame()
    {
        isPauseMenuActive = false;
        if (gm.isStartRoundTimer)
        {
            gm.ToggleUI(gm.preRoundD1, gm.preRoundD2, true);
            gm.ToggleUI3(pauseMenuD1, pauseMenuD2, pauseMenuD3, false);
            Time.timeScale = 1;
        }
        else if (!gm.isStartRoundTimer)
        {
            gm.ToggleUI(gm.inRoundUI_D1, gm.inRoundUI_D2, true);
            gm.ToggleUI3(pauseMenuD1, pauseMenuD2, pauseMenuD3, false);
            gm.isGameRoundTimerRunning = true;
            Time.timeScale = 1;
        }
        audioObj.GetComponent<GeneralAudio>().OV.setValue(0.9f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeGame();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}