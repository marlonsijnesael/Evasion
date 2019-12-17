using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlayground : MonoBehaviour
{

	[Header("Environment Colours")]
	public Color mainMetal;
	public Color stripes;
	public Color tertiaryEnvironmentColor;
	public Color fluxPlatformBaseColor;

	[Header("Character Colours")]
	public Color heads;
	public Color player1;
	public Color player1Fresnel;
	public Color player2;
	public Color player2Fresnel;
	public int pulseTime = 30;

	[Header("Update")]
	public bool autoUpdate = false;
	public KeyCode updateKey = KeyCode.Space;

	[Header("Meshes that need color changing")]
	public Transform mainMetalMeshParent;
	public Transform stripesMeshParent;
	public Transform tertiaryColorMeshParent;
	public Transform fluxPlatformBaseMesh;
	public Transform fluxPlatformP1Mesh;
	public Transform fluxPlatformP2Mesh;
	public Transform p1meshParent;
	public Transform p2meshParent;

	//material lists
	private List<Material> mainMetalList = new List<Material>();
	private List<Material> stripesList = new List<Material>();
	private List<Material> tertiaryList = new List<Material>();
	private Material baseFluxPlatformMaterial;
	private Material p1FluxPlatformMaterial;
	private Material p2FluxPlatformMaterial;
	private List<Material> player1List = new List<Material>();
	private List<Material> player2List = new List<Material>();

	private void Start() {
		InitialiseVariables();
		StartCoroutine(UpdatePlayerState());
		UpdateColors();
	}

	private void Update() {
		if (autoUpdate || Input.GetKeyDown(updateKey)) {
			UpdateColors();
		}
	}

	private void InitialiseVariables() {
		foreach (Transform mesh in mainMetalMeshParent) {
			mainMetalList.Add(mesh.GetComponent<Renderer>().materials[0]);
		}
		foreach (Transform mesh in stripesMeshParent) {
			stripesList.Add(mesh.GetComponent<Renderer>().materials[1]);
		}
		foreach (Transform mesh in tertiaryColorMeshParent) {
			tertiaryList.Add(mesh.GetComponent<Renderer>().material);
		}

		baseFluxPlatformMaterial = fluxPlatformBaseMesh.GetComponent<Renderer>().material;
		p1FluxPlatformMaterial = fluxPlatformP1Mesh.GetComponent<Renderer>().material;
		p2FluxPlatformMaterial = fluxPlatformP2Mesh.GetComponent<Renderer>().material;

		foreach (Transform mesh in p1meshParent) {
			player1List.Add(mesh.GetComponent<Renderer>().material);
		}
		foreach (Transform mesh in p2meshParent) {
			player2List.Add(mesh.GetComponent<Renderer>().material);
		}
	}

	private void UpdateColors() {
		
		foreach (Material mat in mainMetalList) {
			mat.SetColor("_BaseColor", mainMetal);
		}
		foreach (Material mat in stripesList) {
			mat.SetColor("_BaseColor", stripes);
			mat.SetColor("_EmissiveColor", stripes);
			mat.SetColor("_EmissiveColorLDR", stripes);
		}
		foreach (Material mat in tertiaryList) {
			mat.SetColor("_BaseColor", tertiaryEnvironmentColor);
		}

		baseFluxPlatformMaterial.SetColor("_StartColor", fluxPlatformBaseColor);
		baseFluxPlatformMaterial.SetColor("_GoalColor", fluxPlatformBaseColor);
		p1FluxPlatformMaterial.SetColor("_StartColor", player1);
		p1FluxPlatformMaterial.SetColor("_GoalColor", player1);
		p2FluxPlatformMaterial.SetColor("_StartColor", player2);
		p2FluxPlatformMaterial.SetColor("_GoalColor", player2);

		foreach (Material mat in player1List) {
			mat.SetColor("_BaseColor", heads);
			mat.SetColor("_Color", player1);
			mat.SetColor("_FresnelColor", player1Fresnel);
		}

		foreach (Material mat in player2List) {
			mat.SetColor("_BaseColor", heads);
			mat.SetColor("_Color", player2);
			mat.SetColor("_FresnelColor", player2Fresnel);
		}
	}

	private IEnumerator UpdatePlayerState() {
		float pulseFrac = 1.0f / (pulseTime - 1);
		while (true) {
			for (int i = 0; i < pulseTime; i++) {
				foreach (Material mat in player1List) {
					mat.SetFloat("_State", i * pulseFrac);
				}
				foreach (Material mat in player2List) {
					mat.SetFloat("_State", 1 - i * pulseFrac);
				}
				yield return null;
			}
			for (int i = 0; i < pulseTime; i++) {
				foreach (Material mat in player2List) {
					mat.SetFloat("_State", i * pulseFrac);
				}
				foreach (Material mat in player1List) {
					mat.SetFloat("_State", 1 - i * pulseFrac);
				}
				yield return null;
			}
		}
	}

}
