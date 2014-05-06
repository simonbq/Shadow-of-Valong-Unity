using UnityEngine;
using System.Collections;
public class GameController : MonoBehaviour {

    private DialogueContainer dialogues;
    private static bool inDialogue;
    private static int currentDialogue;
    private static int currentText;

	// Use this for initialization
	void Start (){
		inDialogue = false;
		//Load all dialogues
        dialogues = DialogueContainer.Load("dialogue.xml");
        //Count them for debugging
        Debug.Log(dialogues.Dialogues.Count);
	}
	
	// Update is called once per frame
	void Update (){
		if (inDialogue && Input.GetButtonDown("Interact")) {
			currentText++;
		}
	}

    void OnGUI(){
    	if(inDialogue){
    		if(currentText < dialogues.Dialogues[currentDialogue].Texts.Count){
				GUI.Label((new Rect(10, 10, 100, 20)), dialogues.Dialogues[currentDialogue].Texts[currentText]);
			}
    	}
    	/*
        for (int i = 0; i < dialogues.Dialogues.Count; i++){
            for (int j = 0; j < dialogues.Dialogues[i].Texts.Count; j++){
                GUI.Label((new Rect(10+i*10, 10+j*10, 100, 20)), dialogues.Dialogues[i].Texts[j]);
                Debug.Log(dialogues.Dialogues[i].Texts[j]);
            }
        }*/
    }
    
	public static void startDialogue(int dialogueId)
	{
		Debug.Log("Dialogue started: "+dialogueId);
		currentDialogue = dialogueId;
		currentText = 0;
		inDialogue=true;
	}
}
