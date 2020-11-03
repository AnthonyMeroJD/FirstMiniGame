using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkyColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update() {
		
		SpriteRenderer sol = this.GetComponent<SpriteRenderer>();
		GameObject cameraO = GameObject.FindWithTag("MainCamera");
		Camera cameraC=cameraO.GetComponent<Camera>();
		if (sol != null) {
			if (Input.GetKeyDown(KeyCode.N))
			{
				sol.color = Color.white;
				cameraC.backgroundColor = Color.black;
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				sol.color = Color.yellow;
				cameraC.backgroundColor = Color.blue;

			}
		}

	
		
	}
}
