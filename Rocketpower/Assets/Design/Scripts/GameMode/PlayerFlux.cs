using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFlux : MonoBehaviour
{
    [Header("GameMode")]
	public FluxManager gm;
	
	[Header("Changing Material")]
	[Tooltip("please put the transform containing all mesh parts of the character here")]public Transform meshParent;
	public int transitionTime;
	
	private int playerID = 0;
	private Color playerColor;
	private float currFluxCaptureTime;
	
	private List<Material> materialList = new List<Material>();
	
	//set player ID based on tag
	private void Awake(){
		if (this.transform.tag == "Player" || this.transform.tag == "Player1"){
			playerID = 1;
			gm.player1 = this;
			playerColor = gm.player1color;
		}
		else if (this.transform.tag == "Player2"){
			playerID = 2;
			gm.player2 = this;
			playerColor = gm.player2color;
		}
		
		foreach (Transform part in meshParent){
			materialList.Add(part.GetComponent<Renderer>().material);
		}
	}
	
	private void Update(){
		//Debug.Log(isFluxPlayerColliderOnCD);
	}
	
    private void OnTriggerEnter(Collider other) {
		//on grabbing the flux, set fluxplayer to this player
        if (other.gameObject.CompareTag("Flux")) {
			gm.ChangeFluxplayer(this);
            //gm.fluxPlayer = this;
			//gm.textFluxPlayer.text = "Flux: " + gm.fluxPlayer.ToString();
            Destroy(other.gameObject);
        }
		
		//on stepping on platform while holding flux, claim it
        if (other.gameObject.CompareTag("ColorPlatform") && gm.fluxPlayer == this) {
			PlatformState platform = other.gameObject.GetComponent<PlatformState>();
			
			if ((platform.GetPlayerID() != playerID) && (platform.GetIsChanging() == false)){
				platform.ChangeColorTo(playerID, playerColor);
				gm.updateScore();
			}
        }
		
		//single collision flux transfer + starting cooldown
		/*if (other.gameObject.CompareTag("FluxCollider" )){
			if(gm.fluxPlayer == other.gameObject.transform.parent.GetComponent<PlayerFlux>() && !gm.isFluxPlayerColliderOnCD){
				Debug.Log("Parent Collision");
				gm.fluxPlayer = this;
				gm.textFluxPlayer.text = "Flux: " + gm.fluxPlayer.ToString();
				gm.StartCoroutine("FluxColliderSeconds");
			}
			else{
				return;
			}
		}*/
    }

	private void OnTriggerStay(Collider other){
		//Flux transfer over time + cooldown
		if (other.gameObject.CompareTag("FluxCollider" )){
			if(gm.fluxPlayer == other.gameObject.transform.parent.GetComponent<PlayerFlux>() && gm.fluxPlayer != this.gameObject.transform.GetComponent<PlayerFlux>() && !gm.isFluxPlayerColliderOnCD){
				gm.sliderCaptureObject.SetActive(true);
				currFluxCaptureTime += Time.deltaTime;
				gm.sliderCaptureTime.value = currFluxCaptureTime;			
				if(currFluxCaptureTime > gm.fluxCaptureTime){
					PlayerFlux otherPlayer = gm.fluxPlayer;
					gm.ChangeFluxplayer(this);
					//gm.fluxPlayer = this;
					//gm.textFluxPlayer.text = "Flux: " + gm.fluxPlayer.ToString();
					gm.StartCoroutine("FluxColliderSeconds");
					currFluxCaptureTime = 0;
					gm.sliderCaptureObject.SetActive(false);
				}
			}
			else{
				return;
			}
		}
	}

	private void OnTriggerExit(Collider other){
		currFluxCaptureTime = 0;
		gm.sliderCaptureTime.value = 0;
		gm.sliderCaptureObject.SetActive(false);
	}
	
	public void TurnFlux(bool turnOn){
		if (turnOn){
			StartCoroutine(TurnOnFluxEffect());
		}
		else{
			StartCoroutine(TurnOffFluxEffect());
		}
	}
	
	private IEnumerator TurnOnFluxEffect(){
		float transitionFrac = 1.0f / (transitionTime - 1);
		for (int i = 0; i < transitionTime; i++){
			foreach (Material mat in materialList){
				mat.SetFloat("_State", i * transitionFrac);
			}
			yield return null;
		}
		yield return null;
	}
	
	private IEnumerator TurnOffFluxEffect(){
		float transitionFrac = 1.0f / (transitionTime - 1);
		for (int i = 0; i < transitionTime; i++){
			foreach (Material mat in materialList){
				mat.SetFloat("_State", 1 - (i * transitionFrac));
			}
			yield return null;
		}
		yield return null;
	}

}












