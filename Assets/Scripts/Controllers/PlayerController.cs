using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 0.1f;
	
	private Vector2 currentDirection = -Vector2.up;
	
	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
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
			RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 1, 1 << LayerMask.NameToLayer("Interactable"));
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
			animator.Play("PlayerRight");
		}
		if (horizontalInput < 0) {
			currentDirection = -Vector2.right;
			animator.Play("PlayerLeft");
		}
		if (verticalInput > 0) {
			currentDirection = Vector2.up;
			animator.Play("PlayerUp");
		}
		if (verticalInput < 0) {
			currentDirection = -Vector2.up;
			animator.Play("PlayerDown");
		}
	}
}

