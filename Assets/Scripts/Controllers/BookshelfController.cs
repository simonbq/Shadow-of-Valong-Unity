using UnityEngine;
using System.Collections;

public class BookshelfController : MonoBehaviour {
    public int dialogueId;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact(){
        DialogueController.startDialogue(dialogueId);
    }
}
