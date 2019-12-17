using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	/*public bool rotateX = true;
	public bool rotateY = true;
	public bool rotateZ = true;

	public float rotatingSpeed = .5f;*/
	
	public Vector3 rotationDirection = new Vector3(0, 0, 0);

	private float xRotation = 0;
	private float yRotation = 0;
	private float zRotation = 0;

	private void FixedUpdate() {
		xRotation += rotationDirection.x;
		yRotation += rotationDirection.y;
		zRotation += rotationDirection.z;

		transform.localRotation = Quaternion.Euler(new Vector3(xRotation, yRotation, zRotation));
	}

}