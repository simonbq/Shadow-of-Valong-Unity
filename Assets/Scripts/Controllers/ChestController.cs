using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour {

	public static bool hasKey;
	public static bool hasStick;

    public int failedDialogueId;
    public int dialogueId;

    public int itemRequired;
    public int[] itemsInside;

    private bool opened;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Interact()
    {
        if (!opened)
        {
        	if(hasKey){
	            DialogueController.startDialogue(dialogueId);
	            Debug.Log("Add items from chest to inventory now");
	            opened = true;
	            hasStick = true;
            }else{
            	DialogueController.startDialogue(failedDialogueId);
            }
        }
    }
}
