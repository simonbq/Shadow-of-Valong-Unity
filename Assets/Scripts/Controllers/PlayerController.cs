using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //Public variables
	public float movementSpeed = 2f;
	public float interactionDistance = 0.5f;

    //Public sounds
    public AudioClip hitSound;
    public AudioClip walkSound;
    //

    private bool justDroppedObject = false;
	private GameObject grabbedObject = null;
    protected float speedModifier = 1.0f;
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
            if (Input.GetButtonDown("AbilityToggle"))
            {
                /*B button*/
                //Toggle abilities on and off
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
                        speedModifier = 0.6f;
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
                    speedModifier = 1.0f;
                }
            }

            Debug.DrawRay(transform.position, currentDirection, Color.red);
        }
        justDroppedObject = false;

        if (Input.GetButtonDown("Inventory")){
            /*Y button*/
            //Ingame menus
            switch(GameController.gameState){
                case GameController.GameState.GAME:
                    GameController.gameState = GameController.GameState.QUESTMENU;
                    break;
                case GameController.GameState.QUESTMENU:
                    GameController.gameState = GameController.GameState.GAME;
                    break;
                default:
                    GameController.gameState = GameController.GameState.GAME;
                    break;
            }
        }
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
		transform.Translate (horizontalInput * movementSpeed * speedModifier * Time.deltaTime, verticalInput * movementSpeed * speedModifier * Time.deltaTime, 0);
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

