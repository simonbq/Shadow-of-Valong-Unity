using UnityEngine;
using System.Collections;
using System.IO;

public class GertrudeController : MonoBehaviour {

    private DialogueContainer dialogues;

	// Use this for initialization
	void Start () {
        dialogues = DialogueContainer.Load("dialogue.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact(){
        startDialogue(1);
    }
    void startDialogue(int dialogueId)
    {
        Debug.Log("Dialogue started");
        GUI.Label((new Rect(10, 10, 100, 20)), dialogues.Dialogues[0].Texts[0]);
    }
}
