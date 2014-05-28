using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

    //Public variables accessible through editor
    public Texture2D dialogueTexture;
    public GUISkin mSkin;

    //Private variables
    private DialogueContainer dialogues = new DialogueContainer();
    private static bool inDialogue = false;
    private static int currentDialogue;
    private static int currentText;
    private static bool justStartedDialogue;

	// Use this for initialization
	void Start () {
        dialogues = DialogueContainer.Load("dialogue.xml");
        
	}
	
	// Update is called once per frame
	void Update () {
        if (inDialogue && !justStartedDialogue && Input.GetButtonDown("Interact")){
            currentText++;
        }
        if (inDialogue){
            GameController.gameState = GameController.GameState.PAUSED;
        }

        justStartedDialogue = false;
	}

    void OnGUI(){
        

        if (inDialogue){
            if (currentText < dialogues.Dialogues[currentDialogue].Texts.Count){
                string dialogueText = replaceVariables(dialogues.getDialogue(currentDialogue).getText(currentText));

                string speakerId = replaceSpeakerId(dialogues.getDialogue(currentDialogue).getSpeakerId(currentText));

                GUI.Label(new Rect((Screen.width / 2) - getWidth(dialogueTexture.width)/2, Screen.height - getHeight(dialogueTexture.height + 80), getWidth(dialogueTexture.width), getHeight(dialogueTexture.height)), dialogueTexture);
                GUI.skin = mSkin;
                GUI.skin.label.fontSize = Mathf.RoundToInt(getHeight(30));

                GUI.Label(new Rect(Screen.width/2 - getWidth(dialogueTexture.width - 50)/2, Screen.height - getHeight(dialogueTexture.height + 35), getWidth(dialogueTexture.width - 100), getHeight(dialogueTexture.height)), "<color=black>"+speakerId + " says: "+"</color>");
                GUI.skin.label.fontSize = Mathf.RoundToInt(getHeight(40));
                GUI.Label(new Rect(Screen.width / 2 - getWidth(dialogueTexture.width - 30) / 2, Screen.height - getHeight(dialogueTexture.height), getWidth(dialogueTexture.width-100), getHeight(dialogueTexture.height)), "<color=black>"+dialogueText+"</color>");
            }else{
                inDialogue = false;
                GameController.gameState = GameController.GameState.GAME;
            }
        }
    }
    float getHeight(float height)
    {
        return height * Screen.height / 1080;
    }
    float getWidth(float width)
    {
        return width * Screen.width / 1920;
    }

    public static void startDialogue(int dialogueId){
        Debug.Log("Dialogue started: " + dialogueId);
        currentDialogue = dialogueId;
        currentText = 0;
        justStartedDialogue = true;
        inDialogue = true;
    }

    string replaceVariables(string dialogueText){
        if (dialogueText != null){
            string newText = dialogueText.Replace("{PlayerName}", UnitController.getName(0));
            return newText;
        }else{
            return dialogueText;
        }
    }

    public static string replaceSpeakerId(int id){
        string name = UnitController.getName(id);
        if (name == null){
            return string.Empty + "NPC_ID_" + id;
        }else{
            return name;
        }
    }
}
