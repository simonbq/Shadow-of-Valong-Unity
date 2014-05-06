using UnityEngine;
using System.Collections;

public class GertrudeController : MonoBehaviour {

    //private DialogueContainer dialogues;
    //private int currentDialogue;
    //private int currentText;

	// Use this for initialization
	void Start () {
        //dialogues = DialogueContainer.Load("dialogue.xml");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		/*if(currentDialogue != null){
			GUI.Label((new Rect(10, 10, 100, 20)), dialogues.Dialogues[currentDialogue].Texts[currentText]);
		}*/
	}

    void Interact(){
        GameController.startDialogue(0);
    }
    /*
    void startDialogue(int dialogueId)
    {
        Debug.Log("Dialogue started");
		//currentDialogue = dialogueId;
    }
    */
}
