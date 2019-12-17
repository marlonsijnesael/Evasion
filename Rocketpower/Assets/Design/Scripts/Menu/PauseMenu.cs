using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public FluxManager gm;

    public GameObject pauseMenuP1, pauseMenuP2;

    public bool isPauseMenuActive;

    public EventSystem ES;
    private GameObject StoreSelected;

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            isPauseMenuActive = !isPauseMenuActive;
            if (isPauseMenuActive)
            {
                PauseGame();
            }
            else if (!isPauseMenuActive)
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        if (!gm.isGameRoundTimerRunning)
        {
            gm.ToggleUI(gm.preRoundD1, gm.preRoundD2, false);
            gm.ToggleUI(pauseMenuP1, pauseMenuP2, true);
            Time.timeScale = 0;
            StoreSelected = ES.firstSelectedGameObject;
        }
        else if (gm.isGameRoundTimerRunning)
        {
            gm.ToggleUI(gm.inRoundUI_D1, gm.inRoundUI_D2, false);
            gm.ToggleUI(pauseMenuP1, pauseMenuP2, true);
            Time.timeScale = 0;
            StoreSelected = ES.firstSelectedGameObject;
        }
    }

    public void ResumeGame()
    {
        isPauseMenuActive = false;
        if (!gm.isGameRoundTimerRunning)
        {
            gm.ToggleUI(gm.preRoundD1, gm.preRoundD2, true);
            gm.ToggleUI(pauseMenuP1, pauseMenuP2, false);
            Time.timeScale = 1;
        }
        else if (gm.isGameRoundTimerRunning)
        {
            gm.ToggleUI(gm.inRoundUI_D1, gm.inRoundUI_D2, true);
            gm.ToggleUI(pauseMenuP1, pauseMenuP2, false);
            Time.timeScale = 1;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ResumeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
