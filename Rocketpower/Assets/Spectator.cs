using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectator : MonoBehaviour
{
    public Camera spectatorCam;
    public GameObject testCam;
    public bool isLerping;

    public Camera currentCam;

    public static Spectator _instance;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        currentCam = spectatorCam;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentCam = testCam.GetComponent<Camera>();
            StartCoroutine(LerpCamToPos(2));
        }
    }

    private void LateUpdate()
    {
        if (!isLerping)
        {
            transform.position = currentCam.transform.position;
            transform.rotation = currentCam.transform.rotation;
            spectatorCam.fieldOfView = currentCam.fieldOfView;
        }
    }

    public IEnumerator LerpCamToPos(float time)
    {
        float elapsedTime = 0;
        Vector3 startPos = transform.position;
        Quaternion rotation = transform.rotation;
        isLerping = true;
        while (elapsedTime < time)
        {

            transform.position = Vector3.Lerp(startPos, currentCam.transform.position, (elapsedTime / time));
            transform.rotation = Quaternion.Lerp(rotation, currentCam.transform.rotation, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isLerping = false;
    }

    public void SwitchCam(Camera newCam)
    {
        currentCam = newCam;
        spectatorCam.fieldOfView = currentCam.fieldOfView;
    }
}

