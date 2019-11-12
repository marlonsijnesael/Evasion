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
        if (other.gameObject.CompareTag("Flux")) {
            gameModeManager.isFluxActive = true;
        }

        if (other.gameObject.CompareTag("ColorPlatform") /*&& gameModeManager.isFluxActive*/) {
            other.gameObject.GetComponent<Renderer>().material = gameModeManager.m_orangePlatform;
        }
    }

}
