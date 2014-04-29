using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 2f;

	private bool isGrabbing = false;
	private Vector2 currentDirection = -Vector2.up;
	protected Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")) {
			/*A button*/
			//Attack
		}
		if (Input.GetButtonDown("Fire2")) {
			/*B button*/
			//
		}
		if (Input.GetButtonDown("Fire3")) {
			/*X button*/
			//Interact
			RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, 1, (1 << LayerMask.NameToLayer("Interactable") | 1 << LayerMask.NameToLayer("Grabbable"))	);
			if(hit){
				Debug.Log(hit.collider.gameObject.name);
				hit.collider.gameObject.SendMessage("Interact", transform);

				//Push & pull
				if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Grabbable")){
					isGrabbing = true;
					hit.collider.gameObject.SendMessage("Interact", transform);
				}
			}
		}
		if (Input.GetButtonUp("Fire3")) {
			if(isGrabbing){
				isGrabbing=false;
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
		transform.Translate (horizontalInput * movementSpeed * Time.deltaTime, verticalInput*movementSpeed * Time.deltaTime, 0);
		if(!isGrabbing){
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
}

