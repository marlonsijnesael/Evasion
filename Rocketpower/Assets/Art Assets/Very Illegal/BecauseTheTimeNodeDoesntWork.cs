using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecauseTheTimeNodeDoesntWork : MonoBehaviour
{
    
	public List<Material> materialList;
	
	private void Update(){
		foreach (Material m in materialList){
			m.SetFloat("_t", Time.time);
		}
	}
	
}
