using UnityEngine;
using System.Collections;
public class GameController : MonoBehaviour {

    private DialogueContainer dialogues;
    private static bool inDialogue = false;
    private static int currentDialogue;
    private static int currentText;
    private static bool justStartedDialogue;

    public enum GameState { GAME, PAUSED, MAINMENU, MAP };
    public static GameState gameState = GameState.GAME;

	// Use this for initialization
	void Start (){
		//Load all dialogues
        dialogues = DialogueContainer.Load("dialogue.xml");
        //Count them for debugging
        Debug.Log(dialogues.Dialogues.Count);
	}
	
	// Update is called once per frame
	void Update (){
		if (inDialogue && !justStartedDialogue && Input.GetButtonDown("Interact")) {
			currentText++;
		}
        if (inDialogue){
            gameState = GameState.PAUSED;
        }

        justStartedDialogue = false;
	}

    void OnGUI(){
    	if(inDialogue){
            //Debug.Log(dialogues.Dialogues[currentDialogue].Texts.Count);
    		if(currentText < dialogues.Dialogues[currentDialogue].Texts.Count){
                string dialogueText = replaceVariables(dialogues.Dialogues[currentDialogue].Texts[currentText].value);
                //string dialogueText = "something";
                string speakerId = string.Empty + dialogues.Dialogues[currentDialogue].Texts[currentText].SpeakerId;
                //string speakerId = "someone";
				GUI.Label((new Rect(400, 100, 400, 20)),  speakerId + " says: " + dialogueText);
			}else{
                inDialogue = false;
                gameState = GameState.GAME;
            }
    	}
    }
    
	public static void startDialogue(int dialogueId)
	{
		Debug.Log("Dialogue started: "+dialogueId);
		currentDialogue = dialogueId;
		currentText = 0;
        justStartedDialogue = true;
		inDialogue=true;
	}

    string replaceVariables(string dialogueText){
        if (dialogueText != null){
            string newText = dialogueText.Replace("{PlayerName}", "Kunkka");
            return newText;
        }else{
            return dialogueText;
        }
    }
}
