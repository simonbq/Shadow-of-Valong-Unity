using UnityEngine;
using System.Collections;

public class DialogueController : MonoBehaviour {

    //Public variables accessible through editor
    public Texture2D dialogueTexture;

    //Private variables
    private DialogueContainer dialogues;
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
        if (inDialogue && !justStartedDialogue && Input.GetButtonDown("Interact"))
        {
            currentText++;
        }
        if (inDialogue)
        {
            GameController.gameState = GameController.GameState.PAUSED;
        }

        justStartedDialogue = false;
	}

    void OnGUI()
    {
        if (inDialogue)
        {
            //Debug.Log(dialogues.Dialogues[currentDialogue].Texts.Count);
            if (currentText < dialogues.Dialogues[currentDialogue].Texts.Count)
            {
                string dialogueText = replaceVariables(dialogues.getDialogue(currentDialogue).getText(currentText));

                string speakerId = replaceSpeakerId(dialogues.getDialogue(currentDialogue).getSpeakerId(currentText));

                GUI.Label((new Rect((Screen.width / 2) - (dialogueTexture.width / 2), Screen.height - dialogueTexture.height - 100, dialogueTexture.width, dialogueTexture.height)), dialogueTexture);
                GUI.Label((new Rect((Screen.width / 2) - (dialogueTexture.width / 2) + 100, Screen.height - dialogueTexture.height - 20, dialogueTexture.width, dialogueTexture.height)), speakerId + " says: ");
                GUI.Label((new Rect((Screen.width / 2) - (dialogueTexture.width / 2) + 100, Screen.height - dialogueTexture.height, dialogueTexture.width, dialogueTexture.height)), dialogueText);
            }
            else
            {
                inDialogue = false;
                GameController.gameState = GameController.GameState.GAME;
            }
        }
    }

    public static void startDialogue(int dialogueId)
    {
        Debug.Log("Dialogue started: " + dialogueId);
        currentDialogue = dialogueId;
        currentText = 0;
        justStartedDialogue = true;
        inDialogue = true;
    }

    string replaceVariables(string dialogueText)
    {
        if (dialogueText != null)
        {
            string newText = dialogueText.Replace("{PlayerName}", "Link");
            return newText;
        }
        else
        {
            return dialogueText;
        }
    }

    public static string replaceSpeakerId(int id)
    {
        string oldName = string.Empty + id;
        string name = oldName.Replace("0", "Link");
        name = name.Replace("1", "Gertrude");
        name = name.Replace("2", "Calle");
        name = name.Replace("3", "Dog");

        if (oldName.Equals(name, System.StringComparison.Ordinal))
        {
            return string.Empty + "NPC_ID_" + id;
        }
        else
        {
            return name;
        }
    }
}
