using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public EventSystem ES;
    private GameObject StoreSelected;

    public void Start(){
        StoreSelected = ES.firstSelectedGameObject;
    }

    public void Update(){
        if(ES.currentSelectedGameObject != StoreSelected){
            if(ES.currentSelectedGameObject == null){
                ES.SetSelectedGameObject(StoreSelected);
            }
            else{
                StoreSelected = ES.currentSelectedGameObject;
            }
        }
    }

    public void StartLevel1(){
        SceneManager.LoadScene("Level1");
    }

    public void StartLevel2(){
        SceneManager.LoadScene("Level2");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
