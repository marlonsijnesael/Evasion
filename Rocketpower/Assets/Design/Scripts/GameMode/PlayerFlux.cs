using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFlux : MonoBehaviour
{
    public FluxManager gm;
	
	private int playerID = 0;
	private Color playerColor;
	
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
	}
	
	private void Update(){
		Debug.Log(this);
	}
	
    private void OnTriggerEnter(Collider other) {
		//on grabbing the flux, set fluxplayer to this player
        if (other.gameObject.CompareTag("Flux")) {
            gm.fluxPlayer = this;
			gm.textFluxPlayer.text = "Flux: " + gm.fluxPlayer.ToString();
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
    }

}
