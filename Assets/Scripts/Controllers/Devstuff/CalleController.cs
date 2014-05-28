using UnityEngine;
using System.Collections;

public class CalleController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Interact(){
        if (QuestController.questStarted(5))
        {//insert actual check to see if player has the cheese
            if (QuestController.objectiveCompleted(5, 0)) {
                QuestController.completeObjective(5, 1);
                DialogueController.startDialogue(6);
            }else{
                DialogueController.startDialogue(5);
            }
        }

        if (!QuestController.questStarted(5)){
            DialogueController.startDialogue(2);
            QuestController.startQuest(5);
        }

        
	}
}
