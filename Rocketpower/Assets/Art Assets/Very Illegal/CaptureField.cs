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
    
    void Start()
    {
        mat = transform.GetComponent<Renderer>().material;
		alphaSlide = mat.GetFloat("_Alpha");
    }

    
    void Update()
    {
        if (otherPlayer)
        {
            totalDist = Vector3.Distance(otherPlayer.position, transform.position);
            //print(totalDist);
			
			//if (totalDist < maxDist){
				alphaSlide = Mathf.Clamp01(-(maxDist - totalDist)/(minDist - maxDist)) * maxAlpha;
			/*}
			else {
				alphaSlide = 0;
			}*/
			mat.SetFloat("_Alpha", alphaSlide);
        }
		
        
    }
}
