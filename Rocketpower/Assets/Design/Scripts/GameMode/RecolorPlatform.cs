using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecolorPlatform : MonoBehaviour
{
    public GameModeManager gameModeManager;

    public Text textOrange;

    void Update() {
        textOrange.text = gameModeManager.orangeScore.ToString();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Flux") && this.transform.tag == "Player") {
            //gameModeManager.isFluxActive = true;
            //gameModeManager.flux.transform.parent = gameObject.transform;
            gameModeManager.player1 = gameModeManager.fluxPlayer;
        }

        if(other.gameObject.CompareTag("Flux") && this.transform.tag == "Player2"){
            gameModeManager.player2 = gameModeManager.fluxPlayer;
        }

        if (other.gameObject.CompareTag("ColorPlatform") && gameModeManager.player1 == gameModeManager.fluxPlayer) {
            other.gameObject.GetComponent<Renderer>().material = gameModeManager.m_orangePlatform;
            gameModeManager.updateScore();
        }

        if(other.gameObject.CompareTag("ColorPlatform") && gameModeManager.player2 == gameModeManager.fluxPlayer){
            other.gameObject.GetComponent<Renderer>().material = gameModeManager.m_bluePlatform;
            gameModeManager.updateScore();
        }
    }

}
