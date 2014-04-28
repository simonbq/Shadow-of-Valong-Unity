using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

	public float movementSpeed = 0.1f;
	
	private Vector2 currentDirection = -Vector2.up;

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
		if (Input.GetButton ("Fire1")) {
			/*X button*/
			//Interact
			collider2D.enabled = false;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 2);
			collider2D.enabled = true;
			if(hit){
				Debug.Log(hit.collider.gameObject.name);
				hit.collider.gameObject.SendMessage("Interact");
			}
		}
		Debug.DrawRay(transform.position, currentDirection, Color.red);
	}

	void FixedUpdate(){
		Move ();
	}

	void Move(){
		float horizontalInput = Input.GetAxis ("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");
		//Movement
		transform.Translate (horizontalInput * movementSpeed, verticalInput*movementSpeed, 0);
		if (horizontalInput > 0) {
			currentDirection = Vector2.right;
		}
		if (horizontalInput < 0) {
			currentDirection = -Vector2.right;
		}
		if (verticalInput > 0) {
			currentDirection = Vector2.up;
		}
		if (verticalInput < 0) {
			currentDirection = -Vector2.up;
		}
	}
}

