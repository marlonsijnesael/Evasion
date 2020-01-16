using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningAudio : MonoBehaviour
{
	GameObject obj;
	GameObject obj2;
	
	[FMODUnity.EventRef] public string inputsound;
	[FMODUnity.EventRef] public string inputsound2;
	

	bool player1ismoving;
	bool player2ismoving;
	
	bool player1OnGround;
	bool player2OnGround;

	public float walkingspeed;
	
    void Start()
    {
		obj = GameObject.FindGameObjectWithTag("Player");
		obj2 = GameObject.FindGameObjectWithTag("Player2");
		
        InvokeRepeating("CallFootsteps", 0, walkingspeed);
		InvokeRepeating("CallFootsteps2", 0, walkingspeed);

    }
	
	void CallFootsteps()
	{
		if (player1ismoving == true && player1OnGround == true){
			FMODUnity.RuntimeManager.PlayOneShot(inputsound);
		}
	}
	
	void CallFootsteps2()
	{

		if (player2ismoving == true && player1OnGround == true){
			FMODUnity.RuntimeManager.PlayOneShot(inputsound2);
		}
	}

    void Update()
    {
		if (obj.GetComponent<StateMachine>().forwardVelocity > 0)
		{
			player1ismoving = true;
		}
		else {
			player1ismoving = false;
		}
	
		if (obj2.GetComponent<StateMachine>().forwardVelocity > 0)
		{
			player2ismoving = true;
		}
		else {
			player2ismoving = false;
		}    
		
		player1OnGround = obj.GetComponent<StateMachine>().isGrounded;
		player2OnGround = obj2.GetComponent<StateMachine>().isGrounded;

	}
}
