using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour {

	public bool activateRotation = true;
	public float speedRotation = 5f;

	private void Update() 
	{
		if(activateRotation)
			transform.Rotate(Vector3.up*speedRotation*Time.deltaTime);
	}
}
