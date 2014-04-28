using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public float movementSpeed = 0.5f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1")) {
			/*A button*/
			//Attack
		}
		if (Input.GetButton ("Fire2")) {
			/*B button*/
			//
		}
		if (Input.GetButton ("Fire3")) {
			/*X button*/
			//Interact

		}
	}

	void FixedUpdate(){
		float horizontalInput = Input.GetAxis ("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");
		//Movement
		transform.Translate (horizontalInput * movementSpeed, verticalInput*movementSpeed, 0);
			

	}
}
