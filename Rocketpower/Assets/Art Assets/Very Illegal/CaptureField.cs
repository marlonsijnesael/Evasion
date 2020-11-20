using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureField : MonoBehaviour
{
    private Material mat;
	public float maxAlpha;
    public Transform otherPlayer;
    public float minDist;
    public float maxDist;

    private float alphaSlide;
    private float totalDist;
	private bool isRendering = true;
    
    void Start()
    {
        mat = transform.GetComponent<Renderer>().material;
		alphaSlide = mat.GetFloat("_Alpha");

		mat.renderQueue = 3002; //i hate this with my entire being

		transform.GetComponent<Renderer>().enabled = false;
		isRendering = false;
    }

    
    void Update()
    {
        if (otherPlayer)
        {
            totalDist = Vector3.Distance(otherPlayer.position, transform.position);
			alphaSlide = Mathf.Clamp01(-(maxDist - totalDist)/(minDist - maxDist)) * maxAlpha;

			if (alphaSlide > .01f) {
				if (!isRendering) {
					transform.GetComponent<Renderer>().enabled = true;
					isRendering = true;
				}
				mat.SetFloat("_Alpha", alphaSlide);
			}
			else if (isRendering) {
				transform.GetComponent<Renderer>().enabled = false;
				isRendering = false;
			}
        }
		
        
    }
}
