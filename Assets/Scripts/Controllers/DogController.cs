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
			transform.Translate(0.02f, 0, 0);
		}
	}
	
	void Interact(){
		running = true;
	}
}
