using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public EventSystem ES;
    private GameObject StoreSelected;
    public string LevelToLoad = "TimeTrial_Level_v3";
    public string LevelToLoad2 = "";

    public UnityEngine.UI.Toggle invert_Y, jumpOnPress;

    public void Start()
    {

        StoreSelected = ES.firstSelectedGameObject;

        if (Display.displays.Length > 1)
            Display.displays[1].Activate();
    }

    public void Update()
    {
        Settings.GameSettings.invert_Y = invert_Y.isOn;
        Settings.GameSettings.jumpOnPress = jumpOnPress;

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
    }

    public void StartLevel1()
    {
        SceneManager.LoadScene(LevelToLoad);
        Settings.GameSettings.invert_Y = invert_Y.isOn;
        Settings.GameSettings.jumpOnPress = jumpOnPress.isOn;
    }

    public void StartLevel2()
    {
        SceneManager.LoadScene(LevelToLoad2);
        Settings.GameSettings.invert_Y = invert_Y.isOn;
        Settings.GameSettings.jumpOnPress = jumpOnPress.isOn;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
