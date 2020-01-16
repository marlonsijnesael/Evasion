using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// movement audio
public class RunningAudio : MonoBehaviour
{
	GameObject obj;

    bool playerismoving = false;
    public float stepRate = 0.30f;
    private float nextFire = 0.0f;

    FMOD.Studio.EventInstance Steps_scape;
    FMOD.Studio.ParameterInstance SSV;
	
	FMOD.Studio.EventInstance StepsWall;
	FMOD.Studio.ParameterInstance SWV;

    void Start()
    {
		obj = GameObject.FindGameObjectWithTag("Player");

        Steps_scape = FMODUnity.RuntimeManager.CreateInstance("event:/SD/Steps_scape");
        Steps_scape.getParameter("SSV", out SSV);
        SSV.setValue(0.7f);
		
		StepsWall = FMODUnity.RuntimeManager.CreateInstance("event:/SD/StepsWall");
		StepsWall.getParameter("SWV", out SWV);
        SWV.setValue(0.75f);
    }

    void Update()
    {
		// walk
        if ((int)obj.GetComponent<StateMachine>().playerState == 1 && Time.time > nextFire)
        {
			SSV.setValue(0.62f);
			SWV.setValue(0.4f);

            nextFire = Time.time + stepRate + Random.Range(0.0f, 0.1f);
			
			StepsWall.start();
            Steps_scape.start();
        }
		// wallrun
		else if ((int)obj.GetComponent<StateMachine>().playerState == 2 && Time.time > nextFire)
		{
			nextFire = Time.time + stepRate + Random.Range(0.0f, 0.1f);
			SSV.setValue(0.5f);
			SWV.setValue(0.8f);

			Steps_scape.start();
            StepsWall.start();
		}
		// wallrun right
		else if ((int)obj.GetComponent<StateMachine>().playerState == 3 && Time.time > nextFire)
		{
			nextFire = Time.time + stepRate + Random.Range(0.0f, 0.1f);
			SSV.setValue(0.5f);
			SWV.setValue(0.8f);

			Steps_scape.start();
            StepsWall.start();
		}
		// climb
		else if ((int)obj.GetComponent<StateMachine>().playerState == 4 && Time.time > nextFire)
		{
			nextFire = Time.time + stepRate + Random.Range(0.0f, 0.1f);
			SSV.setValue(0.5f);
			SWV.setValue(0.8f);

			Steps_scape.start();
            StepsWall.start();
		}
    }
}
