using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecolorPlatform : MonoBehaviour
{
    public GameModeManager gameModeManager;

    void Update() {
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Flux") && this.transform.tag == "Player") {
            gameModeManager.fluxPlayer = gameModeManager.player1;
            Destroy(other.gameObject);
        }

        else if(other.gameObject.CompareTag("Flux") && this.transform.tag == "Player2"){
            gameModeManager.fluxPlayer = gameModeManager.player2;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("ColorPlatform") && gameModeManager.fluxPlayer == gameModeManager.player1 && this.transform.tag == "Player") {
            other.gameObject.GetComponent<Renderer>().material = gameModeManager.m_orangePlatform;
            gameModeManager.updateScore();
        }

        else if(other.gameObject.CompareTag("ColorPlatform") &&  gameModeManager.fluxPlayer == gameModeManager.player2 && this.transform.tag == "Player2"){
            other.gameObject.GetComponent<Renderer>().material = gameModeManager.m_bluePlatform;
            gameModeManager.updateScore();
        }
    }

}
