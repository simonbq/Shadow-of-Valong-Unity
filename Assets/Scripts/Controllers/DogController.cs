using UnityEngine;
using System.Collections;

public class DogController : MonoBehaviour {

	private bool running = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(running){
			if(transform.position.x < 3){
			transform.Translate(0.02f, 0, 0);
				collider2D.enabled=false;
				RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1, 1 << LayerMask.NameToLayer("Interactable"));
				collider2D.enabled=true;
				if(hit){
					Debug.Log(hit.collider.gameObject.name);
					hit.collider.gameObject.SendMessage("Interact", transform);
				}
			}
		}
	}
	
	void Interact(){
		running = true;
	}
}
