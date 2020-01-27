using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuColorLerp : MonoBehaviour
{
    public Renderer rend;
    public Material menuMaterial;
    public Color startColor;
    public Color endColor;

    private float time;
    public float speed;
    public float pongLength;
    void Update()
    {
        time = Mathf.PingPong(Time.time * speed, pongLength);
        Color tmp = Color.Lerp(startColor, endColor, time);
        menuMaterial.SetColor("_BaseColor", tmp);
    }

}
