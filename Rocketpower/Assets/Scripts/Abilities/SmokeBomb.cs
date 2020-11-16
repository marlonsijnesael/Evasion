using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeBomb : MonoBehaviour {
    public VirtualController VirtualController;
    public Transform playerPos;

    private bool isSmokeCD;
    public GameObject smokeBombObj;
    private GameObject clone;
    private float smokeCDSec;

    public GameObject smokeCDTextObj;
    public Text smokeCDText;

    void Start() {
        smokeCDText.text = "Ready";
    }

    void LateUpdate() {
        if (VirtualController.YButtonPressedThisFrame && !isSmokeCD) {
            Debug.Log("Test");
            //StartCoroutine(spawnSmoke());
            clone = Instantiate(smokeBombObj, playerPos.transform.position + (playerPos.forward * -4) + (playerPos.up * 2), playerPos.transform.rotation);
            isSmokeCD = true;
            StartCoroutine(smokeCooldown());
            //StartCoroutine(spawnSmoke());
        }
    }

    public IEnumerator spawnSmoke() {
        yield return new WaitForSeconds(0.5f);
        clone = Instantiate(smokeBombObj, playerPos.transform.position + (playerPos.forward * -12) + (playerPos.up * 2), playerPos.transform.rotation);
        isSmokeCD = true;
    }

    public IEnumerator smokeCooldown() {
        smokeCDSec = 10f;
        while (smokeCDSec > 1) {
            smokeCDSec -= 1;
            smokeCDText.text = smokeCDSec.ToString();
            yield return new WaitForSeconds(1);
            if (smokeCDSec < 5) {
                Destroy(clone);
            }
        }
        smokeCDText.text = "Ready";
        isSmokeCD = false;
    }
}