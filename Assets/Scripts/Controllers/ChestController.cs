using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour {

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
            DialogueController.startDialogue(dialogueId);
            Debug.Log("Add items from chest to inventory now");
            opened = true;
        }
    }
}
