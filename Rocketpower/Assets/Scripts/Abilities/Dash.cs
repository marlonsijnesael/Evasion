using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash : MonoBehaviour {
    public VirtualController VirtualController;

    public Transform playerPos;
    public Transform tpObject;
    public Transform originalPlayerPos;
    public GameObject originalPlayerObject;

    public float dashVelocity;
    public float dashCooldownSec;

    private bool isDashOnCD;
    private bool isTPDone;

    float dashTimer;

    //UI Elements
    public GameObject dashCDTextObj;
    public Text dashCDText;
    public Text textCDTimer;
    public Slider tpSlider;
    public GameObject tpSliderObj;

    void Start() {
        textCDTimer.text = isDashOnCD.ToString();
        dashCDText.text = "Ready";
    }

    void Update() {
        Debug.Log(isTPDone);
        if (VirtualController.XButtonPressedThisFrame && !isDashOnCD) {
            originalPlayerPos.transform.position = playerPos.transform.position;
            StartCoroutine(onDash(this.GetComponent<StateMachine>()));
            StartCoroutine(tpDuration());
            tpSliderObj.SetActive(true);
            dashCDTextObj.SetActive(false);
        }
    }

    void LateUpdate() {
        if (isTPDone) {
            if (VirtualController.XButtonPressedThisFrame) {
                playerPos.transform.position = originalPlayerPos.transform.position;
                originalPlayerObject.SetActive(false);
                isTPDone = false;
                tpSliderObj.SetActive(false);
                StartCoroutine(dashCooldown());
            }
        }
    }

    void DashUse(StateMachine owner) {
        dashVelocity = 300;
        owner.gameObject.GetComponent<StateMachine>().forwardVelocity = dashVelocity;
        owner.gameObject.GetComponent<StateMachine>().maxSpeed = dashVelocity;
    }

    private IEnumerator onDash(StateMachine _owner) {
        isDashOnCD = true;
        textCDTimer.text = isDashOnCD.ToString();
        dashVelocity = 25;
        dashTimer = 0.1f;

        _owner.forwardVelocity = dashVelocity;
        _owner.maxSpeed = dashVelocity;
        yield return new WaitForSeconds(dashTimer);
        _owner.maxSpeed = 9f;

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator dashCooldown() {
        dashCDTextObj.SetActive(true);
        dashCooldownSec = 11f;
        while (dashCooldownSec > 1) {
            dashCooldownSec -= 1;
            dashCDText.text = dashCooldownSec.ToString();
            yield return new WaitForSeconds(1);
        }
        dashCDText.text = "Ready";
        isDashOnCD = false;
        textCDTimer.text = isDashOnCD.ToString();
    }

    public IEnumerator tpDuration() {
        float timeTP = 0f;
        float tpCDSec = 6;
        tpSlider.value = tpCDSec;

        yield return new WaitForSeconds(0.01f);
        isTPDone = true;
        originalPlayerObject.SetActive(true);

        while (timeTP < tpCDSec) {
            timeTP += Time.deltaTime;
            float lerpValueTP = timeTP / tpCDSec;
            tpSlider.value = Mathf.Lerp(6f, 0f, lerpValueTP);
            yield return null;
        }

        if (isTPDone) {
            originalPlayerObject.SetActive(false);
            isTPDone = false;
            tpSliderObj.SetActive(false);
            StartCoroutine(dashCooldown());
        }

        yield return new WaitForEndOfFrame();
    }
}