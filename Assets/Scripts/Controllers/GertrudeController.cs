using UnityEngine;
using System.Collections;

public class GertrudeController : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact(){
        GameController.startDialogue(0);
    }
}
