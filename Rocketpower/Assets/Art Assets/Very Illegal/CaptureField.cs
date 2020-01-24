using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureField : MonoBehaviour
{
    public Material mat;
    public Transform otherPlayer;
    public float minDist;
    public float maxDist;
    private float alphaSlide;
    private float totalDist;
    
    void Start()
    {
        alphaSlide = mat.GetFloat("_Alpha");
    }

    
    void Update()
    {
        if (otherPlayer)
        {
            totalDist = Vector3.Distance(otherPlayer.position, transform.position);
            print(totalDist);
        }
        /*if (totalDist >= 5)
        {
            alphaSlide = Mathf.Clamp01();
            mat.SetFloat("_Alpha", alphaSlide);
        }
        else
        {
            alphaSlide = 1f;
             mat.SetFloat("_Alpha", alphaSlide);
        }*/
        alphaSlide = Mathf.Clamp01(-(maxDist - totalDist)/(minDist - maxDist));
        mat.SetFloat("_Alpha", alphaSlide);
    }
}
