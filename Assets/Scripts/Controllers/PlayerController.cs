using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float movementSpeed = 2f;
	public float interactionDistance = 0.5f;

    private bool justDroppedObject = false;
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
        if (GameController.gameState == GameController.GameState.GAME){
            if (Input.GetButtonDown("Fire1"))
            {
                /*A button*/
                //Attack
            }
            if (Input.GetButtonDown("Inventory"))
            {
                /*B button*/
                //Inventory
            }
            if (Input.GetButtonDown("Interact"))
            {
                /*X button*/
                //Interact

                if (heldObject != null)
                {
                    //fix throw code
                    heldObject.SendMessage("Drop", transform);
                    heldObject = null;
                    justDroppedObject = true;
                }

                RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, interactionDistance, (1 << LayerMask.NameToLayer("Interactable") | 1 << LayerMask.NameToLayer("Grabbable") | 1 << LayerMask.NameToLayer("Liftable")));
                if (hit && heldObject == null)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Interactable")){
                        hit.collider.gameObject.SendMessage("Interact", transform);

                    }

                    //Push & pull
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Grabbable")){
                        hit.collider.gameObject.SendMessage("StartGrabbing", transform);
                        grabbedObject = hit.collider.gameObject;
                    }

                    //Pick up object
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Liftable") && !justDroppedObject){
                        hit.collider.gameObject.SendMessage("Lift", transform);
                        heldObject = hit.collider.gameObject;
                    }
                }
            }

            if (Input.GetButtonUp("Interact"))
            {
                if (grabbedObject != null)
                {
                    grabbedObject.SendMessage("StopGrabbing", transform);
                    grabbedObject = null;
                }
            }

            if (Input.GetButtonDown("Map"))
            {
                /*Y button*/
                //Map

            }

            Debug.DrawRay(transform.position, currentDirection, Color.red);
        }
        justDroppedObject = false;
	}

	void FixedUpdate(){
        if (GameController.gameState == GameController.GameState.GAME){
            Move();
        }
	}

	void Move(){
		float horizontalInput = Input.GetAxis ("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");
		//Movement
		transform.Translate (horizontalInput * movementSpeed * Time.deltaTime, verticalInput*movementSpeed * Time.deltaTime, 0);
		if(horizontalInput != 0 || verticalInput != 0){
			animator.SetFloat("SpeedX", horizontalInput);
			animator.SetFloat("SpeedY", verticalInput);
			animator.SetBool("Walking", true);
			if(grabbedObject == null){

					
				if (horizontalInput > 0.05f) {
					currentDirection = Vector2.right;
				}
				if (horizontalInput < -0.05f) {
					currentDirection = -Vector2.right;
				}
				if (verticalInput > 0.05f) {
					currentDirection = Vector2.up;
				}
				if (verticalInput < -0.05f) {
					currentDirection = -Vector2.up;
				}

				animator.SetFloat("DirectionX", currentDirection.x);
				animator.SetFloat("DirectionY", currentDirection.y);
			}
		}else{
			animator.SetBool("Walking", false);
		}
	}
}

