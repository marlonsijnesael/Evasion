using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColor : MonoBehaviour
{
    public bool isPlatformOrange;
    public bool isPlatformBlue;

    void Update() {
        platformColorCheck();
    }

    void platformColorCheck() {
        if (gameObject.GetComponent<Renderer>().material.name == "M_PlatformOrange_v1 (Instance)") {
            isPlatformOrange = !isPlatformBlue;
        }

        if (gameObject.GetComponent<Renderer>().material.name == "M_PlatformColorBlue_v1") {
            isPlatformBlue = !isPlatformOrange;
        }
    }
}