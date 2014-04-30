using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 2f;

	private GameObject grabbedObject = null;
	private GameObject heldObject = null;
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
				if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable")){
					hit.collider.gameObject.SendMessage("Interact", transform);

				}

				//Push & pull
				if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Grabbable")){
					hit.collider.gameObject.SendMessage("StartGrabbing", transform);
					grabbedObject = hit.collider.gameObject;
				}
			}
		}

		if (Input.GetButtonUp("Fire3")) {
			if(grabbedObject != null){
				grabbedObject.SendMessage("StopGrabbing", transform);
				grabbedObject = null;
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
		animator.SetFloat("SpeedX", horizontalInput);
		animator.SetFloat("SpeedY", verticalInput);
		if(grabbedObject == null){


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

			animator.SetFloat("DirectionX", currentDirection.x);
			animator.SetFloat("DirectionY", currentDirection.y);
		}
	}
}

